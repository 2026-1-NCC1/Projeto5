using UnityEngine;
using UnityEngine.UI;

public class BarreiraDoJogador : MonoBehaviour
{
    [Header("Controle")]
    [Tooltip("Segure o botão direito do mouse para ativar a barreira.")]
    public int botaoMouse = 1; // 1 = botão direito

    [Header("Energia / Recarga")]
    public int energiaMaxima = 50;
    public float recargaSegundos = 8f;

    [Header("UI (opcional)")]
    public Graphic iconeBarreira; // Image ou RawImage
    public Slider barraEnergia;

    public bool EstaAtiva { get; private set; }
    public bool EmRecarga => tempoRecargaRestante > 0f;
    public int EnergiaAtual => energiaAtual;

    private int energiaAtual;
    private float tempoRecargaRestante;

    // Evita reativar automaticamente enquanto o jogador mantém o botão pressionado
    private bool bloqueadoAteSoltar = false;

    private void Awake()
    {
        energiaAtual = energiaMaxima;
        EstaAtiva = false;
        tempoRecargaRestante = 0f;

        AtualizarUI();
    }

    private void Update()
    {
        // Recarga
        if (tempoRecargaRestante > 0f)
        {
            tempoRecargaRestante -= Time.deltaTime;
            if (tempoRecargaRestante <= 0f)
            {
                tempoRecargaRestante = 0f;
                energiaAtual = energiaMaxima;
            }
            AtualizarUI();
        }

        // HOLD: ativa enquanto estiver segurando o botão
        bool segurando = Input.GetMouseButton(botaoMouse);

        if (!segurando)
        {
            bloqueadoAteSoltar = false;
            if (EstaAtiva) Desativar();
            return;
        }

        // Se zerou e entrou em recarga, só reativa depois de soltar e segurar de novo
        if (bloqueadoAteSoltar) return;

        // Tenta ativar (só ativa se não estiver em recarga e tiver energia)
        TentarAtivar();
    }

    public void TentarAtivar()
    {
        if (EstaAtiva) return;
        if (EmRecarga) return;
        if (energiaAtual <= 0) return;

        EstaAtiva = true;
        AtualizarUI();
    }

    public void Desativar()
    {
        EstaAtiva = false;
        AtualizarUI();
    }

    // Consome energia e devolve o dano restante para ir para a vida
    public int ProcessarDano(int dano)
    {
        if (dano <= 0) return 0;
        if (!EstaAtiva) return dano;
        if (energiaAtual <= 0) return dano;

        int consumido = Mathf.Min(energiaAtual, dano);
        energiaAtual -= consumido;

        int restante = dano - consumido;

        // Se zerou: desativa e inicia recarga
        if (energiaAtual <= 0)
        {
            energiaAtual = 0;
            EstaAtiva = false;
            tempoRecargaRestante = recargaSegundos;

            // impede reativar enquanto o botão continuar segurado
            bloqueadoAteSoltar = true;
        }

        AtualizarUI();
        return restante;
    }

    private void AtualizarUI()
    {
        if (iconeBarreira != null)
            iconeBarreira.gameObject.SetActive(EstaAtiva);

        if (barraEnergia != null)
        {
            barraEnergia.maxValue = energiaMaxima;
            barraEnergia.value = energiaAtual;
            barraEnergia.gameObject.SetActive(EstaAtiva || EmRecarga);
        }
    }
}