using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class GerenciadorDeVitoria : MonoBehaviour
{
    [Header("Cenas (Build Index)")]

    [Header("Raiz da UI (arraste PainelVitoria aqui)")]
    [SerializeField] private Transform painelVitoria;

    [Header("Textos")]
    public TextMeshProUGUI textoMensagem;
    public TextMeshProUGUI textoPontuacaoFinal;

    [Header("Nome do Jogador")]
    public TMP_InputField inputNome;

    [Header("Ranking (5 linhas)")]
    public TextMeshProUGUI[] textosRanking; // tamanho 5

    private int pontuacaoFinal;
    private bool nomeConfirmado = false;

    private void Awake()
    {
        // Garante configurações corretas ao entrar na cena
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Cria EventSystem caso não exista
        if (EventSystem.current == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));

        // Garante existência do sistema de ranking
        if (SalvadorDePontuacao.Instancia == null)
        {
            GameObject obj = new GameObject("SalvadorDePontuacao");
            obj.AddComponent<SalvadorDePontuacao>();
        }

        // Tenta conectar automaticamente referências da UI
        AutoConectarSePreciso();
    }

    private void Start()
    {
        // Mantém cursor liberado nos primeiros frames
        StartCoroutine(ForcarCursorPorAlgunsFrames());

        // Carrega pontuação salva
        pontuacaoFinal = PlayerPrefs.GetInt("UltimaPontuacao", 0);

        Debug.Log($"[VITORIA] Pontuação carregada: {pontuacaoFinal}");

        // Atualiza texto da pontuação
        if (textoPontuacaoFinal != null)
            textoPontuacaoFinal.text = $"PONTUAÇÃO: {pontuacaoFinal:D6}";

        // Define mensagem com base na pontuação
        if (textoMensagem != null)
        {
            if (pontuacaoFinal >= 5000)
                textoMensagem.text = "INCRÍVEL! DEFESA PERFEITA!";
            else if (pontuacaoFinal >= 2000)
                textoMensagem.text = "ÓTIMO TRABALHO, PILOTO!";
            else
                textoMensagem.text = "A ESTAÇÃO FOI DEFENDIDA!";
        }

        MostrarRanking();
    }

    private IEnumerator ForcarCursorPorAlgunsFrames()
    {
        // Reforça as configurações do cursor
        for (int i = 0; i < 10; i++)
        {
            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            yield return null;
        }
    }

    private void AutoConectarSePreciso()
    {
        // Procura automaticamente o painel principal
        if (painelVitoria == null)
        {
            GameObject go = GameObject.Find("PainelVitoria");

            if (go != null)
                painelVitoria = go.transform;
        }

        if (painelVitoria == null) return;

        // Procura textos automaticamente
        if (textoPontuacaoFinal == null)
            textoPontuacaoFinal = BuscarTMP("TextoPontuacaoFinal");

        if (textoMensagem == null)
            textoMensagem = BuscarTMP("TextoMensagem");

        // Procura input de nome
        if (inputNome == null)
            inputNome = BuscarInput("InputField (TMP)") ?? BuscarInput("InputNome");

        // Procura os textos do ranking
        if (textosRanking == null || textosRanking.Length == 0)
        {
            textosRanking = new TextMeshProUGUI[5];

            for (int i = 0; i < 5; i++)
                textosRanking[i] = BuscarTMP($"TextoRanking{i + 1}");
        }
    }

    private TextMeshProUGUI BuscarTMP(string nome)
    {
        Transform t = painelVitoria.Find(nome);

        return t ? t.GetComponent<TextMeshProUGUI>() : null;
    }

    private TMP_InputField BuscarInput(string nome)
    {
        Transform t = painelVitoria.Find(nome);

        return t ? t.GetComponent<TMP_InputField>() : null;
    }

    // -------------------------
    // FUNÇÕES DOS BOTÕES
    // -------------------------

    public void ConfirmarNome()
    {
        if (nomeConfirmado) return;

        // Verifica se o input foi encontrado
        if (inputNome == null)
        {
            Debug.LogError("[VITORIA] inputNome não encontrado.");
            return;
        }

        string nome = inputNome.text.Trim();

        // Salva pontuação no ranking
        SalvadorDePontuacao.Instancia.SalvarPontuacao(nome, pontuacaoFinal);

        nomeConfirmado = true;

        Debug.Log($"[VITORIA] Salvo: '{nome}' com {pontuacaoFinal} pontos");

        MostrarRanking();

        // Esconde o input após confirmar
        inputNome.gameObject.SetActive(false);
    }

    public void LimparRanking()
    {
        // Remove todas as pontuações salvas
        SalvadorDePontuacao.Instancia?.LimparRanking();

        MostrarRanking();
    }

    public void aMenu()
    {
        // Volta para o menu principal
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void aJogo()
    {
        // Reinicia a partida
        SceneManager.LoadScene("SampleScene");
    }

    private void MostrarRanking()
    {
        if (textosRanking == null || textosRanking.Length == 0) return;
        if (SalvadorDePontuacao.Instancia == null) return;

        // Carrega ranking salvo
        List<(string, int)> ranking = SalvadorDePontuacao.Instancia.CarregarRanking();

        for (int i = 0; i < textosRanking.Length; i++)
        {
            if (textosRanking[i] == null) continue;

            // Exibe jogador no ranking
            if (i < ranking.Count)
                textosRanking[i].text = $"{i + 1}º  {ranking[i].Item1}  {ranking[i].Item2:D6}";
            else
                textosRanking[i].text = $"{i + 1}º  ------";
        }
    }
}