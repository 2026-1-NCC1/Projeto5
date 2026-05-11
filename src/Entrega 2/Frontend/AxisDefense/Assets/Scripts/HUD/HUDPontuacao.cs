using UnityEngine;
using TMPro;
using System.Collections;

public class HUDPontuacao : MonoBehaviour
{
    // Instância global da HUD
    public static HUDPontuacao Instancia { get; private set; }

    [Header("Textos (TMP diferentes no Inspector!)")]
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoMultiplicador;
    public TextMeshProUGUI textoCombo;

    [Header("Efeito Shake/Punch")]
    public float duracaoShake = 0.15f;
    public float intensidadeShake = 8f;
    public float escalaPunch = 1.3f;

    private Coroutine rotinaPontuacao;
    private Coroutine rotinaMultiplicador;

    private void Awake()
    {
        // Garante apenas uma instância da HUD
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;

        // Verifica referências repetidas no Inspector
        if (textoCombo != null && textoMultiplicador != null && textoCombo == textoMultiplicador)
            Debug.LogWarning("[HUDPontuacao] ERRO: textoCombo e textoMultiplicador estão no MESMO TMP.");

        if (textoPontuacao != null && textoMultiplicador != null && textoPontuacao == textoMultiplicador)
            Debug.LogWarning("[HUDPontuacao] ERRO: textoPontuacao e textoMultiplicador estão no MESMO TMP.");

        if (textoPontuacao != null && textoCombo != null && textoPontuacao == textoCombo)
            Debug.LogWarning("[HUDPontuacao] ERRO: textoPontuacao e textoCombo estão no MESMO TMP.");
    }

    private void Start()
    {
        // Estado inicial da HUD
        if (textoPontuacao != null)
            textoPontuacao.text = "000000";

        if (textoMultiplicador != null)
            textoMultiplicador.text = "x1";

        if (textoCombo != null)
            textoCombo.text = "COMBO";
    }

    public void Atualizar(int pontos, int multiplicador, int combo)
    {
        // Atualiza pontuação
        if (textoPontuacao != null)
            textoPontuacao.text = pontos.ToString("D6");

        // Atualiza multiplicador
        if (textoMultiplicador != null)
            textoMultiplicador.text = $"x{Mathf.Max(1, multiplicador)}";

        // Mantém texto de combo visível
        if (textoCombo != null)
            textoCombo.text = "COMBO";

        // Aplica efeito na pontuação
        if (textoPontuacao != null)
        {
            if (rotinaPontuacao != null)
                StopCoroutine(rotinaPontuacao);

            rotinaPontuacao = StartCoroutine(ShakeTexto(textoPontuacao));
        }

        // Aplica efeito no multiplicador
        if (textoMultiplicador != null)
        {
            if (rotinaMultiplicador != null)
                StopCoroutine(rotinaMultiplicador);

            rotinaMultiplicador = StartCoroutine(PunchTexto(textoMultiplicador));
        }
    }

    private IEnumerator ShakeTexto(TextMeshProUGUI tmp)
    {
        RectTransform rt = tmp.rectTransform;

        Vector2 posOriginal = rt.anchoredPosition;
        Vector3 escalaOriginal = rt.localScale;

        // Aumenta o texto temporariamente
        rt.localScale = escalaOriginal * escalaPunch;

        float t = 0f;

        while (t < duracaoShake)
        {
            t += Time.deltaTime;

            float progresso = t / duracaoShake;
            float shake = (1f - progresso);

            // Movimento aleatório do texto
            rt.anchoredPosition = posOriginal + new Vector2(
                Random.Range(-intensidadeShake, intensidadeShake) * shake,
                Random.Range(-intensidadeShake, intensidadeShake) * shake
            );

            rt.localScale = Vector3.Lerp(rt.localScale, escalaOriginal, Time.deltaTime * 12f);

            yield return null;
        }

        // Restaura posição e escala original
        rt.anchoredPosition = posOriginal;
        rt.localScale = escalaOriginal;
    }

    private IEnumerator PunchTexto(TextMeshProUGUI tmp)
    {
        RectTransform rt = tmp.rectTransform;

        Vector3 escalaOriginal = rt.localScale;

        // Dá um pequeno aumento no texto
        rt.localScale = escalaOriginal * escalaPunch;

        float t = 0f;

        while (t < 0.2f)
        {
            t += Time.deltaTime;

            rt.localScale = Vector3.Lerp(rt.localScale, escalaOriginal, Time.deltaTime * 15f);

            yield return null;
        }

        rt.localScale = escalaOriginal;
    }
}