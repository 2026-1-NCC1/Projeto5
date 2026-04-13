using UnityEngine;

public class ControladorDeMira : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Arraste o objeto da mira (Crosshair) aqui.")]
    public RectTransform retanguloMira;

    [Header("Configurações de Movimento")]
    [Tooltip("Velocidade de suavização do movimento da mira.")]
    public float velocidadeMira = 8f;

    [Header("Limites de Movimento")]
    [Tooltip("Limite horizontal da mira na tela.")]
    public float limiteHorizontal = 400f;
    [Tooltip("Limite vertical da mira na tela.")]
    public float limiteVertical = 300f;

    private Vector2 posicaoAlvo;

    private void Update()
    {
        Vector2 posicaoMouse = Input.mousePosition;

        Vector2 posicaoCanvas;
        posicaoCanvas.x = posicaoMouse.x - Screen.width / 2f;
        posicaoCanvas.y = posicaoMouse.y - Screen.height / 2f;

        posicaoCanvas.x = Mathf.Clamp(posicaoCanvas.x, -limiteHorizontal, limiteHorizontal);
        posicaoCanvas.y = Mathf.Clamp(posicaoCanvas.y, -limiteVertical, limiteVertical);

        posicaoAlvo = Vector2.Lerp(
            retanguloMira.anchoredPosition,
            posicaoCanvas,
            velocidadeMira * Time.deltaTime
        );

        retanguloMira.anchoredPosition = posicaoAlvo;
    }

    public Vector2 ObterPosicaoMira()
    {
        return retanguloMira.anchoredPosition;
    }
}