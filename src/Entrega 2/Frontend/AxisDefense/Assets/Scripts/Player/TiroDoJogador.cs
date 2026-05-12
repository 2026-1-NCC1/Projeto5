using UnityEngine;
using UnityEngine.EventSystems;

public class TiroDoJogador : MonoBehaviour
{
    [Header("Configurações de Tiro")]
    public GameObject modeloProjetil;

    [Tooltip("Tiros por segundo")]
    public float cadenciaDeTiro = 5f;

    [Header("Referências")]
    public ControladorDeMira controladorDeMira;

    public Transform pontoDeDisparo;

    [Header("Som")]
    public AudioClip somTiro;

    [Range(0f, 1f)]
    public float volumeTiro = 0.8f;

    private AudioSource fonteAudio;

    private float contadorDeTiro = 0f;

    private void Awake()
    {
        // Busca o componente de áudio
        fonteAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        contadorDeTiro += Time.deltaTime;

        bool apertouMouse = Input.GetButtonDown("Fire1");
        bool apertouSpace = Input.GetKeyDown(KeyCode.Space);

        // Impede tiro ao clicar em anúncios da UI
        if (apertouMouse)
        {
            if (AdManager.AnunciosAtivos > 0 &&
                EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
        }

        // Sai caso nenhuma tecla de tiro tenha sido pressionada
        if (!(apertouMouse || apertouSpace))
            return;

        // Evita divisão por zero
        if (cadenciaDeTiro <= 0f)
            cadenciaDeTiro = 0.01f;

        // Controle de cadência
        if (contadorDeTiro >= 1f / cadenciaDeTiro)
        {
            contadorDeTiro = 0f;

            Atirar();
        }
    }

    private void Atirar()
    {
        if (GerenciadorDeCamera.Instancia == null) return;

        if (modeloProjetil == null) return;

        Camera camera =
            GerenciadorDeCamera.Instancia.CameraAtiva;

        if (camera == null) return;

        // Define origem do disparo
        Vector3 origem =
            pontoDeDisparo != null
            ? pontoDeDisparo.position
            : camera.transform.position;

        Vector3 direcao = camera.transform.forward;

        // Usa posição da mira para calcular direção do tiro
        if (controladorDeMira != null &&
            controladorDeMira.retanguloMira != null)
        {
            Vector2 posMiraTela =
                RectTransformUtility.WorldToScreenPoint(
                    null,
                    controladorDeMira.retanguloMira.position
                );

            Ray raio =
                camera.ScreenPointToRay(posMiraTela);

            if (Physics.Raycast(raio, out RaycastHit hit, 1000f))
                direcao = (hit.point - origem).normalized;
            else
                direcao = raio.direction.normalized;
        }

        // Cria projétil
        Instantiate(
            modeloProjetil,
            origem,
            Quaternion.LookRotation(direcao)
        );

        // Toca som do disparo
        if (somTiro != null && fonteAudio != null)
            fonteAudio.PlayOneShot(somTiro, volumeTiro);
    }
}