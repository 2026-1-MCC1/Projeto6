using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuração")]
    // Quantidade inicial de vidas do jogador
    [SerializeField] private int lifes = 3;

    // Indica se o jogo já terminou
    private bool end = false;

    [Header("UI")]
    // Texto das as vidas
    [SerializeField] private TextMeshProUGUI TextLifes;
    // Texto de Game Over
    [SerializeField] private GameObject TextGameOver;

    public void Start()
    {
        AtualizarUI();
        TextGameOver.SetActive(false);
    }
    //Remove uma vida do jogador
    public void PerderVida()
    {
        // Se o jogo já acabou
        if (end) return;
        // Tira o número de vidas
        lifes--;
        AtualizarUI();
        // Exibe no console
        Debug.Log("Vidas restantes: " + lifes);
        // Verifica se acabou o jogo
        if (lifes <= 0)
        {
            GameOver();
        }
    }
    public void AtualizarUI()
    {
        if (TextLifes == null)
        {
            Debug.LogError("Texto de vidas NÃO está conectado!");
            return;
        }

        TextLifes.text = "Vidas: " + lifes;
    }

    public bool JogoAcabou()
    {
        return end;
    }
    //Chamado quando o jogador perde todas as vidas
    private void GameOver()
    {
        // Marca o jogo como finalizado
        end = true;
        Debug.Log("Game Over");
        // Aqui para o jogo
        Time.timeScale = 0f;
        // Mostra texto na tela
        TextGameOver.SetActive(true);
    }
}



