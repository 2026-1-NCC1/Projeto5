using UnityEngine;
using TMPro;

public class PiscaTexto : MonoBehaviour
{
    [Header("Configurações de Piscada")]
    [Tooltip("A duração de cada estado (aceso/apagado). 0.5 significa que fica 0.5s aceso e 0.5s apagado.")]
    public float velocidadeDePiscada = 0.5f;

    private TextMeshProUGUI texto;

    private void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
        if (texto == null)
        {
            Debug.LogError("O script PiscaTexto precisa estar no mesmo objeto que um componente TextMeshProUGUI.", gameObject);
            this.enabled = false;
        }
    }

    private void Update()
    {
        // A matemática do módulo cria um ciclo repetitivo
        float cicloCompleto = velocidadeDePiscada * 2;
        bool estaVisivel = (Time.time % cicloCompleto) < velocidadeDePiscada;

        texto.enabled = estaVisivel;
    }
}