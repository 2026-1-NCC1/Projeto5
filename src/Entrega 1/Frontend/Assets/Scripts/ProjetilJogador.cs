using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjetilJogador : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    public float velocidade = 50f;
    public float tempoDeVida = 3f;

    private void Start()
    {
        Destroy(gameObject, tempoDeVida);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider outro)
    {
        if (outro.CompareTag("Enemy"))
        {
            DadosInimigo dados = outro.GetComponent<DadosInimigo>();
            int pontos = dados != null ? dados.pontosPorMorte : 100;

            if (GerenciadorDePontuacao.Instancia != null)
                GerenciadorDePontuacao.Instancia.RegistrarKill(pontos);
            else
                Debug.LogWarning("GerenciadorDePontuacao não encontrado na cena!");

            Destroy(outro.gameObject);
            Destroy(gameObject);
        }
    }
}