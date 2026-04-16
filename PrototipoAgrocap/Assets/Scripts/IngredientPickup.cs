using TMPro;
using UnityEngine;

// Detecta quando a bola coleta um ingrediente
public class IngredientPickup : MonoBehaviour
{
    [Header("Configuração")]
    // Tipo do ingrediente (definido no Inspector)
    [SerializeField] private IngredienteTipo ingrediente;

    [Header("Referência")]
    // Referência ao Inventario para adicionar o ingrediente coletado
    [SerializeField] private Inventory inventory;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é a bola
        if (!other.CompareTag("Ball")) return;

        Debug.Log($"Você pegou: {ingrediente}");

        // Envia para o ScoreManager
        inventory.AdicionarIngrediente(ingrediente);

        // Destroi o objeto após coleta
        Destroy(gameObject);
    }
}