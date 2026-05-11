using UnityEngine;
using TMPro;

public class PiscaTexto : MonoBehaviour
{
    [Header("Configurações de Piscada")]

    [Tooltip("A duração de cada estado (aceso/apagado).")]
    public float velocidadeDePiscada = 0.5f;

    private TextMeshProUGUI texto;

    private void Start()
    {
        // Pega o componente de texto do objeto
        texto = GetComponent<TextMeshProUGUI>();

        // Desativa o script caso não encontre o TMP
        if (texto == null)
        {
            Debug.LogError("O script PiscaTexto precisa estar no mesmo objeto que um componente TextMeshProUGUI.", gameObject);

            this.enabled = false;
        }
    }

    private void Update()
    {
        // Tempo total de um ciclo completo
        float cicloCompleto = velocidadeDePiscada * 2;

        // Alterna entre visível e invisível
        bool estaVisivel = (Time.time % cicloCompleto) < velocidadeDePiscada;

        texto.enabled = estaVisivel;
    }
}