using TMPro; // adicionado para desaparecer com o texto no menu principal
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // adicionado por conta do canva
using UnityEngine.Video; // adicionacdo por conta do video
using static Unity.Burst.Intrinsics.X86.Avx;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer; //vide
    public GameObject MenuOpcoes; // segundo menu
    public RawImage imagemDoVideo; // Imagem do video
    public TextMeshProUGUI titulo; // titulo
    public TextMeshProUGUI subtitulo; // subtitulo
    public GameObject painelControles; // painel
    public GameObject painelNome; // painel onde o jogador digita o nome
    public TMP_InputField inputNome; // campo de texto do nome
    public GameObject MenuNome;

    void Start()
    { //menu vai começar desativado
        if (MenuOpcoes != null) MenuOpcoes.SetActive(false);

        // painel de nome começa desativado
        if (MenuNome != null) MenuNome.SetActive(false);
    }

    void Update()
    {
        // Se o vídeo estiver aparecendo e você clicar ou apertar alguma tecla
        if (videoPlayer != null && videoPlayer.isPlaying && Input.anyKeyDown)
        {
            AtivarNome();
        }
    }

    void AtivarNome()
    {
        videoPlayer.Stop(); //o video vai parar
        // A imagem do video desaparace 
        if (imagemDoVideo != null) imagemDoVideo.enabled = false;

        videoPlayer.gameObject.SetActive(false);

        // O video, o titulo e o subtitulo desaparecem e o menu abre
        if (titulo != null) titulo.gameObject.SetActive(false);
        if (subtitulo != null) subtitulo.gameObject.SetActive(false);
        // menu ativado 
        if (MenuOpcoes != null) MenuOpcoes.SetActive(true);
    }

    // Codigo dos botoes 
    public void JogarJogo()
    {
        MenuOpcoes.SetActive(false);
        MenuNome.SetActive(true);
       
    }

    public void ConfirmarNome()
    {
        string nomeDigitado = inputNome.text;

        RankingAPI.NomeJogador = nomeDigitado;

        SceneManager.LoadScene("Game");
    }

    public void SairDoJogo()
    {
        Debug.Log("Botão Sair clicado!");
        Application.Quit(); // Fecha o jogo
    }
    public void AbrirControles()
    {
        MenuOpcoes.SetActive(false);
        painelControles.SetActive(true);
    }

}