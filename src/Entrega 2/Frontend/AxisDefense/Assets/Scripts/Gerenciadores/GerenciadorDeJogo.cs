using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorDeJogo : MonoBehaviour
{
    // Instância global do gerenciador
    public static GerenciadorDeJogo Instancia { get; private set; }

    [Header("Interface do Jogador (HUD)")]
    public TextMeshProUGUI textoEscudo;
    public TextMeshProUGUI textoCronometro;

    [Header("Configurações da Partida")]

    [Tooltip("Tempo em segundos para vencer a partida")]
    public float tempoDeReparo = 120f;

    private float tempoAtual;
    private bool jogoTerminou = false;

    private void Awake()
    {
        // Garante apenas uma instância do gerenciador
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
    }

    private void Start()
    {
        // Define o tempo inicial da partida
        tempoAtual = tempoDeReparo;

        // Estado padrão durante o jogo
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (jogoTerminou) return;

        // Atualiza o cronômetro
        tempoAtual -= Time.deltaTime;

        // Verifica condição de vitória
        if (tempoAtual <= 0f)
        {
            tempoAtual = 0f;
            Vitoria();
        }

        // Atualiza texto do tempo na interface
        if (textoCronometro != null)
            textoCronometro.text = "Reparo: " + tempoAtual.ToString("F0") + "s";
    }

    public void AtualizarInterfaceEscudo(int vidaAtual, int vidaMaxima)
    {
        // Atualiza o valor do escudo na HUD
        if (textoEscudo != null)
            textoEscudo.text = "Escudo: " + vidaAtual + "%";
    }

    public void ExibirFimDeJogo()
    {
        if (jogoTerminou) return;

        jogoTerminou = true;

        // Libera o mouse antes de trocar de cena
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void Vitoria()
    {
        if (jogoTerminou) return;

        jogoTerminou = true;

        // Salva a pontuação final
        int pontuacaoFinal = 0;

        if (GerenciadorDePontuacao.Instancia != null)
            pontuacaoFinal = GerenciadorDePontuacao.Instancia.Pontuacao;

        PlayerPrefs.SetInt("UltimaPontuacao", pontuacaoFinal);
        PlayerPrefs.Save();

        Debug.Log($"[VITORIA] Pontuação salva: {pontuacaoFinal}");

        // Libera o mouse antes de trocar de cena
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("Vitoria", LoadSceneMode.Single);
    }
}