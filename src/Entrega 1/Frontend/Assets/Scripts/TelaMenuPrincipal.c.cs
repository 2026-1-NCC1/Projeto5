using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaMenuPrincipal : MonoBehaviour
{
    [Header("Cenas")]
    public string nomeCenaDoJogo = "SampleScene";

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Jogar()
    {
        SceneManager.LoadScene(nomeCenaDoJogo);
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Sair() chamado (no Editor não fecha).");
    }
}