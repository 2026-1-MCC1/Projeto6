using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public float tempoEntreSpawns = 10f;

    private bool esperandoRespawn = false;

    private Vector3 centroSpawn = new Vector3(4.2066f, -0.2f, -7.4849f);
    private Vector3 tamanhoSpawn = new Vector3(3.898744f, 0.4f, 9.242114f);

    void Start()
    {
        Debug.Log("Spawner iniciado. Vai respawnar somente depois da coleta.");
    }

    public void ItemFoiColetado()
    {
        if (!esperandoRespawn)
        {
            StartCoroutine(RespawnDepoisDoTempo());
        }
    }

    IEnumerator RespawnDepoisDoTempo()
    {
        esperandoRespawn = true;

        Debug.Log("Item coletado. Respawn em " + tempoEntreSpawns + " segundos.");

        yield return new WaitForSeconds(tempoEntreSpawns);

        Spawnar();

        esperandoRespawn = false;
    }

    void Spawnar()
{
    float minX = centroSpawn.x - tamanhoSpawn.x / 2f;
    float maxX = centroSpawn.x + tamanhoSpawn.x / 2f;

    float minZ = centroSpawn.z - tamanhoSpawn.z / 2f;
    float maxZ = centroSpawn.z + tamanhoSpawn.z / 2f;

    float x = Random.Range(minX, maxX);
    float z = Random.Range(minZ, maxZ);
    float y = centroSpawn.y + (tamanhoSpawn.y / 2f) + 0.2f;

    Vector3 posicaoSpawn = new Vector3(x, y, z);

    GameObject itemCriado = Instantiate(itemPrefab, posicaoSpawn, Quaternion.identity);

    IngredientPickup pickup = itemCriado.GetComponent<IngredientPickup>();
    if (pickup != null)
    {
        pickup.spawner = this;
    }
    else
    {
        Debug.LogWarning("O prefab criado não tem IngredientPickup.");
    }

    Debug.Log("Item criado com sucesso: " + itemCriado.name + " na posição " + posicaoSpawn);
}
}