using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaFimDeJogo : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do jogo...");
    }
}