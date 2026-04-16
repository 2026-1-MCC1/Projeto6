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

        Debug.Log($"Inventário → Trigo:{trigo} Ovo:{ovo} Leite:{leite} Chocolate:{chocolate}");

        AtualizarUI();
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
