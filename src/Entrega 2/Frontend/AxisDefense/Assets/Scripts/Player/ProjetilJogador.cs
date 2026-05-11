using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjetilJogador : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float velocidade = 100f;

    public float tempoDeVida = 3f;

    [Header("Dano")]
    public int danoNoBoss = 10;

    private void Start()
    {
        // Remove o projétil após alguns segundos
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        // Move o projétil para frente
        transform.Translate(
            Vector3.forward * velocidade * Time.deltaTime
        );
    }

    private void OnTriggerEnter(Collider outro)
    {
        // Ignora colisões que não sejam inimigos
        if (!outro.CompareTag("Enemy"))
            return;

        // Verifica se acertou a Nave Mãe
        NaveMaeBoss boss =
            outro.GetComponentInParent<NaveMaeBoss>();

        if (boss != null)
        {
            // Aplica dano no boss
            boss.SofrerDano(danoNoBoss);

            Destroy(gameObject);

            return;
        }

        // Inimigo comum
        GameObject inimigo = outro.gameObject;

        // Busca quantidade de pontos do inimigo
        DadosInimigo dados =
            inimigo.GetComponent<DadosInimigo>();

        int pontos =
            dados != null
            ? dados.pontosPorMorte
            : 100;

        // Adiciona pontuação
        if (GerenciadorDePontuacao.Instancia != null)
            GerenciadorDePontuacao.Instancia.RegistrarKill(pontos);

        // Remove inimigo e projétil
        Destroy(inimigo);

        Destroy(gameObject);
    }
}