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

    // Adiciona pontos com base no tipo do ingrediente (pois cada ingrediente tem uma pontuação diferente)
    public void AdicionarPontos(IngredienteTipo ingrediente)
    {
        switch (ingrediente)
        {
            case IngredienteTipo.Trigo:
                score += 10;
                break;

            case IngredienteTipo.Ovo:
                score += 20;
                break;

            case IngredienteTipo.Leite:
                score += 15;
                break;

            case IngredienteTipo.Chocolate:
                score += 25;
                break;

            case IngredienteTipo.Morango:
                score += 25;
                break;
        }

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