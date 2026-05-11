using System.Collections;
using UnityEngine;

// Controla o ciclo de criação (spawn) dos ingredientes coletáveis.
// Responsável por gerar itens, aguardar respawn e atualizar as telas da mesa.
public class ItemSpawner : MonoBehaviour
{
    private const string NomeMarcadorBaseItem = "BaseSpawn";

    //Lista de prefabs que podem ser spawnados.
    [Header("Configuração")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private float alturaAcimaDaMesa = 0.02f;

    [Header("Referência")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private AnimarGif[] telasGif;
    [SerializeField] private Transform referenciaBaseSpawn;

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
    // Chamado quando um item é coletado.
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
    // Responsável por escolher e instanciar um item aleatório.
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
        Transform referencia = referenciaBaseSpawn != null ? referenciaBaseSpawn : ponto;

        // Sorteia um ponto e um prefab diferentes a cada respawn para variar a rodada.
        GameObject item = Instantiate(itemPrefabs[itemIndex], ponto.position, ponto.rotation);
        AlinharItemComMesa(item, referencia.position, referencia.up);

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

    private void AlinharItemComMesa(GameObject item, Vector3 pontoMesa, Vector3 normalMesa)
    {
        if (normalMesa.sqrMagnitude <= Mathf.Epsilon)
        {
            normalMesa = Vector3.up;
        }
        else
        {
            normalMesa.Normalize();
        }

        float pontoBaseItem = ObterPontoBaseItem(item, normalMesa);
        float pontoBaseMesa = Vector3.Dot(pontoMesa, normalMesa);
        float deslocamento = pontoBaseMesa + alturaAcimaDaMesa - pontoBaseItem;
        item.transform.position += normalMesa * deslocamento;
    }

    private float ObterPontoBaseItem(GameObject item, Vector3 normalMesa)
    {
        foreach (Transform filho in item.GetComponentsInChildren<Transform>(true))
        {
            if (filho.name == NomeMarcadorBaseItem)
            {
                return Vector3.Dot(filho.position, normalMesa);
            }
        }

        if (TryGetPontoMaisBaixoVisual(item, normalMesa, out float pontoMaisBaixo))
        {
            return pontoMaisBaixo;
        }

        return Vector3.Dot(item.transform.position, normalMesa);
    }

    private bool TryGetPontoMaisBaixoVisual(GameObject item, Vector3 normalMesa, out float pontoMaisBaixo)
    {
        Renderer[] renderers = item.GetComponentsInChildren<Renderer>();
        pontoMaisBaixo = float.PositiveInfinity;
        bool encontrouRenderer = false;

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null || !renderer.enabled)
            {
                continue;
            }

            encontrouRenderer = true;
            Bounds bounds = renderer.localBounds;
            Vector3 center = bounds.center;
            Vector3 extents = bounds.extents;

            for (int x = -1; x <= 1; x += 2)
            {
                for (int y = -1; y <= 1; y += 2)
                {
                    for (int z = -1; z <= 1; z += 2)
                    {
                        Vector3 localCorner = center + Vector3.Scale(extents, new Vector3(x, y, z));
                        Vector3 corner = renderer.transform.TransformPoint(localCorner);
                        pontoMaisBaixo = Mathf.Min(pontoMaisBaixo, Vector3.Dot(corner, normalMesa));
                    }
                }
            }
        }

        return encontrouRenderer;
    }

}
