using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Vai para o menu
    public void ReturnMenu()
    {
        //despausa o jogo
        Time.timeScale = 1f;
        // Carrega a cena "menu"
        SceneManager.LoadScene("Menu");
    }

    // Vai para o scoreboard
    public void ReturnScoreboard()
    {
        Time.timeScale = 1f;
        // Carrega a cena "Scoreboard"
        SceneManager.LoadScene("Scoreboard");
    }
}