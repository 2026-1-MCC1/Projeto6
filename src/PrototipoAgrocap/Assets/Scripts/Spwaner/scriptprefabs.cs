using UnityEngine;


// Representa um item que pode ser coletado pela bola.
// Este script foi mantido para garantir compatibilidade
// com prefabs antigos que ainda utilizam este componente.
public class ItemColetavel : MonoBehaviour

{
    // Tipo de ingrediente associado a este item.
    // Definido pelo Inspector da Unity.
    [SerializeField] private IngredienteTipo ingrediente;
    // Referência ao spawner responsável por criar o item.
    // Usado para notificar quando o item é coletado.
    private ItemSpawner spawner;

    // Define qual spawner criou este item.
    public void DefinirSpawner(ItemSpawner s)
    {
        spawner = s;
    }

    // Chamado automaticamente pela Unity quando outro collider
    // entra na área de trigger deste objeto.

    private void OnTriggerEnter(Collider other)
    {
        // Ignora qualquer objeto que não seja a bola.
        if (!other.CompareTag("Ball")) return;
        // Exibe no console qual item foi coletado.
        Debug.Log("Item ooletado: " + ingrediente);
        // Exibe no console qual item foi coletado.
        if (spawner != null)
        {
            spawner.ItemFoiColetado(ingrediente);
        }
        // Remove o item da cena após a coleta.
        Destroy(gameObject);
    }
}
