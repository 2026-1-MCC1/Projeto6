using System.Collections;
using UnityEngine;

// Controla o ciclo de spawn dos ingredientes coletaveis pela mesa.
public class ItemSpawner : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float respawnDelay = 5f;

    [Header("Referência")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private AnimarGif[] telasGif;

    private bool esperandoRespawn = false;

    private void Start()
    {
        SpawnarItem();
    }

    public void ItemFoiColetado(IngredienteTipo ingrediente)
    {
        // Atualiza as telas da mesa imediatamente e agenda o proximo item.
        TrocarGifEmTodasAsTelas(ingrediente);

        if (!esperandoRespawn)
        {
            StartCoroutine(Respawn());
        }
    }

    private void TrocarGifEmTodasAsTelas(IngredienteTipo ingrediente)
    {
        int index = (int)ingrediente;

        foreach (AnimarGif tela in telasGif)
        {
            if (tela != null)
            {
                tela.TrocarGif(index);
            }
        }
    }

    private IEnumerator Respawn()
    {
        esperandoRespawn = true;

        // Espera alguns segundos para o tabuleiro nao ficar vazio apenas por um frame.
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

        if (itemPrefabs.Length == 0)
        {
            Debug.LogError("Nenhum item prefab definido!");
            return;
        }

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        int itemIndex = Random.Range(0, itemPrefabs.Length);

        Transform ponto = spawnPoints[spawnIndex];

        // Sorteia um ponto e um prefab diferentes a cada respawn para variar a rodada.
        GameObject item = Instantiate(itemPrefabs[itemIndex], ponto.position, ponto.rotation);

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
