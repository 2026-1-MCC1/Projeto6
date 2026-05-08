using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Configuração : MonoBehaviour
{
    public GameObject MenuConfig; // Para atribuirmos o menu de configuração'
    public GameObject canvasparabotaoconfig;
    public Button BotaoConfig; // Para atribuirmos o botão de configuração do menu
    public Button Botaovoltar; // Para atribuirmos o botão de voltar do menu

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BotaoConfig.onClick.AddListener(AbrirConfig); // Adiciona o método AbrirConfig como ouvinte do clique no botão
        Botaovoltar.onClick.AddListener(FecharConfig); // Adiciona o método FecharConfig como ouvinte do clique no botão

    }

    // Update is called once per frame
    void Update()
    {

    }

    void AbrirConfig() // Método para abrir o menu de configuração'
    {
        MenuConfig.SetActive(true); // Ativa o menu de configuração para que ele apareça na tela
        canvasparabotaoconfig.SetActive(false); // Desativa o canvas do botão de configuração para que ele desapareça da tela
        Debug.Log("Menu de configuração aberto"); // Imprime uma mensagem no console para confirmar que o menu foi aberto
    }

    void FecharConfig()
    {
        MenuConfig.SetActive(false); // Desativa o menu de configuração para que ele desapareça da tela
        canvasparabotaoconfig.SetActive(true); // Ativa o canvas do botão de configuração para que ele apareça na tela
        Debug.Log("Menu de configuração fechado"); // Imprime uma mensagem no console para confirmar que o menu foi fechado
    }
}
