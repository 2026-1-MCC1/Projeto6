using UnityEngine;

public class Bloqueador : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Collider paredeFisica; // Arraste o Box Collider da parede aqui
    
    private bool bolaSaiuDoCorredor = false;

    void Start()
    {
        // Garante que a parede comece aberta para a bola sair
        paredeFisica.isTrigger = true; 
    }

    // Coloque este objeto UM POUCO DEPOIS da saída do corredor
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
            // Agora que ela saiu TOTALMENTE, viramos uma parede sólida
            paredeFisica.isTrigger = false;
            Debug.Log("Passagem fechada com segurança!");
        }
    }
    
    // Função para quando o jogador perder a vida e precisar lançar de novo
    public void ResetarPassagem()
    {
        bolaSaiuDoCorredor = false;
        paredeFisica.isTrigger = true;
    }
}