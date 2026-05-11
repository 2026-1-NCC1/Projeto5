using UnityEngine;
using UnityEngine.SceneManagement;

public class TelaMenuPrincipal : MonoBehaviour
{
    [Header("Nomes das cenas")]
    [Tooltip("Cena do jogo (ex.: SampleScene).")]
    public string cenaJogo = "SampleScene";

    [Tooltip("Cena de instruções (ex.: Instrucoes).")]
    public string cenaInstrucoes = "Instrucoes";

    private void Start()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Ligue no botão "JOGAR"
    public void Jogar()
    {
        SceneManager.LoadScene(cenaJogo, LoadSceneMode.Single);
    }

    public void Voltar()
    {
        SceneManager.LoadScene(2);
    }

    // Ligue no botão "INSTRUÇÕES"
    public void Instrucoes()
    {
        SceneManager.LoadScene(4);
    }

    

    // Ligue no botão "SAIR"
    public void Sair()
    {
        Application.Quit();
        Debug.Log("Sair() chamado (no Editor não fecha).");
    }
}