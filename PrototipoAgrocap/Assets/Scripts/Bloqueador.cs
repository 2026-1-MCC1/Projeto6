using UnityEngine;

// Fecha o corredor de saida depois que a bola entra definitivamente na mesa.
public class Bloqueador : MonoBehaviour
{
    [Header("ConfiguraÃ§Ãµes")]
    [SerializeField] private Collider paredeFisica; // Arraste o Box Collider da parede aqui
    
    private bool bolaSaiuDoCorredor = false;

    void Start()
    {
        // Garante que a parede comece aberta para a bola sair
        paredeFisica.isTrigger = true; 
    }

    // Coloque este objeto UM POUCO DEPOIS da saÃ­da do corredor
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.name.Contains("Bolinha"))
        {
            // A bola passou pelo sensor na mesa
            bolaSaiuDoCorredor = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (bolaSaiuDoCorredor && (other.CompareTag("Player") || other.name.Contains("Bolinha")))
        {
            // Agora que ela saiu TOTALMENTE, viramos uma parede sÃ³lida
            paredeFisica.isTrigger = false;
            Debug.Log("Passagem fechada com seguranÃ§a!");
        }
    }
    
    // FunÃ§Ã£o para quando o jogador perder a vida e precisar lanÃ§ar de novo
    public void ResetarPassagem()
    {
        bolaSaiuDoCorredor = false;
        paredeFisica.isTrigger = true;
    }
}
