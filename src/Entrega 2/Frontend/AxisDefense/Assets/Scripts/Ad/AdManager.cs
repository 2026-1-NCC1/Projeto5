using UnityEngine;
using System.Collections;

public class AdManager : MonoBehaviour
{
    // Quantidade de anúncios ativos na tela
    public static int AnunciosAtivos { get; private set; } = 0;

    [Header("Onde os anúncios aparecem (arraste AdLayer)")]
    public RectTransform adLayer;

    [Header("Prefabs")]
    public GameObject prefabClick;
    public GameObject prefabDigitar;

    [Header("Spawn")]
    public float intervaloMin = 4f;
    public float intervaloMax = 10f;
    public int maxAnunciosNaTela = 6;

    [Range(0f, 1f)] public float chanceDigitar = 0.35f;

    private void Start()
    {
        // Inicia o loop de criação dos anúncios
        StartCoroutine(LoopSpawn());

        // Ajusta o cursor no início
        AtualizarCursor();
    }

    private IEnumerator LoopSpawn()
    {
        while (true)
        {
            // Espera um tempo aleatório antes de criar outro anúncio
            yield return new WaitForSeconds(Random.Range(intervaloMin, intervaloMax));

            if (adLayer == null) continue;

            // Evita ultrapassar o limite de anúncios
            if (AnunciosAtivos >= maxAnunciosNaTela) continue;

            SpawnUm();
        }
    }

    private void SpawnUm()
    {
        // Decide se vai spawnar anúncio de digitar ou de clique
        bool spawnDigitar = prefabDigitar != null && UnityEngine.Random.value < chanceDigitar;

        GameObject prefab = spawnDigitar ? prefabDigitar : prefabClick;

        if (prefab == null) return;

        GameObject go = Instantiate(prefab, adLayer);

        AnunciosAtivos++;
        AtualizarCursor();

        // Posiciona o popup em uma área aleatória da tela
        RectTransform rt = go.GetComponent<RectTransform>();

        if (rt != null)
            rt.anchoredPosition = PegarPosicaoAleatoria(adLayer.rect, rt);

        // Atualiza a contagem ao fechar popup de clique
        AdPopup click = go.GetComponent<AdPopup>();

        if (click != null)
            click.AoFechar += _ =>
            {
                AnunciosAtivos--;
                AtualizarCursor();
            };

        // Atualiza a contagem ao fechar popup de digitação
        AdPopupDigitar dig = go.GetComponent<AdPopupDigitar>();

        if (dig != null)
            dig.AoFechar += _ =>
            {
                AnunciosAtivos--;
                AtualizarCursor();
            };
    }

    private Vector2 PegarPosicaoAleatoria(Rect area, RectTransform popup)
    {
        float halfW = popup.rect.width * 0.5f;
        float halfH = popup.rect.height * 0.5f;

        float minX = area.xMin + halfW;
        float maxX = area.xMax - halfW;
        float minY = area.yMin + halfH;
        float maxY = area.yMax - halfH;

        // Caso a área seja menor que o popup
        if (maxX <= minX || maxY <= minY)
            return Vector2.zero;

        float x = UnityEngine.Random.Range(minX, maxX);
        float y = UnityEngine.Random.Range(minY, maxY);

        return new Vector2(x, y);
    }

    private void AtualizarCursor()
    {
        // Libera o cursor enquanto houver anúncios ativos
        if (AnunciosAtivos > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            // Retorna ao modo normal do jogo
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}