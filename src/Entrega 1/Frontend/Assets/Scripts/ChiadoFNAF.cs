using UnityEngine;
using UnityEngine.UI;

public class ChiadoFNAF : MonoBehaviour
{
    [Header("Texturas do Chiado")]
    [Tooltip("Arraste aqui as imagens (texturas) que serão animadas em loop.")]
    public Texture[] texturasDeChiado;

    [Header("Configurações da Animação")]
    [Tooltip("Tempo em segundos entre cada troca de imagem. Menor = mais rápido.")]
    public float velocidadeDaTroca = 0.05f;

    private RawImage imagemCrua;
    private int indiceImagemAtual = 0;
    private float contador = 0f;

    private void Start()
    {
        imagemCrua = GetComponent<RawImage>();
        if (imagemCrua == null)
        {
            Debug.LogError("O script ChiadoFNAF precisa estar no mesmo objeto que um componente RawImage.", gameObject);
            this.enabled = false;
        }
    }

    private void Update()
    {
        if (texturasDeChiado.Length == 0) return;

        contador += Time.deltaTime;

        if (contador >= velocidadeDaTroca)
        {
            contador = 0f;
            indiceImagemAtual = (indiceImagemAtual + 1) % texturasDeChiado.Length;
            imagemCrua.texture = texturasDeChiado[indiceImagemAtual];
        }
    }
}