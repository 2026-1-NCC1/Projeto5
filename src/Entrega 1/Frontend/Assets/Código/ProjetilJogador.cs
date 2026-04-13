using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ProjetilJogador : MonoBehaviour
{
    [Header("Configurações do Projétil")]
    [Tooltip("A velocidade de avanço do projétil.")]
    public float velocidade = 50f;
    [Tooltip("Tempo em segundos antes do projétil se autodestruir.")]
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
            Destroy(outro.gameObject);
            Destroy(gameObject);
        }
    }
}