using UnityEngine;
using System.Collections;

public class GeradorDeInimigos : MonoBehaviour
{
    [Header("Modelos de Inimigos")]
    public GameObject modeloKamikaze;
    public GameObject modeloShooter;

    [Header("Configurações de Geração")]

    [Tooltip("Tempo entre spawns")]
    public float intervaloGeracao = 1.5f;

    public float distanciaGeracao = 25f;

    [Header("Probabilidade")]

    [Range(0f, 1f)]
    public float chanceShooter = 0.3f;

    [Header("Pontos de Geração")]
    public Transform[] pontosDeGeracao;

    private int indicePontoAtual = 0;
    private bool pausado = false;

    public void SetPausado(bool v) => pausado = v;

    private void Start()
    {
        // Verifica se os dados necessários foram configurados
        if (modeloKamikaze == null ||
            pontosDeGeracao == null ||
            pontosDeGeracao.Length == 0)
        {
            Debug.LogError("ERRO: Configure o Modelo Kamikaze e os Pontos de Geração!");

            enabled = false;

            return;
        }

        StartCoroutine(GerarEmCiclo());
    }

    private IEnumerator GerarEmCiclo()
    {
        while (true)
        {
            // Pausa o spawn enquanto necessário
            while (pausado)
                yield return null;

            // Pega o ponto atual de geração
            Transform pontoAtual =
                pontosDeGeracao[indicePontoAtual];

            Vector3 posicaoGeracao =
                pontoAtual.position + pontoAtual.forward * distanciaGeracao;

            // Decide qual inimigo será gerado
            bool gerarShooter =
                modeloShooter != null && Random.value < chanceShooter;

            GameObject modeloEscolhido =
                gerarShooter ? modeloShooter : modeloKamikaze;

            // Cria o inimigo
            Instantiate(
                modeloEscolhido,
                posicaoGeracao,
                Quaternion.identity
            );

            // Avança para o próximo ponto
            indicePontoAtual++;

            if (indicePontoAtual >= pontosDeGeracao.Length)
                indicePontoAtual = 0;

            yield return new WaitForSeconds(intervaloGeracao);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pontosDeGeracao == null) return;

        // Desenha os pontos de spawn na cena
        for (int i = 0; i < pontosDeGeracao.Length; i++)
        {
            Transform ponto = pontosDeGeracao[i];

            if (ponto != null)
            {
                Gizmos.color = Color.red;

                Vector3 posicaoGizmo =
                    ponto.position + ponto.forward * distanciaGeracao;

                Gizmos.DrawWireSphere(posicaoGizmo, 1f);

                Gizmos.DrawLine(ponto.position, posicaoGizmo);

#if UNITY_EDITOR
                UnityEditor.Handles.Label(ponto.position, "Ponto " + i);
#endif
            }
        }
    }
}