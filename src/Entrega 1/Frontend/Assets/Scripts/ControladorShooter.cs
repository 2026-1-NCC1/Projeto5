using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class ControladorShooter : MonoBehaviour
{
    [Header("Referências")]
    public Transform visual;
    public Transform pontoDeDisparo;
    public GameObject modeloProjetil;

    [Header("Movimento")]
    public float velocidadeMaxima = 10f;
    public float aceleracao = 8f;
    public float velocidadeRotacao = 4f;

    public float distanciaIdeal = 18f;
    public float toleranciaDistancia = 3f;
    public float velocidadeOrbita = 8f;
    public float tempoMinTrocaOrbita = 2f;
    public float tempoMaxTrocaOrbita = 5f;

    [Header("Oscilação Vertical")]
    public float amplitudeVertical = 2f;
    public float frequenciaVertical = 1.2f;

    [Header("Inclinação Visual (estilo caça)")]
    public float inclinacaoMaxima = 35f;
    public float suavidadeInclinacao = 8f;

    [Header("Tiro")]
    public float intervaloDeTiro = 1.2f;
    public int danoContato = 5;

    [Header("Som")]
    public AudioClip somTiro;
    [Range(0f, 1f)] public float volumeTiro = 0.7f;
    public float pitchMin = 0.95f;
    public float pitchMax = 1.05f;

    private Rigidbody rb;
    private Transform alvo;
    private Quaternion rotacaoVisualInicial;

    private float timerTiro;
    private float timerTrocaOrbita;
    private float direcaoOrbita = 1f;
    private float faseVertical;

    private AudioSource fonteAudio;

    private void OnEnable()
    {
        if (Radar.Instancia != null)
            Radar.Instancia.RegistrarInimigo(transform);
    }

    private void OnDisable()
    {
        if (Radar.Instancia != null)
            Radar.Instancia.DesregistrarInimigo(transform);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        fonteAudio = GetComponent<AudioSource>();
        fonteAudio.playOnAwake = false;

        faseVertical = Random.Range(0f, Mathf.PI * 2f);
        timerTrocaOrbita = Random.Range(tempoMinTrocaOrbita, tempoMaxTrocaOrbita);

        if (visual != null)
            rotacaoVisualInicial = visual.localRotation;

        if (VidaDoJogador.Instancia != null)
        {
            alvo = VidaDoJogador.Instancia.transform;
        }
        else
        {
            Debug.LogError("Shooter não encontrou VidaDoJogador na cena.", gameObject);
            enabled = false;
        }
    }

    private void Update()
    {
        if (alvo == null) return;

        timerTiro += Time.deltaTime;
        if (timerTiro >= intervaloDeTiro)
        {
            timerTiro = 0f;
            Atirar();
        }

        timerTrocaOrbita -= Time.deltaTime;
        if (timerTrocaOrbita <= 0f)
        {
            direcaoOrbita = Random.value > 0.5f ? 1f : -1f;
            timerTrocaOrbita = Random.Range(tempoMinTrocaOrbita, tempoMaxTrocaOrbita);
        }
    }

    private void FixedUpdate()
    {
        if (alvo == null) return;

        Vector3 paraAlvo = alvo.position - transform.position;
        float distancia = paraAlvo.magnitude;
        if (distancia < 0.001f) return;

        Vector3 dirFrente = paraAlvo / distancia;

        Vector3 dirDireitaOrbita = Vector3.Cross(Vector3.up, dirFrente).normalized;
        Vector3 componenteOrbita = dirDireitaOrbita * (direcaoOrbita * velocidadeOrbita);

        float erro = distancia - distanciaIdeal;

        float componenteAproxima = 0f;
        if (erro > toleranciaDistancia) componenteAproxima = 1f;
        else if (erro < -toleranciaDistancia) componenteAproxima = -1f;

        Vector3 componenteDistancia = dirFrente * (componenteAproxima * (velocidadeMaxima * 0.65f));

        faseVertical += Time.fixedDeltaTime * frequenciaVertical;
        Vector3 componenteVertical = Vector3.up * (Mathf.Sin(faseVertical) * amplitudeVertical);

        Vector3 velocidadeAlvo = componenteOrbita + componenteDistancia + componenteVertical;
        velocidadeAlvo = Vector3.ClampMagnitude(velocidadeAlvo, velocidadeMaxima);

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocidadeAlvo, aceleracao * Time.fixedDeltaTime);

        Quaternion rotAlvo = Quaternion.LookRotation(dirFrente, Vector3.up);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, rotAlvo, velocidadeRotacao * Time.fixedDeltaTime));

        AplicarInclinacaoVisual();
    }

    private void AplicarInclinacaoVisual()
    {
        if (visual == null) return;

        float velLateral = Vector3.Dot(rb.linearVelocity, transform.right);
        float fator = 0f;

        if (Mathf.Abs(velocidadeOrbita) > 0.01f)
            fator = Mathf.Clamp(velLateral / velocidadeOrbita, -1f, 1f);

        float roll = -fator * inclinacaoMaxima;

        Quaternion rotAlvo = rotacaoVisualInicial * Quaternion.Euler(0f, 0f, roll);
        visual.localRotation = Quaternion.Slerp(
            visual.localRotation,
            rotAlvo,
            suavidadeInclinacao * Time.deltaTime
        );
    }

    private void Atirar()
    {
        if (modeloProjetil == null || alvo == null) return;

        Transform origem = pontoDeDisparo != null ? pontoDeDisparo : transform;
        Vector3 direcao = (alvo.position - origem.position).normalized;

        GameObject proj = Instantiate(modeloProjetil, origem.position, Quaternion.LookRotation(direcao));

        ProjetilInimigo p = proj.GetComponent<ProjetilInimigo>();
        if (p != null)
            p.Inicializar(direcao);

        if (somTiro != null && fonteAudio != null)
        {
            fonteAudio.pitch = Random.Range(pitchMin, pitchMax);
            fonteAudio.PlayOneShot(somTiro, volumeTiro);
        }
    }

    private void OnCollisionEnter(Collision colisao)
    {
        if (colisao.gameObject.CompareTag("Player"))
        {
            VidaDoJogador vida = colisao.gameObject.GetComponent<VidaDoJogador>();
            if (vida != null) vida.SofrerDano(danoContato);
            Destroy(gameObject);
        }
    }
}