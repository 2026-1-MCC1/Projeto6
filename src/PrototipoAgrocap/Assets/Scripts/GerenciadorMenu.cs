using UnityEngine;
using UnityEngine.UI;

public class GerenciadorMenu : MonoBehaviour
{
    public GameObject painelConfig; // Para atribuirmos o painel de configuração do menu
    public Button botaoAbrir;   // Para atribuirmos o botão de abrir

    void Start()
    {
        // Para o jogo começar sem o menu aberto, garantindo que o tempo esteja normal
        // e o menu escondido
        ContinuarJogo();
        botaoAbrir.onClick.AddListener(AbrirMenu); // Adiciona o método AbrirMenu como ouvinte do clique no botão de abrir
    }

    public void AbrirMenu() // Método para abrir o menu de configuração
     // Chamado quando o jogador clicar no botão de abrir o menu
    {
        painelConfig.SetActive(true); // Aparece o menu de configuração
        botaoAbrir.gameObject.SetActive(false);  // Esconde o botão de abrir para evitar que o jogador clique novamente
        Time.timeScale = 0f;          // Congela o tempo do jogo (Pausa)
    }

    public void ContinuarJogo() // Método para fechar o menu de configuração e continuar o jogo
    // Chamado quando o jogador clicar no botão de continuar dentro do menu
    {
        painelConfig.SetActive(false); // Esconde o menu
        botaoAbrir.gameObject.SetActive(true);    // Mostra o botão de abrir de volta
        Time.timeScale = 1f;           // Faz o tempo voltar ao normal
    }
}