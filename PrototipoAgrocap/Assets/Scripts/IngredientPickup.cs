using UnityEngine;

// Representa o item coletavel em cena e encaminha a coleta para os sistemas centrais.
public class IngredientPickup : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private IngredienteTipo ingrediente;

    private Inventory inventory;
    private ItemSpawner spawner;

    public void Configurar(ItemSpawner novoSpawner, Inventory novoInventory)
    {
        // O spawner injeta as referencias logo apos instanciar o prefab.
        spawner = novoSpawner;
        inventory = novoInventory;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Somente a bola ativa a coleta do ingrediente.
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
            // Libera o respawn do proximo item depois que este foi consumido.
            spawner.ItemFoiColetado(ingrediente);
        }

        Destroy(gameObject);
    }
}
