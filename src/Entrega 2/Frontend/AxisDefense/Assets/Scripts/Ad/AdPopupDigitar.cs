using UnityEngine;
using TMPro;
using System;

public class AdPopupDigitar : MonoBehaviour
{
    // Evento chamado ao fechar o popup
    public event Action<AdPopupDigitar> AoFechar;

    [Header("Referências")]
    public TextMeshProUGUI textoDesafio;
    public TMP_InputField input;

    private string codigoAtual;

    private void Start()
    {
        // Gera o primeiro código ao iniciar
        GerarCodigo();
    }

    private void GerarCodigo()
    {
        // Cria um código aleatório de 3 números
        codigoAtual = UnityEngine.Random.Range(100, 999).ToString();

        // Atualiza o texto do desafio
        if (textoDesafio != null)
            textoDesafio.text = $"DIGITE: {codigoAtual}";

        // Limpa o campo de input
        if (input != null)
            input.text = "";
    }

    // Ligue no botão OK
    public void TentarConfirmar()
    {
        if (input == null) return;

        // Fecha o popup se o código estiver correto
        if (input.text.Trim() == codigoAtual)
        {
            AoFechar?.Invoke(this);
            Destroy(gameObject);
        }
        else
        {
            // Se errar, gera um novo código
            GerarCodigo();
        }
    }
}