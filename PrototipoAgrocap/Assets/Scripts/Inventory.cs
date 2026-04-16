using TMPro;
using UnityEngine;

// Gerencia o inventário de ingredientes do jogador
// Armazena os itens coletados e atualiza a interface
public class Inventory : MonoBehaviour
{
    [Header("Ingredientes")]

    // Quantidade de cada ingrediente
    public int trigo = 0;
    public int ovo = 0;
    public int leite = 0;
    public int chocolate = 0;
    public int morango = 0;

    [Header("UI")]

    // Texto que mostra apenas o último item coletado
    [SerializeField] private TextMeshProUGUI textoUltimoItem;

    // Adiciona um ingrediente ao inventário
    public void AdicionarIngrediente(IngredienteTipo tipo)
    {
        // Verifica qual ingrediente foi coletado
        switch (tipo)
        {
            case IngredienteTipo.Trigo:
                trigo++;
                break;

            case IngredienteTipo.Ovo:
                ovo++;
                break;

            case IngredienteTipo.Leite:
                leite++;
                break;

            case IngredienteTipo.Chocolate:
                chocolate++;
                break;

            case IngredienteTipo.Morango:
                morango++;
                break;
        }

        // Exibe no console (debug)
        Debug.Log($"Coletado: {tipo}");

        // Atualiza a UI do último item coletado
        AtualizarUltimoItem(tipo);
    }

    // Atualiza o texto exibindo apenas o último item coletado
    private void AtualizarUltimoItem(IngredienteTipo tipo)
    {
        if (textoUltimoItem != null)
        {
            textoUltimoItem.text = "Último item: " + tipo;
        }
        else
        {
            Debug.LogWarning("Texto do último item não está conectado!");
        }
    }
}