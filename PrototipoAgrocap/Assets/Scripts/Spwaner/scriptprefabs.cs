using UnityEngine;

// Item coletável pela bola
public class ItemColetavel : MonoBehaviour
{
    [SerializeField] private IngredienteTipo ingrediente;

    private ItemSpawner spawner;

    public void DefinirSpawner(ItemSpawner s)
    {
        spawner = s;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Debug.Log("Item coletado: " + ingrediente);

        if (spawner != null)
        {
            spawner.ItemFoiColetado(ingrediente);
        }

        Destroy(gameObject);
    }
}