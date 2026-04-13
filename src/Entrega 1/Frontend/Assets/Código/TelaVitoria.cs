using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaVitoria : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("O nome EXATO da sua cena principal de jogo.")]
    public string nomeCenaJogo = "SampleScene";

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void JogarNovamente()
    {
        SceneManager.LoadScene(nomeCenaJogo);
    }

    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Saindo do jogo...");
    }
}