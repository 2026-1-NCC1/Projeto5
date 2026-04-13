using UnityEngine;

public class TiroDoJogador : MonoBehaviour
{
    [Header("Configurações de Tiro")]
    [Tooltip("O prefab do projétil que será disparado.")]
    public GameObject modeloProjetil;
    [Tooltip("Quantos tiros podem ser disparados por segundo.")]
    public float cadenciaDeTiro = 5f;

    [Header("Referências")]
    [Tooltip("Arraste o objeto que tem o script ControladorDeMira aqui.")]
    public ControladorDeMira controladorDeMira;

    private float contadorDeTiro = 0f;

    private void Update()
    {
        contadorDeTiro += Time.deltaTime;

        if (Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space))
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
        if (GerenciadorDeCamera.Instancia == null || modeloProjetil == null) return;

        Camera cameraAtiva = GerenciadorDeCamera.Instancia.CameraAtiva;
        if (cameraAtiva == null) return;

        // Converte a posição da mira no Canvas para a posição na tela real
        Vector2 posicaoMiraNaTela = new Vector2(
            controladorDeMira.ObterPosicaoMira().x + Screen.width / 2f,
            controladorDeMira.ObterPosicaoMira().y + Screen.height / 2f
        );

        // Cria um raio que parte da câmera, passando pelo ponto da mira na tela
        Ray raio = cameraAtiva.ScreenPointToRay(posicaoMiraNaTela);
        RaycastHit pontoDeImpacto;

        Vector3 direcaoDisparo;

        if (Physics.Raycast(raio, out pontoDeImpacto, 1000f))
        {
            // Se o raio acertou algo no mundo, atira nessa direção
            direcaoDisparo = (pontoDeImpacto.point - cameraAtiva.transform.position).normalized;
        }
        else
        {
            // Se não acertou nada (ex: céu), atira para um ponto distante na direção do raio
            direcaoDisparo = raio.direction.normalized;
        }

        Instantiate(
            modeloProjetil,
            cameraAtiva.transform.position,
            Quaternion.LookRotation(direcaoDisparo)
        );
    }
}