using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

// Controla o fluxo de entrada do jogador no menu principal.
public class MenuController : MonoBehaviour
{
    private const string DefaultPlayerName = "Jogador";
    private const string PlayerNameKey = "MenuController_NomeJogador";

    private static bool playerNameLoaded;

    public static string NomeJogador = DefaultPlayerName;

    public VideoPlayer videoPlayer;
    public GameObject MenuOpcoes;
    public RawImage imagemDoVideo;
    public TextMeshProUGUI titulo;
    public TextMeshProUGUI subtitulo;
    public GameObject painelControles;
    public TMP_InputField inputNome;
    public GameObject MenuNome;

    private void Awake()
    {
        ObterNomeJogador();
    }

    private void Start()
    {
        // A cena sempre comeca no video de abertura e so libera os paineis depois da interacao.
        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(false);
        }

        if (MenuNome != null)
        {
            MenuNome.SetActive(false);
        }

        if (inputNome != null)
        {
            inputNome.text = ObterNomeJogador();
        }
    }

    private void Update()
    {
        if (videoPlayer != null && videoPlayer.isPlaying && Input.anyKeyDown)
        {
            AtivarNome();
        }

        if (MenuNome != null && MenuNome.activeSelf &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ConfirmarNome();
        }
    }

    public static void SalvarNomeJogador(string nome)
    {
        // Mantem o ultimo nome digitado salvo para reaproveitar nas proximas partidas.
        if (string.IsNullOrWhiteSpace(nome))
        {
            nome = DefaultPlayerName;
        }

        NomeJogador = nome.Trim();
        playerNameLoaded = true;

        PlayerPrefs.SetString(PlayerNameKey, NomeJogador);
        PlayerPrefs.Save();
    }

    public static string ObterNomeJogador()
    {
        if (!playerNameLoaded)
        {
            NomeJogador = PlayerPrefs.GetString(PlayerNameKey, DefaultPlayerName);
            playerNameLoaded = true;
        }

        if (string.IsNullOrWhiteSpace(NomeJogador))
        {
            NomeJogador = DefaultPlayerName;
        }

        return NomeJogador.Trim();
    }

    private void AtivarNome()
    {
        // Troca do video inicial para o menu jogavel.
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
        }

        if (imagemDoVideo != null)
        {
            imagemDoVideo.enabled = false;
        }

        if (titulo != null)
        {
            titulo.gameObject.SetActive(false);
        }

        if (subtitulo != null)
        {
            subtitulo.gameObject.SetActive(false);
        }

        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(true);
        }
    }

    public void JogarJogo()
    {
        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(false);
        }

        if (MenuNome != null)
        {
            MenuNome.SetActive(true);
        }
    }

    public void ConfirmarNome()
    {
        string nomeDigitado = inputNome != null ? inputNome.text : DefaultPlayerName;
        SalvarNomeJogador(nomeDigitado);

        // A partida nova sempre comeca com um estado limpo e com o nome confirmado no menu.
        GameResults.PrepararNovaPartida();
        GameResults.DefinirNomeJogador(NomeJogador);

        Debug.Log("Nome do jogador salvo: " + NomeJogador);
        SceneManager.LoadScene("Game");
    }

    public void SairDoJogo()
    {
        Debug.Log("Botao Sair clicado.");
        Application.Quit();
    }

    public void AbrirControles()
    {
        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(false);
        }

        if (painelControles != null)
        {
            painelControles.SetActive(true);
        }
    }
}
