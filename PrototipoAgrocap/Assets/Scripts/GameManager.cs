using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // Calcula os resultados finais com base nos ingredientes coletados
    // e salva os valores para a cena de Scoreboard
    private void CalcularResultadosFinais()
    {
        // Verifica se o inventário foi conectado corretamente
        if (inventory == null)
        {
            Debug.LogError("ERRO: Inventory está NULL no GameManager!");
            return;
        }

        Debug.Log("Inventory conectado com sucesso.");
        Debug.Log("Trigo: " + inventory.Trigo);
        Debug.Log("Ovo: " + inventory.Ovo);
        Debug.Log("Leite: " + inventory.Leite);
        Debug.Log("Chocolate: " + inventory.Chocolate);
        Debug.Log("Morango: " + inventory.Morango);

        int pontos = 0;

        int especial = 0;
        int choc = 0;
        int mora = 0;
        int simples = 0;

        // Bolo especial: chocolate + morango
        while (inventory.Trigo >= 1 &&
               inventory.Ovo >= 1 &&
               inventory.Leite >= 1 &&
               inventory.Chocolate >= 1 &&
               inventory.Morango >= 1)
        {
            inventory.Trigo--;
            inventory.Ovo--;
            inventory.Leite--;
            inventory.Chocolate--;
            inventory.Morango--;

            especial++;
            pontos += 1000;
        }

        // Bolo de chocolate
        while (inventory.Trigo >= 1 &&
               inventory.Ovo >= 1 &&
               inventory.Leite >= 1 &&
               inventory.Chocolate >= 1)
        {
            inventory.Trigo--;
            inventory.Ovo--;
            inventory.Leite--;
            inventory.Chocolate--;

            choc++;
            pontos += 500;
        }

        // Bolo de morango
        while (inventory.Trigo >= 1 &&
               inventory.Ovo >= 1 &&
               inventory.Leite >= 1 &&
               inventory.Morango >= 1)
        {
            inventory.Trigo--;
            inventory.Ovo--;
            inventory.Leite--;
            inventory.Morango--;

            mora++;
            pontos += 500;
        }

        // Bolo simples
        while (inventory.Trigo >= 1 &&
               inventory.Ovo >= 1 &&
               inventory.Leite >= 1)
        {
            inventory.Trigo--;
            inventory.Ovo--;
            inventory.Leite--;

            simples++;
            pontos += 250;
        }

        Debug.Log("Antes de salvar em GameResults");

        // Salva a pontuação final
        GameResults.ScoreFinal = pontos;

        // Salva a quantidade de bolos criados
        GameResults.BoloEspecial = especial;
        GameResults.BoloChocolate = choc;
        GameResults.BoloMorango = mora;
        GameResults.BoloSimples = simples;

        // Salva os ingredientes que sobraram depois das receitas
        GameResults.TrigoRestante = inventory.Trigo;
        GameResults.OvoRestante = inventory.Ovo;
        GameResults.LeiteRestante = inventory.Leite;
        GameResults.ChocolateRestante = inventory.Chocolate;
        GameResults.MorangoRestante = inventory.Morango;

        // Salva os resultados para manter o Scoreboard mesmo se o jogo for parado
        GameResults.SalvarResultados();

        Debug.Log("Resultados salvos com sucesso!");
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
        Time.timeScale = 0f;
    }

    // Calcula resultados e vai para o Scoreboard
    public void IrParaScoreboard()
    {
        Time.timeScale = 1f;
        // Calcula tudo antes de sair da cena
        CalcularResultadosFinais();
        // Troca de cena
        SceneManager.LoadScene("Scoreboard");
    }

    // Vai para o menu
    public void ReturnMenu()
    {
        //despausa o jogo
        Time.timeScale = 1f;
        // Carrega a cena "menu"
        SceneManager.LoadScene("Menu");
    }
}