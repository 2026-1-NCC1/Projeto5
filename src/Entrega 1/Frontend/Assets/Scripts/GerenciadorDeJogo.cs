using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorDeJogo : MonoBehaviour
{
    public static GerenciadorDeJogo Instancia { get; private set; }

    //Nicolas depois você verificaa certinho a questão da troca de câmera e pontuação e se precisa arrumar aqui nesse cdogio
    [Header("Interface do Jogador (HUD)")]
    public TextMeshProUGUI textoEscudo;
    public Slider sliderEscudo;
    public TextMeshProUGUI textoCronometro;

    [Header("Painel de Vitória")]
    public GameObject painelVitoria;

    [Header("Configurações da Partida")]
    [Tooltip("Tempo em segundos para vencer a partida")]
    public float tempoDeReparo = 120f;

    private float tempoAtual;
    private bool jogoTerminou = false;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instancia = this;
        }
    }

    private void Start()
    {
        tempoAtual = tempoDeReparo;

        if (painelVitoria != null) painelVitoria.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (jogoTerminou) return;

        tempoAtual -= Time.deltaTime;
        if (tempoAtual <= 0)
        {
            tempoAtual = 0;
            Vitoria();
        }

        textoCronometro.text = "Reparo: " + tempoAtual.ToString("F0") + "s";
    }

    public void AtualizarInterfaceEscudo(int vidaAtual, int vidaMaxima)
    {
        if (textoEscudo != null)
        {
            textoEscudo.text = "Escudo: " + vidaAtual.ToString() + "%";
        }
        if (sliderEscudo != null)
        {
            sliderEscudo.maxValue = vidaMaxima;
            sliderEscudo.value = vidaAtual;
        }
    }

    public void ExibirFimDeJogo()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;

        // Vai direto para a cena de Game Over - Removi o painel, porque estava dando erro de interface
        SceneManager.LoadScene("GameOver");
    }

    private void Vitoria()
{
    if (jogoTerminou) return;
    jogoTerminou = true;

    int pontuacaoFinal = 0;
    if (GerenciadorDePontuacao.Instancia != null)
        pontuacaoFinal = GerenciadorDePontuacao.Instancia.Pontuacao;

    PlayerPrefs.SetInt("UltimaPontuacao", pontuacaoFinal);
    PlayerPrefs.Save();

    Time.timeScale = 1f;
    SceneManager.LoadScene("Vitoria");
}

    private void PausarJogo()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}