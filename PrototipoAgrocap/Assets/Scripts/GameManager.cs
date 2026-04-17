using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuração")]
    // Quantidade inicial de vidas do jogador
    [SerializeField] private int lifes = 3;
    // Referências para o inventário do jogador
    [SerializeField] private Inventory inventory;
    // Referência para o score manager para atualizar a pontuação final no Game Over
    [SerializeField] private ScoreManager scoreManager;

    // Indica se o jogo já terminou (evita execução de lógica após Game Over)
    private bool end = false;

    [Header("UI")]
    // Canvas do HUD
    [SerializeField] private GameObject CanvaLife;
    // Texto das vidas
    [SerializeField] private TextMeshProUGUI TextLifes;
    // Canvas do Game Over
    [SerializeField] private GameObject CanvaGameOver;

    void Start()
    {
        // HUD começa ativo
        CanvaLife.SetActive(true);
        // Game Over começa escondido
        CanvaGameOver.SetActive(false);

        AtualizarUI();
    }

    public void PerderVida()
    {
        if (end) return;

        lifes--;

        AtualizarUI();

        // Exibe no console para debug
        Debug.Log("Vidas restantes: " + lifes);
        // Verifica se as vidas acabaram
        if (lifes <= 0)
        {
            GameOver();
        }
    }

    public void AtualizarUI()
    {
        if (TextLifes == null)
        {
            // Para segurança evita erro se o texto não estiver conectado
            Debug.LogError("Texto de vidas NÃO está conectado!");
            return;
        }
        // Atualiza o texto exibido na tela
        TextLifes.text = "Vidas: " + lifes;
    }

    public bool JogoAcabou()
    {
        return end;
    }

    private void CalcularResultadosFinais()
    {
        int pontos = 0;

        int especial = 0;
        int choc = 0;
        int mora = 0;
        int simples = 0;

        while (inventory.trigo >= 1 &&
               inventory.ovo >= 1 &&
               inventory.leite >= 1 &&
               inventory.chocolate >= 1 &&
               inventory.morango >= 1)
        {
            inventory.trigo--;
            inventory.ovo--;
            inventory.leite--;
            inventory.chocolate--;
            inventory.morango--;

            especial++;
            pontos += 1000;
        }

        while (inventory.trigo >= 1 &&
               inventory.ovo >= 1 &&
               inventory.leite >= 1 &&
               inventory.chocolate >= 1)
        {
            inventory.trigo--;
            inventory.ovo--;
            inventory.leite--;
            inventory.chocolate--;

            choc++;
            pontos += 500;
        }

        while (inventory.trigo >= 1 &&
               inventory.ovo >= 1 &&
               inventory.leite >= 1 &&
               inventory.morango >= 1)
        {
            inventory.trigo--;
            inventory.ovo--;
            inventory.leite--;
            inventory.morango--;

            mora++;
            pontos += 500;
        }

        while (inventory.trigo >= 1 &&
               inventory.ovo >= 1 &&
               inventory.leite >= 1)
        {
            inventory.trigo--;
            inventory.ovo--;
            inventory.leite--;

            simples++;
            pontos += 250;
        }

        GameResults.ScoreFinal = pontos;

        GameResults.BoloEspecial = especial;
        GameResults.BoloChocolate = choc;
        GameResults.BoloMorango = mora;
        GameResults.BoloSimples = simples;
    }

    // Executado quando o jogador perde todas as vidas
    private void GameOver()
    {
        end = true;

        Debug.Log("Game Over");
        // Esconde o HUD (vidas)
        CanvaLife.SetActive(false);
        // Mostra a tela de Game Over (com botões)
        CanvaGameOver.SetActive(true);
        CalcularResultadosFinais();
        Time.timeScale = 0f;
    }
}