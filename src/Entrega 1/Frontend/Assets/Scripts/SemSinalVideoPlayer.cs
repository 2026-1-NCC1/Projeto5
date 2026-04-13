using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class SemSinalVideoPlayer : MonoBehaviour
{
    private VideoPlayer vp;

    private void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        vp.playOnAwake = false;
        vp.isLooping = true;
        vp.waitForFirstFrame = true;
    }

    private void OnEnable()
    {
        vp.Stop();
        vp.Prepare();
        vp.prepareCompleted += Tocar;
    }

    private void OnDisable()
    {
        vp.prepareCompleted -= Tocar;
        vp.Stop();
    }

    private void Tocar(VideoPlayer player)
    {
        player.prepareCompleted -= Tocar;
        player.Play();
    }
}