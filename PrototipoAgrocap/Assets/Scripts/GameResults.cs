using UnityEngine;

// Armazena os resultados finais do jogo para serem acessados entre cenas
public class GameResults : MonoBehaviour
{
    // Prefixo usado para salvar os resultados no PlayerPrefs
    private const string Prefixo = "GameResults_";

    // Chave antiga mantida para nao perder partida pendente de versoes anteriores
    private const string ChaveRankingPendente = Prefixo + "RankingPendente";

    // Chave que guarda o numero da ultima partida salva
    private const string ChaveUltimaPartidaSalva = Prefixo + "UltimaPartidaSalva";

    // Chave que guarda o numero da ultima partida enviada ao banco
    private const string ChaveUltimaPartidaEnviada = Prefixo + "UltimaPartidaEnviada";

    // Numero da partida atual
    public static int PartidaAtual;

    // Pontuação total final
    public static int ScoreFinal;

    // Quantidade de cada tipo de bolo produzido
    public static int BoloEspecial;
    public static int BoloChocolate;
    public static int BoloMorango;
    public static int BoloSimples;

    // Quantidade de ingredientes coletados
    public static int Trigo;
    public static int Ovo;
    public static int Leite;
    public static int Chocolate;
    public static int Morango;

    // Quantidade de ingredientes restantes após as receitas
    public static int TrigoRestante;
    public static int OvoRestante;
    public static int LeiteRestante;
    public static int ChocolateRestante;
    public static int MorangoRestante;


    // Prepara os valores em memoria para um novo jogador
    public static void PrepararNovaPartida()
    {
        PartidaAtual = 0;
        ScoreFinal = 0;

        BoloEspecial = 0;
        BoloChocolate = 0;
        BoloMorango = 0;
        BoloSimples = 0;

        Trigo = 0;
        Ovo = 0;
        Leite = 0;
        Chocolate = 0;
        Morango = 0;

        TrigoRestante = 0;
        OvoRestante = 0;
        LeiteRestante = 0;
        ChocolateRestante = 0;
        MorangoRestante = 0;
    }


    // Salva os resultados para nao sumirem quando o jogo for parado
    public static void SalvarResultados()
    {
        // Cada resultado final vira uma nova partida
        PartidaAtual = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, 0) + 1;
        PlayerPrefs.SetInt(ChaveUltimaPartidaSalva, PartidaAtual);

        PlayerPrefs.SetInt(Prefixo + "ScoreFinal", ScoreFinal);

        PlayerPrefs.SetInt(Prefixo + "BoloEspecial", BoloEspecial);
        PlayerPrefs.SetInt(Prefixo + "BoloChocolate", BoloChocolate);
        PlayerPrefs.SetInt(Prefixo + "BoloMorango", BoloMorango);
        PlayerPrefs.SetInt(Prefixo + "BoloSimples", BoloSimples);

        PlayerPrefs.SetInt(Prefixo + "Trigo", Trigo);
        PlayerPrefs.SetInt(Prefixo + "Ovo", Ovo);
        PlayerPrefs.SetInt(Prefixo + "Leite", Leite);
        PlayerPrefs.SetInt(Prefixo + "Chocolate", Chocolate);
        PlayerPrefs.SetInt(Prefixo + "Morango", Morango);

        PlayerPrefs.SetInt(Prefixo + "TrigoRestante", TrigoRestante);
        PlayerPrefs.SetInt(Prefixo + "OvoRestante", OvoRestante);
        PlayerPrefs.SetInt(Prefixo + "LeiteRestante", LeiteRestante);
        PlayerPrefs.SetInt(Prefixo + "ChocolateRestante", ChocolateRestante);
        PlayerPrefs.SetInt(Prefixo + "MorangoRestante", MorangoRestante);

        // Marca que essa partida ainda precisa entrar no banco
        PlayerPrefs.SetInt(ChaveRankingPendente, 1);

        PlayerPrefs.Save();
    }


    // Carrega os resultados salvos para montar o Scoreboard
    public static void CarregarResultados()
    {
        PartidaAtual = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, PartidaAtual);
        ScoreFinal = PlayerPrefs.GetInt(Prefixo + "ScoreFinal", ScoreFinal);

        BoloEspecial = PlayerPrefs.GetInt(Prefixo + "BoloEspecial", BoloEspecial);
        BoloChocolate = PlayerPrefs.GetInt(Prefixo + "BoloChocolate", BoloChocolate);
        BoloMorango = PlayerPrefs.GetInt(Prefixo + "BoloMorango", BoloMorango);
        BoloSimples = PlayerPrefs.GetInt(Prefixo + "BoloSimples", BoloSimples);

        Trigo = PlayerPrefs.GetInt(Prefixo + "Trigo", Trigo);
        Ovo = PlayerPrefs.GetInt(Prefixo + "Ovo", Ovo);
        Leite = PlayerPrefs.GetInt(Prefixo + "Leite", Leite);
        Chocolate = PlayerPrefs.GetInt(Prefixo + "Chocolate", Chocolate);
        Morango = PlayerPrefs.GetInt(Prefixo + "Morango", Morango);

        TrigoRestante = PlayerPrefs.GetInt(Prefixo + "TrigoRestante", TrigoRestante);
        OvoRestante = PlayerPrefs.GetInt(Prefixo + "OvoRestante", OvoRestante);
        LeiteRestante = PlayerPrefs.GetInt(Prefixo + "LeiteRestante", LeiteRestante);
        ChocolateRestante = PlayerPrefs.GetInt(Prefixo + "ChocolateRestante", ChocolateRestante);
        MorangoRestante = PlayerPrefs.GetInt(Prefixo + "MorangoRestante", MorangoRestante);
    }


    // Verifica se existe uma partida salva esperando envio ao ranking
    public static bool RankingEstaPendente()
    {
        int ultimaPartidaSalva = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, 0);
        int ultimaPartidaEnviada = PlayerPrefs.GetInt(ChaveUltimaPartidaEnviada, 0);

        // A partida e pendente quando a salva for mais nova que a enviada
        return ultimaPartidaSalva > ultimaPartidaEnviada || PlayerPrefs.GetInt(ChaveRankingPendente, 0) == 1;
    }


    // Marca que a partida salva ja foi enviada para o banco
    public static void MarcarRankingComoEnviado()
    {
        int ultimaPartidaSalva = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, PartidaAtual);

        PlayerPrefs.SetInt(ChaveUltimaPartidaEnviada, ultimaPartidaSalva);
        PlayerPrefs.SetInt(ChaveRankingPendente, 0);
        PlayerPrefs.Save();
    }
}