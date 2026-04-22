using TMPro;
using UnityEngine;

public class IngredientPickup : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private IngredienteTipo ingrediente;

    [Header("Referência")]
    [SerializeField] private Inventory inventory;

    [Header("Respawn")]
    public ItemSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Debug.Log($"Coletado: {ingrediente}");

        inventory.AdicionarIngrediente(ingrediente);

        if (spawner != null)
        {
            spawner.ItemFoiColetado();
        }
        else
        {
            Debug.LogWarning($"Spawner não foi atribuído no objeto {gameObject.name}");
        }

        Destroy(gameObject);
    }
}