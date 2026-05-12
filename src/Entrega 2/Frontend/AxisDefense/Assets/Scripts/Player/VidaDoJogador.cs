using UnityEngine;

public class VidaDoJogador : MonoBehaviour
{
    public static VidaDoJogador Instancia { get; private set; }

    [Header("Configurações de Vida")]
    public int vidaMaxima = 100;

    private int vidaAtual;

    private BarreiraDoJogador barreira;

    private void Awake()
    {
        // Garante apenas uma instância do jogador
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);

            return;
        }

        Instancia = this;

        // Busca a barreira no mesmo objeto
        barreira = GetComponent<BarreiraDoJogador>();
    }

    private void Start()
    {
        // Define vida inicial
        vidaAtual = vidaMaxima;

        // Atualiza HUD
        if (GerenciadorDeJogo.Instancia != null)
        {
            GerenciadorDeJogo.Instancia.AtualizarInterfaceEscudo(
                vidaAtual,
                vidaMaxima
            );
        }
    }

    public void SofrerDano(int dano)
    {
        if (dano <= 0) return;

        // Barreira absorve o dano primeiro
        if (barreira != null)
            dano = barreira.ProcessarDano(dano);

        // Sai caso a barreira absorva tudo
        if (dano <= 0) return;

        // Reduz vida do jogador
        vidaAtual -= dano;

        vidaAtual = Mathf.Clamp(
            vidaAtual,
            0,
            vidaMaxima
        );

        // Atualiza interface
        if (GerenciadorDeJogo.Instancia != null)
        {
            GerenciadorDeJogo.Instancia.AtualizarInterfaceEscudo(
                vidaAtual,
                vidaMaxima
            );
        }

        // Verifica morte
        if (vidaAtual <= 0)
            Morrer();
    }

    private void Morrer()
    {
        // Exibe tela de fim de jogo
        if (GerenciadorDeJogo.Instancia != null)
            GerenciadorDeJogo.Instancia.ExibirFimDeJogo();
    }

    public void Curar(int qtd)
    {
        if (qtd <= 0) return;

        // Recupera vida
        vidaAtual += qtd;

        vidaAtual = Mathf.Clamp(
            vidaAtual,
            0,
            vidaMaxima
        );

        // Atualiza interface
        if (GerenciadorDeJogo.Instancia != null)
        {
            GerenciadorDeJogo.Instancia.AtualizarInterfaceEscudo(
                vidaAtual,
                vidaMaxima
            );
        }
    }
}