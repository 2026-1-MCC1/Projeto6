using UnityEngine;

// Detecta quando a bola coleta um ingrediente
public class IngredientPickup : MonoBehaviour
{
    [Header("Configuração")]

    // Tipo do ingrediente (definido no Inspector)
    [SerializeField] private IngredienteTipo ingrediente;

    [Header("Referência")]

    // Referência ao ScoreManager
    [SerializeField] private ScoreManager scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se é a bola
        if (!other.CompareTag("Ball")) return;

        Debug.Log($"Você pegou: {ingrediente}");

        // Envia para o ScoreManager
        scoreManager.AdicionarPontos(ingrediente);

        // Destroi o objeto após coleta
        Destroy(gameObject);
    }
}