using UnityEngine;
using System;

public class NaveMaeBoss : MonoBehaviour
{
    // Evento chamado quando o boss morre
    public event Action<NaveMaeBoss> AoMorrer;

    [Header("Vida")]
    public int vidaMaxima = 80;
    public int pontosAoMorrer = 2000;

    [Header("Ataque")]
    public float intervaloTiro = 10f;
    public int danoPorTiro = 15;

    [Tooltip("Se quiser usar projétil, coloque o prefab aqui.")]
    public GameObject prefabProjetil;

    public Transform pontoTiro;
    public float velocidadeProjetil = 25f;

    [Header("Spawns (filhos da Nave Mãe)")]
    public GameObject modeloKamikaze;
    public GameObject modeloShooter;

    [Range(0f, 1f)]
    public float chanceShooter = 0.35f;

    public float intervaloSpawn = 2.2f;

    // Pontos de saída dos inimigos
    public Transform[] pontosDeSpawn;

    private int vidaAtual;

    private float tTiro;
    private float tSpawn;

    private Transform alvo;

    private void OnEnable()
    {
        // Registra a nave no radar
        if (Radar.Instancia != null)
            Radar.Instancia.RegistrarInimigo(transform);
    }

    private void OnDisable()
    {
        // Remove a nave do radar
        if (Radar.Instancia != null)
            Radar.Instancia.DesregistrarInimigo(transform);
    }

    private void Start()
    {
        // Inicializa valores
        vidaAtual = vidaMaxima;

        tTiro = UnityEngine.Random.Range(0f, intervaloTiro);
        tSpawn = UnityEngine.Random.Range(0f, intervaloSpawn);

        // Procura o jogador
        if (VidaDoJogador.Instancia != null)
            alvo = VidaDoJogador.Instancia.transform;
    }

    private void Update()
    {
        if (alvo == null) return;

        // Controle do ataque
        tTiro += Time.deltaTime;

        if (tTiro >= intervaloTiro)
        {
            tTiro = 0f;

            Atirar();
        }

        // Controle de spawn de inimigos
        tSpawn += Time.deltaTime;

        if (tSpawn >= intervaloSpawn)
        {
            tSpawn = 0f;

            SpawnarInimigo();
        }
    }

    public void SofrerDano(int dano)
    {
        if (dano <= 0) return;

        // Reduz vida do boss
        vidaAtual -= dano;

        if (vidaAtual <= 0)
            Morrer();
    }

    private void Morrer()
    {
        // Adiciona pontuação ao derrotar o boss
        if (GerenciadorDePontuacao.Instancia != null)
            GerenciadorDePontuacao.Instancia.RegistrarKill(pontosAoMorrer);

        AoMorrer?.Invoke(this);

        Destroy(gameObject);
    }

    private void Atirar()
    {
        // Dano direto caso não exista projétil
        if (prefabProjetil == null)
        {
            if (VidaDoJogador.Instancia != null)
                VidaDoJogador.Instancia.SofrerDano(danoPorTiro);

            return;
        }

        // Define origem do disparo
        Transform origem =
            pontoTiro != null ? pontoTiro : transform;

        Vector3 dir =
            (alvo.position - origem.position).normalized;

        // Cria o projétil
        GameObject go = Instantiate(
            prefabProjetil,
            origem.position,
            Quaternion.LookRotation(dir)
        );

        // Configura o projétil
        ProjetilInimigo p = go.GetComponent<ProjetilInimigo>();

        if (p != null)
        {
            p.velocidade = velocidadeProjetil;

            p.Inicializar(dir);
        }
    }

    private void SpawnarInimigo()
    {
        if (pontosDeSpawn == null || pontosDeSpawn.Length == 0) return;
        if (modeloKamikaze == null) return;

        // Escolhe um ponto de spawn aleatório
        Transform saida =
            pontosDeSpawn[UnityEngine.Random.Range(0, pontosDeSpawn.Length)];

        // Decide qual inimigo será criado
        bool gerarShooter =
            (modeloShooter != null) &&
            UnityEngine.Random.value < chanceShooter;

        GameObject prefab =
            gerarShooter ? modeloShooter : modeloKamikaze;

        // Cria o inimigo
        Instantiate(prefab, saida.position, saida.rotation);
    }
}