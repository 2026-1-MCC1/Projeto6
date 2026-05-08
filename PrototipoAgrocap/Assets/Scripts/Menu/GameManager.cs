using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Configuracao")]
    [SerializeField] private int lifes = 3;
    [SerializeField] private Inventory inventory;
    [SerializeField] private ScoreManager scoreManager;

    private bool end;

    [Header("UI")]
    [SerializeField] private GameObject CanvaLife;
    [SerializeField] private TextMeshProUGUI TextLifes;
    [SerializeField] private GameObject CanvaGameOver;

    private void Start()
    {
        CanvaLife.SetActive(true);
        CanvaGameOver.SetActive(false);
        AtualizarUI();
    }

    public void PerderVida()
    {
        if (end)
        {
            return;
        }

        lifes--;
        AtualizarUI();

        Debug.Log("Vidas restantes: " + lifes);
        if (lifes <= 0)
        {
            GameOver();
        }
    }

    public void AtualizarUI()
    {
        if (TextLifes == null)
        {
            Debug.LogError("Texto de vidas nao esta conectado.");
            return;
        }

        TextLifes.text = "Vidas: " + lifes;
    }

    public bool JogoAcabou()
    {
        return end;
    }

    private void CalcularResultadosFinais()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory esta null no GameManager.");
            return;
        }

        int trigoColetado = inventory.Trigo;
        int ovoColetado = inventory.Ovo;
        int leiteColetado = inventory.Leite;
        int chocolateColetado = inventory.Chocolate;
        int morangoColetado = inventory.Morango;

        int pontos = 0;

        int especial = 0;
        int choc = 0;
        int mora = 0;
        int simples = 0;

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

        int pontosIngredientesRestantes =
            (inventory.Trigo * ScoreManager.ObterValorIngrediente(IngredienteTipo.Trigo)) +
            (inventory.Ovo * ScoreManager.ObterValorIngrediente(IngredienteTipo.Ovo)) +
            (inventory.Leite * ScoreManager.ObterValorIngrediente(IngredienteTipo.Leite)) +
            (inventory.Chocolate * ScoreManager.ObterValorIngrediente(IngredienteTipo.Chocolate)) +
            (inventory.Morango * ScoreManager.ObterValorIngrediente(IngredienteTipo.Morango));

        pontos += pontosIngredientesRestantes;

        GameResults.DefinirNomeJogador(MenuController.ObterNomeJogador());
        GameResults.ScoreFinal = pontos;

        GameResults.BoloEspecial = especial;
        GameResults.BoloChocolate = choc;
        GameResults.BoloMorango = mora;
        GameResults.BoloSimples = simples;

        GameResults.Trigo = trigoColetado;
        GameResults.Ovo = ovoColetado;
        GameResults.Leite = leiteColetado;
        GameResults.Chocolate = chocolateColetado;
        GameResults.Morango = morangoColetado;

        GameResults.TrigoRestante = inventory.Trigo;
        GameResults.OvoRestante = inventory.Ovo;
        GameResults.LeiteRestante = inventory.Leite;
        GameResults.ChocolateRestante = inventory.Chocolate;
        GameResults.MorangoRestante = inventory.Morango;

        GameResults.SalvarResultados();
        Debug.Log("Resultados finais salvos com sucesso.");
    }

    private void GameOver()
    {
        end = true;
        CanvaLife.SetActive(false);
        CanvaGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void IrParaScoreboard()
    {
        Time.timeScale = 1f;
        CalcularResultadosFinais();
        SceneManager.LoadScene("Scoreboard");
    }

    public void ReturnMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
