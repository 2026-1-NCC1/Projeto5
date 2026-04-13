using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GerenciadorDeJogo : MonoBehaviour
{
    public static GerenciadorDeJogo Instancia { get; private set; }

    [Header("Interface do Jogador (HUD)")]
    public TextMeshProUGUI textoEscudo;
    public Slider sliderEscudo;
    public TextMeshProUGUI textoCronometro;

    [Header("Configurações da Partida")]
    [Tooltip("Tempo em segundos para vencer a partida")]
    public float tempoDeReparo = 120f;

    [Header("Nomes das Cenas")]
    [Tooltip("O nome EXATO da sua cena de Game Over")]
    public string nomeCenaFimDeJogo = "TelaFimDeJogo";
    [Tooltip("O nome EXATO da sua cena de Vitória")]
    public string nomeCenaVitoria = "TelaVitoria";

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
        Debug.Log("FIM DE JOGO! Escudo destruído!");
        PausarJogo();
        SceneManager.LoadScene(nomeCenaFimDeJogo);
    }

    private void Vitoria()
    {
        if (jogoTerminou) return;
        jogoTerminou = true;
        Debug.Log("VITÓRIA! Nave reparada!");
        PausarJogo();
        SceneManager.LoadScene(nomeCenaVitoria);
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