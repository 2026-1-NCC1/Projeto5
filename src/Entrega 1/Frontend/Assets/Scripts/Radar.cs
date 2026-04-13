using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    public static Radar Instancia { get; private set; }

    [Header("Alertas - 4 TMP DIFERENTES no Inspector")]
    public TextMeshProUGUI alertaFrente;    // ▲
    public TextMeshProUGUI alertaTras;      // ▼
    public TextMeshProUGUI alertaEsquerda;  // ◀
    public TextMeshProUGUI alertaDireita;   // ▶

    [Header("Centro do Mapa (arraste o objeto central da cena)")]
    public Transform centroDoMapa;

    [Header("Piscar")]
    public float piscadaBase = 1f;
    public float piscadaMaxima = 8f;
    public int inimigosParaMax = 6;
    [Range(0f, 0.5f)]
    public float alphaApagado = 0.05f;

    private readonly List<Transform> inimigos = new();

    private float tF, tT, tE, tD;
    private bool sF = true, sT = true, sE = true, sD = true;

    private void Awake()
    {
        if (Instancia != null && Instancia != this) { Destroy(gameObject); return; }
        Instancia = this;
    }

    private void Start()
    {
        SetAlpha(alertaFrente, 0f);
        SetAlpha(alertaTras, 0f);
        SetAlpha(alertaEsquerda, 0f);
        SetAlpha(alertaDireita, 0f);
    }

    public void RegistrarInimigo(Transform t)
    {
        if (t != null && !inimigos.Contains(t)) inimigos.Add(t);
    }

    public void DesregistrarInimigo(Transform t) => inimigos.Remove(t);

    private void Update()
    {
        inimigos.RemoveAll(i => i == null);

        Vector3 centro = centroDoMapa != null
            ? centroDoMapa.position
            : Vector3.zero;

        int cF = 0, cT = 0, cE = 0, cD = 0;

        foreach (var inimigo in inimigos)
        {
            if (inimigo == null) continue;

            float dx = inimigo.position.x - centro.x;
            float dz = inimigo.position.z - centro.z;

            if (Mathf.Abs(dz) >= Mathf.Abs(dx))
            {
                if (dz >= 0f) cF++;   // Z positivo = Frente
                else          cT++;   // Z negativo = Trás
            }
            else
            {
                if (dx >= 0f) cD++;   // X positivo = Direita
                else          cE++;   // X negativo = Esquerda
            }
        }

        Piscar(alertaFrente,   cF, ref tF, ref sF);
        Piscar(alertaTras,     cT, ref tT, ref sT);
        Piscar(alertaEsquerda, cE, ref tE, ref sE);
        Piscar(alertaDireita,  cD, ref tD, ref sD);
    }

    private void Piscar(TextMeshProUGUI tmp, int qtd,
                        ref float timer, ref bool estado)
    {
        if (tmp == null) return;

        if (qtd <= 0)
        {
            SetAlpha(tmp, 0f);
            timer  = 0f;
            estado = true;
            return;
        }

        float t   = Mathf.Clamp01((float)qtd / inimigosParaMax);
        Color cor = Color.Lerp(new Color(1f, 0.65f, 0f), Color.red, t);
        float vel = Mathf.Lerp(piscadaBase, piscadaMaxima, t);

        timer += Time.deltaTime * vel;
        if (timer >= 1f)
        {
            timer -= 1f;
            estado = !estado;
        }

        cor.a    = estado ? 1f : alphaApagado;
        tmp.color = cor;
    }

    private static void SetAlpha(TextMeshProUGUI tmp, float a)
    {
        if (tmp == null) return;
        Color c = tmp.color;
        c.a     = a;
        tmp.color = c;
    }
}