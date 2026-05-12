using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class SemSinalVideoPlayer : MonoBehaviour
{
    private VideoPlayer vp;

    private void Awake()
    {
        // Pega o componente de vídeo
        vp = GetComponent<VideoPlayer>();

        // Configurações iniciais do player
        vp.playOnAwake = false;
        vp.isLooping = true;
        vp.waitForFirstFrame = true;
    }

    private void OnEnable()
    {
        // Reinicia e prepara o vídeo ao ativar o objeto
        vp.Stop();
        vp.Prepare();

        vp.prepareCompleted += Tocar;
    }

    private void OnDisable()
    {
        // Remove o evento e interrompe o vídeo
        vp.prepareCompleted -= Tocar;

        vp.Stop();
    }

    private void Tocar(VideoPlayer player)
    {
        // Remove o evento após preparar
        player.prepareCompleted -= Tocar;

        // Inicia reprodução do vídeo
        player.Play();
    }
}