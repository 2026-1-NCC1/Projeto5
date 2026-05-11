using UnityEngine;
using System;

public class AdPopup : MonoBehaviour
{
    // Evento chamado quando o popup é fechado
    public event Action<AdPopup> AoFechar;

    [Header("Movimento (opcional)")]
    public bool mover = true;

    // Intensidade do movimento
    public float amplitude = 25f;

    // Velocidade da animação
    public float velocidade = 2f;

    private RectTransform rt;
    private Vector2 posInicial;
    private float fase;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        // Valor aleatório para cada popup ter movimento diferente
        fase = UnityEngine.Random.Range(0f, 10f);
    }

    private void Start()
    {
        // Guarda a posição inicial depois do spawn
        if (rt != null)
            posInicial = rt.anchoredPosition;
    }

    private void Update()
    {
        if (!mover || rt == null) return;

        // Movimento suave usando seno e cosseno
        fase += Time.unscaledDeltaTime * velocidade;

        rt.anchoredPosition = posInicial + new Vector2(
            Mathf.Sin(fase) * amplitude,
            Mathf.Cos(fase * 0.7f) * amplitude
        );
    }

    public void Fechar()
    {
        // Dispara o evento de fechamento
        AoFechar?.Invoke(this);

        // Remove o popup da cena
        Destroy(gameObject);
    }
}