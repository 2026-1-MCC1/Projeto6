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

    public VideoPlayer videoPlayer; //Para o video do menu inicial 
    public GameObject MenuOpcoes; //Cria um campo dentro do inspector para atribuir o canva de MenuOpcoes
    //Com o GameObject ( a gente criou diversos campos )

    public RawImage imagemDoVideo;//Para a imagem do video, que é o plano de fundo do menu, para esconder quando o video acabar
    public TextMeshProUGUI titulo; //Para o titulo do menu, para esconder quando o video acabar
    public TextMeshProUGUI subtitulo;//Para o subtitulo do menu, para esconder quando o video acabar
    public GameObject painelControles; //Cria um campo dentro do inspector para atribuir o canva do PainelControle
    public TMP_InputField inputNome;//Cria um campo dentro do inspector para atribuir a caixinha de texto onde o jogador digita o nome
    public GameObject MenuNome;//Cria um campo dentro do inspector para atribuir o canva do MenuNome, onde o jogador digita o nome
    public GameObject painelCreditos1; //Cria um campo dentro do inspector para atribuir o canva do PainelCreditos1
    public GameObject painelCreditos2; //Cria um campo dentro do inspector para atribuir o canva do PainelCreditos2
    //Criei dois paineis pois não iria caber em 1 so e um volta para o outro
    public GameObject painelHistorias1;//Cria um campo dentro do inspector para atribuir o canva do PainelHistorias1
    public GameObject painelHistorias2;//Cria um campo dentro do inspector para atribuir o canva do PainelHistorias2

    private void Awake() // O wake serve para quando o objeto que carrega esse script carregar na cena, ele executar essa 
        // função (ObterNomeJogador), buscando basicamente o banco de dados que utilizamos (PlayerPrefs) 
    {
        ObterNomeJogador();
    }

    private void Start()
    {
        // A cena sempre comeca no video de abertura e so libera os paineis depois da interacao com qualquer tecla ou click
        // Tambem garante que o jogo comece com apenas o video, escondendo o menu e todo o resto
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
            //Ele vai na memória do computador, pega o nome que foi usado da última vez e
            //já deixa escrito dentro da caixinha de texto
        }
    }

    
    private void Update()
      //O private void update é uma funcao que executa o código contido nela uma vez por frame de renderização (FPS)
      // Sendo (private) ela so é acessivel dentro da classe atual
      // (Void) indica que ela nao vai retornar nenhum valor 
    
    {
        // Se (IF) o video existir, estiver rodando e voce apertar qualquer tecla
        // Logo em seguida chama a funcao (Ativar Nome)l
        if (videoPlayer != null && videoPlayer.isPlaying && Input.anyKeyDown)
        {
            //Chama a funcao que desliga o video e faz aparecer o menu
            AtivarNome();
        }
        //Se (IF) o painel de digitar o nome estiver aberto e voce apertar a tecla ENTER
        // A tecla ENTER chama a funca (ConfirmarNome)
        if (MenuNome != null && MenuNome.activeSelf &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            //Chama a funcao que salva o nome e começa o jogo 
            ConfirmarNome();
        }
    }

    public static void SalvarNomeJogador(string nome)
        //Metodo principal de Java
        //Funcao que pode ser chamada de qualquer lugar para salvar o nome
        //Public garante que o metodo seja acessivel por qualquer classe 
        //Static permite que seja chamado diretamente pelo nome da classe,
        //sem precisar criar um objeto (novo)   
        // (String [] args) Permite receber argumentos de entrada via linha de comando

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

    public void VoltarDosControles()
    {
        if (painelControles != null)
        {
            painelControles.SetActive(false); // Esconde os controles
        }

        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(true); // Mostra o menu de opções novamente
        }
    }

    public void AbrirCreditos1()
    {
        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(false);
        }

        if (painelCreditos1!= null)
        {
            painelCreditos1.SetActive(true);
        }
    }
    public void AbrirCreditos2()
    {
        if (painelCreditos1 != null)
        {
            painelCreditos1.SetActive(false);
        }

        if (painelCreditos2 != null)
        {
            painelCreditos2.SetActive(true);
        }
    }
    public void VoltarAbrirCreditos()
    {
        if (painelCreditos2 != null)
        {
            painelCreditos2.SetActive(false);
        }

        if (painelCreditos1 != null)
        {
            painelCreditos1.SetActive(true);
        }
    }
    public void VoltarCreditosMenu()
    {
        if (painelCreditos1 != null)
        {
            painelCreditos1.SetActive(false);
        }

        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(true);
        }
    }
    public void VoltarMenuNome()
    {
        if (MenuNome != null)
        {
            MenuNome.SetActive(false);
        }

        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(true);
        }
    }
    public void IrParaHistorias1()
    {
        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(false);
        }

        if (painelHistorias1 != null)
        {
            painelHistorias1.SetActive(true);
        }
    }
    public void IrParaHistorias2()
    {
        if (painelHistorias1 != null)
        {
            painelHistorias1.SetActive(false);
        }

        if (painelHistorias2 != null)
        {
            painelHistorias2.SetActive(true);
        }
    }
    public void VoltarParaHistorias1()
    {
        if (painelHistorias2 != null)
        {
            painelHistorias2.SetActive(false);
        }

        if (painelHistorias1 != null)
        {
            painelHistorias1.SetActive(true);
        }
    }
    public void VoltarParaMenuHistoria()
    {
        if (painelHistorias1 != null)
        {
            painelHistorias1.SetActive(false);
        }

        if (MenuOpcoes != null)
        {
            MenuOpcoes.SetActive(true);
        }
    }
}


