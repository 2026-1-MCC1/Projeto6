using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private int lifes = 3;

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
}