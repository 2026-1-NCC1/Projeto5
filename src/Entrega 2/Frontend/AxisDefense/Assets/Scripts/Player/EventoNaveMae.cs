using UnityEngine;

public class EventoNaveMae : MonoBehaviour
{
    [Header("Referências")]
    public GeradorDeInimigos geradorNormal;

    public NaveMaeBoss prefabNaveMae;

    [Tooltip("4 pontos (Frente, Trás, Esquerda, Direita).")]
    public Transform[] pontosSpawnNaveMae;

    [Header("Gatilho por pontuação")]
    public int pontuacaoParaAparecer = 5000;

    public bool aparecerUmaVez = true;

    [Header("Evento aleatório (opcional)")]
    public bool usarAleatorio = false;

    [Tooltip("Chance por segundo da nave aparecer.")]
    public float chancePorSegundo = 0.002f;

    private bool jaApareceu = false;

    private NaveMaeBoss atual;

    private void Update()
    {
        // Evita criar outra nave enquanto uma já existe
        if (atual != null) return;

        int pontos =
            (GerenciadorDePontuacao.Instancia != null)
            ? GerenciadorDePontuacao.Instancia.Pontuacao
            : 0;

        // Spawn por pontuação
        if (pontos >= pontuacaoParaAparecer &&
            (!aparecerUmaVez || !jaApareceu))
        {
            SpawnarNaveMae();

            return;
        }

        // Spawn aleatório opcional
        if (usarAleatorio &&
            (!aparecerUmaVez || !jaApareceu))
        {
            if (UnityEngine.Random.value <
                chancePorSegundo * Time.deltaTime)
            {
                SpawnarNaveMae();
            }
        }
    }

    private void SpawnarNaveMae()
    {
        if (prefabNaveMae == null) return;

        if (pontosSpawnNaveMae == null ||
            pontosSpawnNaveMae.Length == 0)
            return;

        // Pausa geração normal de inimigos
        if (geradorNormal != null)
            geradorNormal.SetPausado(true);

        // Escolhe um ponto aleatório
        Transform p =
            pontosSpawnNaveMae[
                UnityEngine.Random.Range(
                    0,
                    pontosSpawnNaveMae.Length
                )
            ];

        if (p == null) return;

        // Cria a nave mãe
        atual = Instantiate(
            prefabNaveMae,
            p.position,
            p.rotation
        );

        atual.AoMorrer += OnNaveMaeMorreu;

        jaApareceu = true;

        Debug.Log("[BOSS] Nave Mãe apareceu! Spawns normais pausados.");
    }

    private void OnNaveMaeMorreu(NaveMaeBoss boss)
    {
        // Remove evento antigo
        if (atual != null)
            atual.AoMorrer -= OnNaveMaeMorreu;

        atual = null;

        // Reativa geração normal
        if (geradorNormal != null)
            geradorNormal.SetPausado(false);

        Debug.Log("[BOSS] Nave Mãe destruída! Spawns normais voltaram.");
    }
}