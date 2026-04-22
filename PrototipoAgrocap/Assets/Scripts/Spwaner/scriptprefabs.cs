using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public ItemSpawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Item coletado pelo Player: " + gameObject.name);

            if (spawner != null)
            {
                spawner.ItemFoiColetado();
            }
            else
            {
                Debug.LogWarning("ItemColetavel: referência do spawner está nula.");
            }

            Destroy(gameObject);
        }
    }
}