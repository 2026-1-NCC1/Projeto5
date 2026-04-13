using UnityEngine;
using TMPro;
using System.Collections;

public class HUDPontuacao : MonoBehaviour
{
    public static HUDPontuacao Instancia { get; private set; }

    [Header("Textos")]
    public TextMeshProUGUI textoPontuacao;
    public TextMeshProUGUI textoMultiplicador;
    public TextMeshProUGUI textoCombo;

    [Header("Efeito Shake")]
    public float duracaoShake = 0.15f;
    public float intensidadeShake = 8f;
    public float escalaPunch = 1.3f;

    private Coroutine rotinaPontuacao;
    private Coroutine rotinaMultiplicador;

    private void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }

    public void Atualizar(int pontos, int multiplicador, int combo)
    {
        if (textoPontuacao != null)
            textoPontuacao.text = pontos.ToString("D6");

        if (textoMultiplicador != null)
            textoMultiplicador.text = multiplicador > 1 ? $"x{multiplicador}" : "";

        if (textoCombo != null)
            textoCombo.text = combo > 1 ? $"COMBO {combo}" : "";

        // Shake na pontuação sempre
        if (textoPontuacao != null)
        {
            if (rotinaPontuacao != null) StopCoroutine(rotinaPontuacao);
            rotinaPontuacao = StartCoroutine(ShakeTexto(textoPontuacao));
        }

        if (multiplicador > 1 && textoMultiplicador != null)
        {
            if (rotinaMultiplicador != null) StopCoroutine(rotinaMultiplicador);
            rotinaMultiplicador = StartCoroutine(PunchTexto(textoMultiplicador));
        }
    }

    private IEnumerator ShakeTexto(TextMeshProUGUI tmp)
    {
        RectTransform rt = tmp.rectTransform;
        Vector2 posOriginal = rt.anchoredPosition;
        Vector3 escalaOriginal = rt.localScale;

        rt.localScale = escalaOriginal * escalaPunch;

        float t = 0f;
        while (t < duracaoShake)
        {
            t += Time.deltaTime;

            float progresso = t / duracaoShake;
            float shake = (1f - progresso);

            rt.anchoredPosition = posOriginal + new Vector2(
                Random.Range(-intensidadeShake, intensidadeShake) * shake,
                Random.Range(-intensidadeShake, intensidadeShake) * shake
            );

            rt.localScale = Vector3.Lerp(
                rt.localScale,
                escalaOriginal,
                Time.deltaTime * 12f
            );

            yield return null;
        }

        rt.anchoredPosition = posOriginal;
        rt.localScale = escalaOriginal;
    }

    private IEnumerator PunchTexto(TextMeshProUGUI tmp)
    {
        RectTransform rt = tmp.rectTransform;
        Vector3 escalaOriginal = rt.localScale;

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