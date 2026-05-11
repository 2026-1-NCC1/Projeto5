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
        // Registra o inimigo no radar após ativar
        Invoke(nameof(RegistrarNoRadar), 0.01f);
    }

    private void OnDisable()
    {
        // Remove o inimigo do radar
        if (Radar.Instancia != null)
        {
            Radar.Instancia.DesregistrarInimigo(transform);
        }
    }

    private void RegistrarNoRadar()
    {
        // Adiciona o inimigo ao radar
        if (Radar.Instancia != null)
        {
            Radar.Instancia.RegistrarInimigo(transform);
        }
    }

    private void Start()
    {
        // Pega o Rigidbody do inimigo
        corpoRigido = GetComponent<Rigidbody>();

        // Valor aleatório para variar o movimento
        contadorZigueZague = Random.Range(0f, 2f * Mathf.PI);

        // Procura o jogador como alvo
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

        // Direção principal até o jogador
        Vector3 direcaoParaFrente =
            (alvoDoJogador.position - transform.position).normalized;

        Vector3 direcaoHorizontal = transform.right;
        Vector3 direcaoVertical = transform.up;

        // Atualiza o movimento em zigue-zague
        contadorZigueZague += Time.deltaTime * frequenciaZigueZague;

        float forcaHorizontal =
            Mathf.Sin(contadorZigueZague) * velocidadeZigueZagueHorizontal;

        float forcaVertical =
            Mathf.Cos(contadorZigueZague) * velocidadeZigueZagueVertical;

        // Combina movimento frontal e lateral
        Vector3 velocidadeFinal =
            (direcaoParaFrente * velocidadeMovimento) +
            (direcaoHorizontal * forcaHorizontal) +
            (direcaoVertical * forcaVertical);

        corpoRigido.linearVelocity = velocidadeFinal;

        // Faz o inimigo olhar para o jogador
        if (direcaoParaFrente != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direcaoParaFrente);
        }
    }

    private void OnCollisionEnter(Collision colisao)
    {
        // Aplica dano ao colidir com o jogador
        if (colisao.gameObject.CompareTag("Player"))
        {
            VidaDoJogador vida =
                colisao.gameObject.GetComponent<VidaDoJogador>();

            if (vida != null)
            {
                vida.SofrerDano(danoKamikaze);
            }
        }

        // Destroi o inimigo após colisão
        Destroy(gameObject);
    }
}