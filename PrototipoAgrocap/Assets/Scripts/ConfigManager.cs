using UnityEngine;
using UnityEngine.SceneManagement;

public class ConfigManager : MonoBehaviour
{
    public GameObject meuMenuConfig; // Arraste o objeto MenuConfig (o painel) para cá
    public GameObject meuBotao;      // Arraste o objeto do Botão (da engrenagem) para cá

    void Start()
    {
        // Garante que o menu comece fechado
        if (meuMenuConfig != null)
        {
            meuMenuConfig.SetActive(false);
        }

        // Lógica de esconder o botão dependendo da cena
        // Se a cena atual for "Menu", o botão some.
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            meuBotao.SetActive(false);
        }
        else
        {
            // Se for "Game" ou qualquer outra, ele aparece
            meuBotao.SetActive(true);
        }
    }

    // Essa é a função que o botão vai chamar ao ser clicado
    public void AlternarMenu()
    {
        // Isso TEM que aparecer no console, independente do menu config
        Debug.Log("O BOTÃO FOI CLICADO! O SCRIPT ESTÁ RODANDO!");

        if (meuMenuConfig != null)
        {
            meuMenuConfig.SetActive(!meuMenuConfig.activeSelf);
        }
    }
    }