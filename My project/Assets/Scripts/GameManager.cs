using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Configuração")]
    // Quantidade inicial de vidas do jogador
    [SerializeField] private int vidas = 3;

    // Indica se o jogo já terminou
    private bool acabou = false;

    //Remove uma vida do jogador
    public void PerderVida()
    {
        // Se o jogo já acabou
        if (acabou) return;

        // Tira o número de vidas
        vidas--;

        // Exibe no console
        Debug.Log("Vidas restantes: " + vidas);

        // Verifica se acabou o jogo
        if (vidas <= 0)
        {
            GameOver();
        }
    }
    public bool JogoAcabou()
    {
        return acabou;
    }
    //Chamado quando o jogador perde todas as vidas
    private void GameOver()
    {
        // Marca o jogo como finalizado
        acabou = true;
        Debug.Log("GAME OVER");
        // Aqui você pode: - parar o jogo - mostrar UI
        Time.timeScale = 0f;
    }
}
