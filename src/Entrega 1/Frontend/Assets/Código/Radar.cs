using UnityEngine;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    public static Radar Instancia { get; private set; }

    [Header("Configurações do Radar")]
    [Tooltip("A distância máxima do mundo que o radar consegue 'ver'")]
    public float distanciaRadar = 50f;
    [Tooltip("O raio da sua imagem de radar na UI (metade da largura/altura)")]
    public float raioRadarUI = 100f;

    [Header("Referências")]
    [Tooltip("O prefab do ponto vermelho que representa o inimigo no radar.")]
    public GameObject modeloPontoInimigo;
    [Tooltip("O transform do objeto que é o centro do radar (o jogador).")]
    public Transform alvoDoJogador;

    private Dictionary<Transform, GameObject> inimigosRastreados = new Dictionary<Transform, GameObject>();

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

    public void RegistrarInimigo(Transform inimigo)
    {
        if (inimigo != null && !inimigosRastreados.ContainsKey(inimigo))
        {
            GameObject ponto = Instantiate(modeloPontoInimigo, transform);
            inimigosRastreados[inimigo] = ponto;
        }
    }

    public void DesregistrarInimigo(Transform inimigo)
    {
        if (inimigo != null && inimigosRastreados.ContainsKey(inimigo))
        {
            Destroy(inimigosRastreados[inimigo]);
            inimigosRastreados.Remove(inimigo);
        }
    }

    private void LateUpdate()
    {
        if (alvoDoJogador == null) return;

        List<Transform> inimigosDestruidos = new List<Transform>();

        foreach (var par in inimigosRastreados)
        {
            Transform inimigo = par.Key;

            if (inimigo == null)
            {
                Destroy(par.Value);
                inimigosDestruidos.Add(inimigo);
                continue;
            }

            RectTransform pontoRect = par.Value.GetComponent<RectTransform>();
            Vector3 deslocamentoInimigo = inimigo.position - alvoDoJogador.position;
            Vector2 posicaoPonto = new Vector2(deslocamentoInimigo.x, deslocamentoInimigo.z);

            posicaoPonto = Vector2.ClampMagnitude(posicaoPonto, distanciaRadar);
            posicaoPonto = (posicaoPonto / distanciaRadar) * raioRadarUI;

            pontoRect.anchoredPosition = posicaoPonto;
        }

        foreach (var inimigo in inimigosDestruidos)
        {
            inimigosRastreados.Remove(inimigo);
        }
    }
}