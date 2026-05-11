using UnityEngine;
using TMPro;
using System.Collections;

public class GerenciadorDeCamera : MonoBehaviour
{
    // Instância global do gerenciador
    public static GerenciadorDeCamera Instancia { get; private set; }

    [Header("Câmeras")]
    public Camera cameraFrente;
    public Camera cameraTras;
    public Camera cameraEsquerda;
    public Camera cameraDireita;

    [Header("Interface (UI)")]
    public TextMeshProUGUI textoNomeCamera;

    [Header("Efeito de Transição")]
    public GameObject painelSemSinal;
    public float duracaoSemSinal = 0.5f;

    // Câmera atualmente ativa
    public Camera CameraAtiva { get; private set; }

    private bool estaTrocando = false;

    private void Awake()
    {
        // Garante apenas uma instância do gerenciador
        if (Instancia != null && Instancia != this)
            Destroy(gameObject);
        else
            Instancia = this;
    }

    private void Start()
    {
        // Verifica se todas as câmeras foram configuradas
        if (cameraFrente == null || cameraTras == null ||
            cameraEsquerda == null || cameraDireita == null)
        {
            Debug.LogError("ERRO: Uma ou mais câmeras não estão configuradas!", gameObject);
            return;
        }

        // Define a câmera inicial
        TrocarParaCamera(cameraFrente);
    }

    private void Update()
    {
    if (estaTrocando) return;

    // Frente
    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        StartCoroutine(TrocarComEfeito(cameraFrente));

    // Trás
    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        StartCoroutine(TrocarComEfeito(cameraTras));

    // Esquerda
    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        StartCoroutine(TrocarComEfeito(cameraEsquerda));

    // Direita
    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        StartCoroutine(TrocarComEfeito(cameraDireita));
    }

    private void TrocarParaCamera(Camera novaCamera)
    {
        // Desativa todas as câmeras
        cameraFrente.gameObject.SetActive(false);
        cameraTras.gameObject.SetActive(false);
        cameraEsquerda.gameObject.SetActive(false);
        cameraDireita.gameObject.SetActive(false);

        // Ativa apenas a câmera escolhida
        novaCamera.gameObject.SetActive(true);

        CameraAtiva = novaCamera;

        AtualizarTexto();
    }

    private void AtualizarTexto()
    {
        if (textoNomeCamera == null || CameraAtiva == null) return;

        // Atualiza o nome exibido na interface
        if (CameraAtiva == cameraFrente)
            textoNomeCamera.text = "CAM 01 - FRENTE";

        if (CameraAtiva == cameraTras)
            textoNomeCamera.text = "CAM 02 - TRÁS";

        if (CameraAtiva == cameraEsquerda)
            textoNomeCamera.text = "CAM 03 - ESQUERDA";

        if (CameraAtiva == cameraDireita)
            textoNomeCamera.text = "CAM 04 - DIREITA";
    }

    private IEnumerator TrocarComEfeito(Camera novaCamera)
    {
        estaTrocando = true;

        // Mostra o efeito de "sem sinal"
        if (painelSemSinal != null)
            painelSemSinal.SetActive(true);

        yield return new WaitForSeconds(duracaoSemSinal);

        TrocarParaCamera(novaCamera);

        // Remove o efeito após a troca
        if (painelSemSinal != null)
            painelSemSinal.SetActive(false);

        estaTrocando = false;
    }
}