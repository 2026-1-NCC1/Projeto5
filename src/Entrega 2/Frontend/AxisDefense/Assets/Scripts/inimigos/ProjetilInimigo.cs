using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjetilInimigo : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float velocidade = 15f;
    public int dano = 5;
    public float tempoDeVida = 5f;

    private Vector3 direcao;

    public void Inicializar(Vector3 direcaoDisparo)
    {
        // Define direção do disparo
        direcao = direcaoDisparo.normalized;

        // Destroi o projétil após um tempo
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        // Move o projétil continuamente
        transform.position += direcao * velocidade * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider outro)
    {
        // Verifica colisão com o jogador
        if (outro.CompareTag("Player"))
        {
            Debug.Log("Projétil do INIMIGO acertou o jogador!");

            VidaDoJogador vidaJogador =
                outro.GetComponent<VidaDoJogador>();

            // Aplica dano no jogador
            if (vidaJogador != null)
            {
                vidaJogador.SofrerDano(dano);
            }

            // Remove o projétil após acertar
            Destroy(gameObject);
        }
    }
}