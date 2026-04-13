using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GerenciadorDeVitoria : MonoBehaviour
{
    [Header("Textos")]
    public TextMeshProUGUI textoMensagem;
    public TextMeshProUGUI textoPontuacaoFinal;

    [Header("Nome do Jogador")]
    public TMP_InputField inputNome;
    public Button botaoConfirmar;
    public TextMeshProUGUI textoAviso;

    [Header("Ranking")]
    public TextMeshProUGUI[] textosRanking;

    [Header("Botões")]
    public Button botaoRejogar;
    public Button botaoMenu;
    public Button botaoLimparRanking;

    [Header("Cenas")]
    public string nomeCenaJogo = "SampleScene";
    public string nomeCenaMenu = "Menu";

    private int pontuacaoFinal;
    private bool nomeConfirmado = false;

    private void Start()
    {
        if (SalvadorDePontuacao.Instancia == null)
        {
            GameObject obj = new GameObject("SalvadorDePontuacao");
            obj.AddComponent<SalvadorDePontuacao>();
        }

        pontuacaoFinal = PlayerPrefs.GetInt("UltimaPontuacao", 0);
        Debug.Log($"Pontuação carregada: {pontuacaoFinal}");

        if (textoPontuacaoFinal != null)
            textoPontuacaoFinal.text = $"PONTUAÇÃO: {pontuacaoFinal:D6}";

        if (textoMensagem != null)
        {
            if (pontuacaoFinal >= 5000)
                textoMensagem.text = "INCRÍVEL! DEFESA PERFEITA!";
            else if (pontuacaoFinal >= 2000)
                textoMensagem.text = "ÓTIMO TRABALHO, PILOTO!";
            else
                textoMensagem.text = "A ESTAÇÃO FOI DEFENDIDA!";
        }

        if (textoAviso != null)
            textoAviso.text = "Digite seu nome e confirme para salvar no ranking!";

        MostrarRanking();

        if (botaoConfirmar != null)
            botaoConfirmar.onClick.AddListener(ConfirmarNome);

        if (botaoRejogar != null)
            botaoRejogar.onClick.AddListener(Rejogar);

        if (botaoMenu != null)
            botaoMenu.onClick.AddListener(IrParaMenu);

        if (botaoLimparRanking != null)
            botaoLimparRanking.onClick.AddListener(LimparRanking);
    }

    private void ConfirmarNome()
    {
        if (nomeConfirmado) return;

        if (inputNome == null)
        {
            Debug.LogError("InputNome não conectado no Inspector!");
            return;
        }

        string nome = inputNome.text.Trim();

        if (string.IsNullOrEmpty(nome))
        {
            if (textoAviso != null)
                textoAviso.text = "⚠ Digite seu nome antes de confirmar!";
            return;
        }

        SalvadorDePontuacao.Instancia.SalvarPontuacao(nome, pontuacaoFinal);
        nomeConfirmado = true;

        Debug.Log($"Salvo: '{nome}' com {pontuacaoFinal} pontos");

        MostrarRanking();
        if (textoAviso != null)
            textoAviso.text = $"✓ Salvo! Bem-vindo ao ranking, {nome}!";

        if (inputNome != null)
            inputNome.gameObject.SetActive(false);

        if (botaoConfirmar != null)
            botaoConfirmar.gameObject.SetActive(false);
    }

    private void MostrarRanking()
    {
        if (textosRanking == null || textosRanking.Length == 0) return;
        if (SalvadorDePontuacao.Instancia == null)
        {
            Debug.LogWarning("SalvadorDePontuacao não encontrado!");
            return;
        }

        List<(string, int)> ranking = SalvadorDePontuacao.Instancia.CarregarRanking();

        Debug.Log($"Ranking tem {ranking.Count} entradas");

        for (int i = 0; i < textosRanking.Length; i++)
        {
            if (textosRanking[i] == null) continue;

            if (i < ranking.Count)
            {
                textosRanking[i].text = $"{i + 1}º  {ranking[i].Item1}  {ranking[i].Item2:D6}";
                Debug.Log($"Ranking {i+1}: {ranking[i].Item1} - {ranking[i].Item2}");
            }
            else
            {
                textosRanking[i].text = $"{i + 1}º  ------";
            }
        }
    }

    private void Rejogar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaJogo);
    }

    private void IrParaMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nomeCenaMenu);
    }

    private void LimparRanking()
    {
        if (SalvadorDePontuacao.Instancia != null)
            SalvadorDePontuacao.Instancia.LimparRanking();

        MostrarRanking();
    }
}