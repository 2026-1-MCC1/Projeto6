using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Configuração")]
    // Prefab do item (leite, ovo, etc)
    [SerializeField] private GameObject itemPrefab;
    // Pontos onde o item pode nascer
    [SerializeField] private Transform[] spawnPoints;
    // Tempo para respawn
    [SerializeField] private float respawnDelay = 5f;

    private bool esperandoRespawn = false;

    private void Start()
    {
        SpawnarItem();
    }

    // Chamado quando o item é coletado
    public void ItemFoiColetado()
    {
        if (!esperandoRespawn)
        {
            StartCoroutine(Respawn());
        }
    }

    // Espera e cria outro item
    private IEnumerator Respawn()
    {
        esperandoRespawn = true;

        Debug.Log("Item coletado. Respawn em " + respawnDelay + " segundos.");

        yield return new WaitForSeconds(respawnDelay);

        SpawnarItem();

        esperandoRespawn = false;
    }

    // Spawn do item em um ponto aleatório da lista
    void SpawnarItem()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("Nenhum spawn point definido!");
            return;
        }

        int index = Random.Range(0, spawnPoints.Length);

        Transform ponto = spawnPoints[index];

        GameObject item = Instantiate(
            itemPrefab,
            ponto.position,
            ponto.rotation
        );

        // conecta o spawner no item
        ItemColetavel coletavel = item.GetComponent<ItemColetavel>();

        if (coletavel != null)
        {
            coletavel.DefinirSpawner(this);
        }
    }
}