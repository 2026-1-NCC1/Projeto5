using UnityEngine;

public class GerenciadorDePontuacao : MonoBehaviour
{
    public static GerenciadorDePontuacao Instancia { get; private set; }

    [Header("Configurações de Combo")]
    public float tempoParaPerderCombo = 2f;
    public int multiplicadorMaximo = 8;

    public int Pontuacao { get; private set; }
    public int Multiplicador { get; private set; } = 1;
    public int Combo { get; private set; } = 0;

    private float timerCombo;
    private bool comboAtivo;

    private void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }

    private void Update()
    {
        if (!comboAtivo) return;

        timerCombo -= Time.deltaTime;
        if (timerCombo <= 0f)
            ResetarCombo();
    }

    public void RegistrarKill(int pontos)
    {
        Combo++;
        Multiplicador = Mathf.Clamp(Combo, 1, multiplicadorMaximo);
        timerCombo = tempoParaPerderCombo;
        comboAtivo = true;

        int pontosGanhos = pontos * Multiplicador;
        Pontuacao += pontosGanhos;

        Debug.Log($"KILL! +{pontosGanhos} | Combo: {Combo} | x{Multiplicador} | Total: {Pontuacao}");

        if (HUDPontuacao.Instancia != null)
            HUDPontuacao.Instancia.Atualizar(Pontuacao, Multiplicador, Combo);
    }

    public void ResetarCombo()
    {
        Combo = 0;
        Multiplicador = 1;
        comboAtivo = false;
        timerCombo = 0f;

        if (HUDPontuacao.Instancia != null)
            HUDPontuacao.Instancia.Atualizar(Pontuacao, Multiplicador, Combo);
    }
}