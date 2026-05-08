using UnityEngine;

// Item coletável pela bola
// Mantém compatibilidade com prefabs que ainda usam este componente simples.
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

        Debug.Log("Item ooletado: " + ingrediente);

        if (spawner != null)
        {
            spawner.ItemFoiColetado(ingrediente);
        }

        Destroy(gameObject);
    }
}
