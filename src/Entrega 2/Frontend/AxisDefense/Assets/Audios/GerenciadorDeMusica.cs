using UnityEngine;
using System.Collections;

public class GerenciadorDeMusica : MonoBehaviour
{
    public static GerenciadorDeMusica Instancia { get; private set; }

    [Header("Clips")]
    public AudioClip musicaJogo;
    public AudioClip musicaBoss;

    [Header("Volumes")]
    [Range(0f, 1f)] public float volumeJogo = 1f;
    [Range(0f, 1f)] public float volumeBoss = 1f;

    [Header("Transição")]
    public float duracaoFade = 1.0f;

    [Tooltip("Se true, NÃO sobrepõe: primeiro some uma música, depois entra a outra.")]
    public bool semSobreposicao = true;

    private AudioSource srcJogo;
    private AudioSource srcBoss;
    private Coroutine rotina;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;

        srcJogo = gameObject.AddComponent<AudioSource>();
        srcBoss = gameObject.AddComponent<AudioSource>();

        ConfigurarSource(srcJogo, musicaJogo, volumeJogo);
        ConfigurarSource(srcBoss, musicaBoss, 0f); // começa mudo
    }

    private void Start()
    {
        if (srcJogo.clip != null)
            srcJogo.Play();
    }

    private void ConfigurarSource(AudioSource s, AudioClip clip, float volumeInicial)
    {
        s.playOnAwake = false;
        s.loop = true;
        s.spatialBlend = 0f; // 2D
        s.clip = clip;
        s.volume = volumeInicial;
    }

    public void BossEntrou()
    {
        if (srcBoss.clip == null) return;

        if (rotina != null) StopCoroutine(rotina);
        rotina = StartCoroutine(semSobreposicao
            ? TrocarSemSobrepor(srcJogo, srcBoss, volumeBoss)
            : TrocarComCrossfade(srcJogo, srcBoss, volumeJogo, volumeBoss));
    }

    public void BossSaiu()
    {
        if (srcJogo.clip == null) return;

        if (rotina != null) StopCoroutine(rotina);
        rotina = StartCoroutine(semSobreposicao
            ? TrocarSemSobrepor(srcBoss, srcJogo, volumeJogo)
            : TrocarComCrossfade(srcBoss, srcJogo, volumeBoss, volumeJogo));
    }

    // Não sobrepõe: fade out -> stop -> play outro -> fade in
    private IEnumerator TrocarSemSobrepor(AudioSource de, AudioSource para, float volumeFinalPara)
    {
        if (duracaoFade <= 0f)
        {
            de.Stop();
            de.volume = 0f;

            if (!para.isPlaying) para.Play();
            para.volume = volumeFinalPara;
            yield break;
        }

        // Fade OUT do atual
        float volInicialDe = de.volume;
        float t = 0f;
        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duracaoFade);
            de.volume = Mathf.Lerp(volInicialDe, 0f, k);
            yield return null;
        }
        de.volume = 0f;
        de.Stop();

        // Fade IN do próximo
        if (!para.isPlaying) para.Play();
        para.volume = 0f;

        t = 0f;
        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duracaoFade);
            para.volume = Mathf.Lerp(0f, volumeFinalPara, k);
            yield return null;
        }
        para.volume = volumeFinalPara;
    }

    // Crossfade (sobrepõe durante a transição)
    private IEnumerator TrocarComCrossfade(AudioSource de, AudioSource para, float volDeFinal, float volParaFinal)
    {
        if (!para.isPlaying) para.Play();

        float volDeInicial = de.volume;
        float volParaInicial = para.volume;

        float t = 0f;
        while (t < duracaoFade)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / duracaoFade);

            de.volume = Mathf.Lerp(volDeInicial, 0f, k);
            para.volume = Mathf.Lerp(volParaInicial, volParaFinal, k);

            yield return null;
        }

        de.volume = 0f;
        de.Stop();
        para.volume = volParaFinal;
    }
}