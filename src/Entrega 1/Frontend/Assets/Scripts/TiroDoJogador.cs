using UnityEngine;

public class TiroDoJogador : MonoBehaviour
{
    [Header("Configurações de Tiro")]
    public GameObject modeloProjetil;
    public float cadenciaDeTiro = 5f;

    [Header("Referências")]
    public ControladorDeMira controladorDeMira;
    public Transform pontoDeDisparo;

    [Header("Som")]
    public AudioClip somTiro;
    [Range(0f, 1f)] public float volumeTiro = 0.8f;

    private AudioSource fonteAudio;
    private float contadorDeTiro = 0f;

    private void Awake()
    {
        fonteAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        contadorDeTiro += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            if (contadorDeTiro >= 1f / cadenciaDeTiro)
            {
                contadorDeTiro = 0f;
                Atirar();
            }
        }
    }

    private void Atirar()
    {
        if (GerenciadorDeCamera.Instancia == null) return;
        if (modeloProjetil == null) return;

        Camera camera = GerenciadorDeCamera.Instancia.CameraAtiva;
        if (camera == null) return;

        Vector3 origem = pontoDeDisparo != null ? pontoDeDisparo.position : camera.transform.position;

        Vector3 direcao = camera.transform.forward;
        if (controladorDeMira != null && controladorDeMira.retanguloMira != null)
        {
            Vector2 posMiraTela = RectTransformUtility.WorldToScreenPoint(null, controladorDeMira.retanguloMira.position);
            Ray raio = camera.ScreenPointToRay(posMiraTela);

            if (Physics.Raycast(raio, out RaycastHit hit, 1000f))
                direcao = (hit.point - origem).normalized;
            else
                direcao = raio.direction.normalized;
        }

        Instantiate(modeloProjetil, origem, Quaternion.LookRotation(direcao));

        if (somTiro != null && fonteAudio != null)
            fonteAudio.PlayOneShot(somTiro, volumeTiro);
    }
}