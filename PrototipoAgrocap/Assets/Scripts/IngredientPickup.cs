using UnityEngine;

public class IngredientPickup : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private IngredienteTipo ingrediente;

    private Inventory inventory;
    private ItemSpawner spawner;

    public void Configurar(ItemSpawner novoSpawner, Inventory novoInventory)
    {
        spawner = novoSpawner;
        inventory = novoInventory;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Debug.Log($"Coletado: {ingrediente}");

        if (inventory != null)
        {
            inventory.AdicionarIngrediente(ingrediente);
        }
        else
        {
            Debug.LogError("Inventory não foi configurado!");
        }

        if (spawner != null)
        {
            spawner.ItemFoiColetado();
        }

        Destroy(gameObject);
    }
}