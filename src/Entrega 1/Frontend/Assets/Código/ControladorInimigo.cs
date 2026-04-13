using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ControladorInimigo : MonoBehaviour
{
    [Header("Configurações Kamikaze")]
    public float velocidadeMovimento = 8f;
    public int danoKamikaze = 10;

    [Header("Movimento Dinâmico")]
    [Tooltip("Força do zigue-zague para os lados")]
    public float velocidadeZigueZagueHorizontal = 4f;
    [Tooltip("Força do zigue-zague para cima e para baixo")]
    public float velocidadeZigueZagueVertical = 3f;
    [Tooltip("Velocidade da mudança de direção do zigue-zague")]
    public float frequenciaZigueZague = 2f;

    private Rigidbody corpoRigido;
    private Transform alvoDoJogador;
    private float contadorZigueZague;

    private void OnEnable()
    {
        // Aguarda um frame para garantir que o Radar.Instancia já exista
        Invoke(nameof(RegistrarNoRadar), 0.01f);
    }

    private void OnDisable()
    {
        if (Radar.Instancia != null)
        {
            Radar.Instancia.DesregistrarInimigo(transform);
        }
    }

    private void RegistrarNoRadar()
    {
        if (Radar.Instancia != null)
        {
            Radar.Instancia.RegistrarInimigo(transform);
        }
    }
    
    private void Start()
    {
        corpoRigido = GetComponent<Rigidbody>();
        contadorZigueZague = Random.Range(0f, 2f * Mathf.PI);

        if (VidaDoJogador.Instancia != null)
        {
            alvoDoJogador = VidaDoJogador.Instancia.transform;
        }
        else
        {
            Debug.LogError("ERRO: Inimigo não encontrou a instância de VidaDoJogador!");
            this.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (alvoDoJogador == null) return;

        Vector3 direcaoParaFrente = (alvoDoJogador.position - transform.position).normalized;
        Vector3 direcaoHorizontal = transform.right;
        Vector3 direcaoVertical = transform.up;

        contadorZigueZague += Time.deltaTime * frequenciaZigueZague;

        float forcaHorizontal = Mathf.Sin(contadorZigueZague) * velocidadeZigueZagueHorizontal;
        float forcaVertical = Mathf.Cos(contadorZigueZague) * velocidadeZigueZagueVertical;

        Vector3 velocidadeFinal =
            (direcaoParaFrente * velocidadeMovimento) +
            (direcaoHorizontal * forcaHorizontal) +
            (direcaoVertical * forcaVertical);

        corpoRigido.linearVelocity = velocidadeFinal;

        if (direcaoParaFrente != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direcaoParaFrente);
        }
    }

    private void OnCollisionEnter(Collision colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            VidaDoJogador vida = colisao.gameObject.GetComponent<VidaDoJogador>();
            if (vida != null)
            {
                vida.SofrerDano(danoKamikaze);
            }
        }
        Destroy(gameObject);
    }
}