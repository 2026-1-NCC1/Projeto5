using UnityEngine;

public class GerenciadorDePontuacao : MonoBehaviour
{
    // Instância global do sistema de pontuação
    public static GerenciadorDePontuacao Instancia { get; private set; }

    [Header("Configurações de Combo")]

    // Tempo máximo sem eliminar inimigos antes de perder o combo
    public float tempoParaPerderCombo = 2f;

    // Limite do multiplicador
    public int multiplicadorMaximo = 8;

    public int Pontuacao { get; private set; }

    // Multiplicador atual de pontos
    public int Multiplicador { get; private set; } = 1;

    // Quantidade atual do combo
    public int Combo { get; private set; } = 0;

    private float timerCombo;
    private bool comboAtivo;

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

    private void Update()
    {
        if (!comboAtivo) return;

        // Diminui o tempo restante do combo
        timerCombo -= Time.deltaTime;

        if (timerCombo <= 0f)
            ResetarCombo();
    }

    public void RegistrarKill(int pontos)
    {
        // Atualiza valores do combo
        Combo++;

        Multiplicador = Mathf.Clamp(Combo, 1, multiplicadorMaximo);

        timerCombo = tempoParaPerderCombo;
        comboAtivo = true;

        // Calcula os pontos recebidos
        int pontosGanhos = pontos * Multiplicador;

        Pontuacao += pontosGanhos;

        Debug.Log($"KILL! +{pontosGanhos} | Combo: {Combo} | x{Multiplicador} | Total: {Pontuacao}");

        // Atualiza a interface da HUD
        if (HUDPontuacao.Instancia != null)
            HUDPontuacao.Instancia.Atualizar(Pontuacao, Multiplicador, Combo);
    }

    public void ResetarCombo()
    {
        // Reseta os valores do combo
        Combo = 0;
        Multiplicador = 1;

        comboAtivo = false;
        timerCombo = 0f;

        // Atualiza a HUD após perder o combo
        if (HUDPontuacao.Instancia != null)
            HUDPontuacao.Instancia.Atualizar(Pontuacao, Multiplicador, Combo);
    }
}