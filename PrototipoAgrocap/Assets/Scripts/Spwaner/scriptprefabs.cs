using UnityEngine;

// Item coletável pela bola
public class ItemColetavel : MonoBehaviour
{
    private ItemSpawner spawner;

    public void DefinirSpawner(ItemSpawner s)
    {
        spawner = s;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball")) return;

        Debug.Log("Item coletado: " + gameObject.name);

        if (spawner != null)
        {
            spawner.ItemFoiColetado();
        }

        Destroy(gameObject);
    }
}