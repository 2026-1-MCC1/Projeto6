using TMPro;
using UnityEngine;

// Armazena e controla os ingredientes coletados
public class Inventory : MonoBehaviour
{
    [Header("Ingredientes")]
    public int trigo = 0;
    public int ovo = 0;
    public int leite = 0;
    public int chocolate = 0;
    public int morango = 0;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI textoInventario;
    [SerializeField] private TextMeshProUGUI textoUltimoItem;

    void Start()
    {
        AtualizarUI();
    }

    // Adiciona ingrediente ao inventário
    public void AdicionarIngrediente(IngredienteTipo tipo)
    {
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

        // mostra apenas o último item coletado
        AtualizarUltimoItem(tipo);

        Debug.Log($"Você pegou: {tipo}");
        AtualizarUI();
    }

    // Atualiza o texto exibindo apenas o último item coletado
    private void AtualizarUltimoItem(IngredienteTipo tipo)
    {
        if (textoUltimoItem != null)
        {
            textoUltimoItem.text = "Último item: " + tipo;
        }
    }

    //Atualiza a UI do inventário
    private void AtualizarUI()
    {
        if (textoInventario != null)
        {
            textoInventario.text =
             $"Trigo: {trigo}\n" +
            $"Ovo: {ovo}\n" +
            $"Leite: {leite}\n" +
            $"Chocolate: {chocolate}\n" +
            $"Morango: {morango}";
        }
    }
}
