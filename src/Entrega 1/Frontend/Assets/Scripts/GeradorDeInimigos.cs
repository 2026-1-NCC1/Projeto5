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
    [Tooltip("Chance de spawnar um Shooter (0 a 1). Ex: 0.3 = 30%")]
    [Range(0f, 1f)]
    public float chanceShooter = 0.3f;

    [Header("Pontos de Geração")]
    public Transform[] pontosDeGeracao;

    private int indicePontoAtual = 0;

    private void Start()
    {
        if (modeloKamikaze == null || pontosDeGeracao.Length == 0)
        {
            Debug.LogError("ERRO: Configure o Modelo Kamikaze e os Pontos de Geração!");
            this.enabled = false;
            return;
        }

        StartCoroutine(GerarEmCiclo());
    }

    private IEnumerator GerarEmCiclo()
    {
        while (true)
        {
            Transform pontoAtual = pontosDeGeracao[indicePontoAtual];
            Vector3 posicaoGeracao = pontoAtual.position + pontoAtual.forward * distanciaGeracao;

            bool gerarShooter = modeloShooter != null && Random.value < chanceShooter;
            GameObject modeloEscolhido = gerarShooter ? modeloShooter : modeloKamikaze;

            Instantiate(modeloEscolhido, posicaoGeracao, Quaternion.identity);

            indicePontoAtual++;
            if (indicePontoAtual >= pontosDeGeracao.Length)
                indicePontoAtual = 0;

            yield return new WaitForSeconds(intervaloGeracao);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pontosDeGeracao == null) return;

        for (int i = 0; i < pontosDeGeracao.Length; i++)
        {
            Transform ponto = pontosDeGeracao[i];
            if (ponto != null)
            {
                Gizmos.color = Color.red;
                Vector3 posicaoGizmo = ponto.position + ponto.forward * distanciaGeracao;
                Gizmos.DrawWireSphere(posicaoGizmo, 1f);
                Gizmos.DrawLine(ponto.position, posicaoGizmo);

                #if UNITY_EDITOR
                UnityEditor.Handles.Label(ponto.position, "Ponto " + i);
                #endif
            }
        }
    }
}