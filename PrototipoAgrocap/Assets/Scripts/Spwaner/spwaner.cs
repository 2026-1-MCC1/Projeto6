using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float respawnDelay = 5f;

    [Header("Referência")]
    [SerializeField] private Inventory inventory;

    private bool esperandoRespawn = false;

    private void Start()
    {
        SpawnarItem();
    }

    public void ItemFoiColetado()
    {
        if (!esperandoRespawn)
        {
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        esperandoRespawn = true;

        Debug.Log("Respawn em " + respawnDelay + " segundos");

        yield return new WaitForSeconds(respawnDelay);

        SpawnarItem();

        esperandoRespawn = false;
    }

    private void SpawnarItem()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Nenhum spawn point definido!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);
        Transform ponto = spawnPoints[index];

        GameObject item = Instantiate(itemPrefab, ponto.position, ponto.rotation);

        IngredientPickup pickup = item.GetComponent<IngredientPickup>();

        if (pickup != null)
        {
            pickup.Configurar(this, inventory);
        }
        else
        {
            Debug.LogError("O prefab não tem IngredientPickup!");
        }
    }
}