using TMPro;
using UnityEngine;

// Gerencia a pontuação do jogo baseada nos ingredientes coletados.
// Cada ingrediente possui um valor específico em pontos.
public class ScoreManager : MonoBehaviour
{
    [Header("Pontuação")]
    // Armazena a pontuação atual do jogador
    private int pontos = 0;

    [Header("UI")]

    // Referência ao texto que exibe os pontos na tela
    [SerializeField] private TextMeshProUGUI textoPontos;

    // Inicializa o sistema atualizando a UI com o valor inicial
    void Start()
    {
        AtualizarUI();
    }

    /// Adiciona pontos com base no tipo de ingrediente coletado
    public void AdicionarPontos(string ingrediente)
    {
        // Verifica qual ingrediente foi coletado e adiciona pontos correspondentes
        switch (ingrediente)
        {
            case "trigo":
                pontos += 10; // trigo vale 10 pontos
                break;

            case "ovo":
                pontos += 20; // ovo vale 20 pontos
                break;

            case "leite":
                pontos += 15; // leite vale 15 pontos
                break;

            case "chocolate":
                pontos += 25; // chocolate vale 25 pontos
                break;
        }

        // Exibe no console para debug
        Debug.Log("Pontos: " + pontos);

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
            textoPontos.text = "Pontos: " + pontos;
        }
        else
        {
            // Mensagem de erro caso não esteja conectado
            Debug.LogError("Texto de pontos NÃO está conectado!");
        }
    }
}