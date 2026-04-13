using UnityEngine;

public class VidaDoJogador : MonoBehaviour
{
    public static VidaDoJogador Instancia { get; private set; }

    [Header("Configurações de Vida")]
    public int vidaMaxima = 100;
    private int vidaAtual;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
            Destroy(gameObject);
        else
            Instancia = this;
    }

    private void Start()
    {
        vidaAtual = vidaMaxima;

        if (GerenciadorDeJogo.Instancia != null)
            GerenciadorDeJogo.Instancia.AtualizarInterfaceEscudo(vidaAtual, vidaMaxima);
    }

    public void SofrerDano(int dano)
    {
        vidaAtual -= dano;
        vidaAtual = Mathf.Clamp(vidaAtual, 0, vidaMaxima);

        if (GerenciadorDeJogo.Instancia != null)
            GerenciadorDeJogo.Instancia.AtualizarInterfaceEscudo(vidaAtual, vidaMaxima);

        if (vidaAtual <= 0)
            Morrer();
    }

    private void Morrer()
    {
        if (GerenciadorDeJogo.Instancia != null)
            GerenciadorDeJogo.Instancia.ExibirFimDeJogo();
    }
}