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
        direcao = direcaoDisparo.normalized;
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        transform.position += direcao * velocidade * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Player"))
        {
            Debug.Log("Projétil do INIMIGO acertou o jogador!");

            VidaDoJogador vidaJogador = outro.GetComponent<VidaDoJogador>();

            if (vidaJogador != null)
            {
                vidaJogador.SofrerDano(dano);
            }
            
            Destroy(gameObject);
        }
    }
}