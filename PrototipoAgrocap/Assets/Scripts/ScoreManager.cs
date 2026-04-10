using TMPro;
using UnityEngine;

// Gerencia a pontuação do jogo baseada nos ingredientes coletados.
// Cada ingrediente possui um valor específico em pontos.
public class ScoreManager : MonoBehaviour
{
    [Header("Pontuação")]
    // Armazena a pontuação atual do jogador
    private int score = 0;

    [Header("UI")]

    // Referência ao texto que exibe os pontos na tela
    [SerializeField] private TextMeshProUGUI textoPontos;

    // Inicializa o sistema atualizando a UI com o valor inicial
    void Start()
    {
        AtualizarUI();
    }

    // Adiciona pontos com base no tipo de ingrediente coletado
    public void AdicionarPontos(string ingrediente)
    {
        // Verifica qual ingrediente foi coletado e adiciona pontos correspondentes
        switch (ingrediente)
        {
            case "trigo":
                score += 10; 
                break;

            case "ovo":
                score += 20; 
                break;

            case "leite":
                score += 15; 
                break;

            case "chocolate":
                score += 25;
                break;
        }

        // Exibe no console para debug
        Debug.Log("Pontos: " + score);

        // Atualiza o valor na interface
        AtualizarUI();
    }

    // Atualiza o texto da UI com a pontuação atual
    private void AtualizarUI()
    {
        // Verifica se o texto foi corretamente atribuído
        if (textoPontos != null)
        {
            // Atualiza o texto exibido na tela
            textoPontos.text = "Pontos: " + score;
        }
        else
        {
            // Mensagem de erro caso não esteja conectado
            Debug.LogError("Texto de pontos NÃO está conectado!");
        }
    }
}