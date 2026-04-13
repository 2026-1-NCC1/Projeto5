using UnityEngine;
using TMPro;
using System.Collections;

public class GerenciadorDeCamera : MonoBehaviour
{
    public static GerenciadorDeCamera Instancia { get; private set; }

    [Header("Referências das Câmeras")]
    public Camera cameraFrente;
    public Camera cameraTras;
    public Camera cameraEsquerda;
    public Camera cameraDireita;

    [Header("Interface (UI)")]
    public TextMeshProUGUI textoNomeCamera;

    [Header("Efeito de Transição")]
    public GameObject painelSemSinal;
    public float duracaoSemSinal = 0.5f;

    public Camera CameraAtiva { get; private set; }
    private bool estaTrocando = false;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instancia = this;
        }
    }

    private void Start()
    {
        TrocarParaCamera(cameraFrente);
    }

    private void Update()
    {
        if (!estaTrocando)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) StartCoroutine(TrocarComEfeito(cameraFrente));
            if (Input.GetKeyDown(KeyCode.DownArrow)) StartCoroutine(TrocarComEfeito(cameraTras));
            if (Input.GetKeyDown(KeyCode.LeftArrow)) StartCoroutine(TrocarComEfeito(cameraEsquerda));
            if (Input.GetKeyDown(KeyCode.RightArrow)) StartCoroutine(TrocarComEfeito(cameraDireita));
        }
    }

    private void TrocarParaCamera(Camera novaCamera)
    {
        cameraFrente.gameObject.SetActive(false);
        cameraTras.gameObject.SetActive(false);
        cameraEsquerda.gameObject.SetActive(false);
        cameraDireita.gameObject.SetActive(false);

        novaCamera.gameObject.SetActive(true);
        CameraAtiva = novaCamera;

        if (textoNomeCamera != null)
        {
            if (novaCamera == cameraFrente) textoNomeCamera.text = "CAM 01 - FRENTE";
            if (novaCamera == cameraTras) textoNomeCamera.text = "CAM 02 - TRÁS";
            if (novaCamera == cameraEsquerda) textoNomeCamera.text = "CAM 03 - ESQUERDA";
            if (novaCamera == cameraDireita) textoNomeCamera.text = "CAM 04 - DIREITA";
        }
    }

    private IEnumerator TrocarComEfeito(Camera novaCamera)
    {
        estaTrocando = true;
        if (painelSemSinal != null) painelSemSinal.SetActive(true);
        yield return new WaitForSeconds(duracaoSemSinal);
        TrocarParaCamera(novaCamera);
        if (painelSemSinal != null) painelSemSinal.SetActive(false);
        estaTrocando = false;
    }
}