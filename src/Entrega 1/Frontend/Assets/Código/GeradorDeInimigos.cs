using UnityEngine;
using System.Collections;

public class GeradorDeInimigos : MonoBehaviour
{
    [Header("Configurações de Geração")]
    public GameObject modeloInimigo;
    [Tooltip("Tempo em segundos entre a geração em CADA direção. Menor = mais intenso.")]
    public float intervaloGeracao = 1.5f;
    [Tooltip("Distância do ponto de origem em que o inimigo aparece.")]
    public float distanciaGeracao = 25f;

    [Header("Pontos de Geração")]
    [Tooltip("Arraste os 4 pontos de origem aqui (ex: os objetos SpawnPoint).")]
    public Transform[] pontosDeGeracao;

    private int indicePontoAtual = 0;

    private void Start()
    {
        if (modeloInimigo == null || pontosDeGeracao.Length == 0)
        {
            Debug.LogError("ERRO: Configure o Modelo do Inimigo e os Pontos de Geração!");
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
            Instantiate(modeloInimigo, posicaoGeracao, Quaternion.identity);

            indicePontoAtual++;
            if (indicePontoAtual >= pontosDeGeracao.Length)
            {
                indicePontoAtual = 0;
            }

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
                Gizmos.color = Color.cyan;
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(ponto.position, "Ponto " + i);
                #endif

                Gizmos.color = Color.red;
                Vector3 posicaoGizmo = ponto.position + ponto.forward * distanciaGeracao;
                Gizmos.DrawWireSphere(posicaoGizmo, 1f);
                Gizmos.DrawLine(ponto.position, posicaoGizmo);
            }
        }
    }
}