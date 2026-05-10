using System.Collections;
using UnityEngine;

// Controla o ciclo de criação (spawn) dos ingredientes coletáveis.
// Responsável por gerar itens, aguardar respawn e atualizar as telas da mesa.
public class ItemSpawner : MonoBehaviour
{
    //Lista de prefabs que podem ser spawnados.
    [Header("Configuração")]
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private float alturaAcimaDaMesa = 0.02f;

    [Header("Referência")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private AnimarGif[] telasGif;
    [SerializeField] private Transform referenciaFloor;

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
        ObterPlanoDoFloor(ponto, out Vector3 pontoFloor, out Vector3 normalFloor);

        // Sorteia um ponto e um prefab diferentes a cada respawn para variar a rodada.
        GameObject item = Instantiate(itemPrefabs[itemIndex], ponto.position, ponto.rotation);
        AlinharItemComMesa(item, pontoFloor, normalFloor);

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

        if (!TryGetPontoMaisBaixoVisual(item, normalMesa, out float pontoMaisBaixo))
        {
            return;
        }

        float pontoDaMesa = Vector3.Dot(pontoMesa, normalMesa);
        float deslocamento = pontoDaMesa + alturaAcimaDaMesa - pontoMaisBaixo;
        item.transform.position += normalMesa * deslocamento;
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

    private void ObterPlanoDoFloor(Transform pontoSpawn, out Vector3 pontoFloor, out Vector3 normalFloor)
    {
        Transform floor = referenciaFloor != null ? referenciaFloor : EncontrarFloorMaisProximo(pontoSpawn.position);

        if (floor != null && TryGetTopoDoFloor(floor, out pontoFloor, out normalFloor))
        {
            return;
        }

        pontoFloor = pontoSpawn.position;
        normalFloor = pontoSpawn.up;
    }

    private Transform EncontrarFloorMaisProximo(Vector3 posicao)
    {
        Renderer[] renderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        Transform floorMaisProximo = null;
        float menorDistancia = float.PositiveInfinity;

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null || !renderer.enabled || !renderer.gameObject.activeInHierarchy || renderer.gameObject.name != "Floor")
            {
                continue;
            }

            float distancia = (renderer.bounds.ClosestPoint(posicao) - posicao).sqrMagnitude;

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                floorMaisProximo = renderer.transform;
            }
        }

        return floorMaisProximo;
    }

    private bool TryGetTopoDoFloor(Transform floor, out Vector3 pontoFloor, out Vector3 normalFloor)
    {
        normalFloor = floor.up;

        BoxCollider boxCollider = floor.GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 topoLocal = boxCollider.center + Vector3.up * (boxCollider.size.y * 0.5f);
            pontoFloor = floor.TransformPoint(topoLocal);
            return true;
        }

        MeshFilter meshFilter = floor.GetComponent<MeshFilter>();

        if (meshFilter != null && meshFilter.sharedMesh != null)
        {
            Bounds bounds = meshFilter.sharedMesh.bounds;
            Vector3 topoLocal = bounds.center + Vector3.up * bounds.extents.y;
            pontoFloor = floor.TransformPoint(topoLocal);
            return true;
        }

        pontoFloor = floor.position;
        return true;
    }
}
