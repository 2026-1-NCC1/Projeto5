using UnityEngine;

public class RotacaoAutomaticaCamera : MonoBehaviour
{
    [Header("Configurações de Balanço")]

    [Tooltip("Velocidade do balanço da câmera.")]
    public float velocidadeBalanco = 0.5f;

    [Tooltip("Intensidade do balanço horizontal.")]
    public float intensidadeHorizontal = 15f;

    [Tooltip("Intensidade do balanço vertical.")]
    public float intensidadeVertical = 5f;

    private Quaternion rotacaoInicial;
    private float contador;

    private void Start()
    {
        // Guarda a rotação inicial da câmera
        rotacaoInicial = transform.localRotation;

        // Valor aleatório para evitar movimentos iguais
        contador = Random.Range(0f, 100f);
    }

    private void Update()
    {
        // Atualiza o tempo do balanço
        contador += Time.deltaTime * velocidadeBalanco;

        // Calcula os ângulos do movimento
        float anguloHorizontal = Mathf.Sin(contador) * intensidadeHorizontal;
        float anguloVertical = Mathf.Cos(contador * 0.7f) * intensidadeVertical;

        // Aplica a rotação suavemente
        transform.localRotation = rotacaoInicial *
                                  Quaternion.Euler(anguloVertical, anguloHorizontal, 0f);
    }
}