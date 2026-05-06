using TMPro;
using UnityEngine;

// Tipos de ingredientes disponíveis no jogo
public enum IngredienteTipo
{
    Trigo,
    Ovo,
    Leite,
    Chocolate,
    Morango
}

// Gerencia a pontuação baseada nos ingredientes coletados
public class ScoreManager : MonoBehaviour
{
    [Header("Pontuação")]
    // Armazena a pontuação atual do jogador
    private int score = 0;

    [Header("UI")]
    // Texto que exibe a pontuação na tela
    [SerializeField] private TextMeshProUGUI textoPontos;

    void Start()
    {
        AtualizarUI();
    }

    // Retorna o valor de cada ingrediente
    // Essa regra tambem e usada no Scoreboard para contar os ingredientes restantes
    public static int ObterValorIngrediente(IngredienteTipo ingrediente)
    {
        switch (ingrediente)
        {
            case IngredienteTipo.Trigo:
                return 10;

            case IngredienteTipo.Leite:
                return 15;

            case IngredienteTipo.Ovo:
                return 20;

            case IngredienteTipo.Chocolate:
                return 25;

            case IngredienteTipo.Morango:
                return 25;

            default:
                return 0;
        }
    }

    // Adiciona pontos com base no tipo do ingrediente (pois cada ingrediente tem uma pontuacao diferente)
    public void AdicionarPontos(IngredienteTipo ingrediente)
    {
        // Pega o valor definido para o ingrediente coletado
        int valorIngrediente = ObterValorIngrediente(ingrediente);

        // Soma o valor do ingrediente na pontuacao atual
        score += valorIngrediente;

        Debug.Log("Pontos: " + score);

        AtualizarUI();
    }

    // Atualiza o texto da UI
    private void AtualizarUI()
    {
        if (textoPontos != null)
        {
            textoPontos.text = "Pontos: " + score;
        }
        else
        {
            Debug.LogError("Texto de pontos NÃO está conectado!");
        }
    }

    public void AdicionarPontosDireto(int valor)
    {
        score += valor;
        AtualizarUI();
    }
}