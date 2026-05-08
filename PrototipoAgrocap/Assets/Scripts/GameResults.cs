using System;
using UnityEngine;

// Guarda os dados finais da partida entre a cena do jogo e a tela de resultados.
public class GameResults : MonoBehaviour
{
    private const string Prefixo = "GameResults_";
    private const string DefaultPlayerName = "Jogador";

    private const string ChaveRankingPendente = Prefixo + "RankingPendente";
    private const string ChaveUltimaPartidaSalva = Prefixo + "UltimaPartidaSalva";
    private const string ChaveUltimaPartidaEnviada = Prefixo + "UltimaPartidaEnviada";

    public static int PartidaAtual;
    public static string NomeJogador = DefaultPlayerName;
    public static long DataPartidaUtcTicks;

    public static int ScoreFinal;

    public static int BoloEspecial;
    public static int BoloChocolate;
    public static int BoloMorango;
    public static int BoloSimples;

    public static int Trigo;
    public static int Ovo;
    public static int Leite;
    public static int Chocolate;
    public static int Morango;

    public static int TrigoRestante;
    public static int OvoRestante;
    public static int LeiteRestante;
    public static int ChocolateRestante;
    public static int MorangoRestante;

    public static void PrepararNovaPartida()
    {
        // Limpa o estado anterior para impedir que uma nova partida herde resultados antigos.
        PartidaAtual = 0;
        NomeJogador = DefaultPlayerName;
        DataPartidaUtcTicks = 0;
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

    public static void DefinirNomeJogador(string nomeJogador)
    {
        if (string.IsNullOrWhiteSpace(nomeJogador))
        {
            NomeJogador = DefaultPlayerName;
            return;
        }

        NomeJogador = nomeJogador.Trim();
    }

    public static DateTime ObterDataPartidaUtc()
    {
        if (DataPartidaUtcTicks <= 0)
        {
            return DateTime.UtcNow;
        }

        return new DateTime(DataPartidaUtcTicks, DateTimeKind.Utc);
    }

    public static void SalvarResultados()
    {
        // Gera um identificador simples para rastrear se a ultima partida ja foi enviada ao ranking.
        PartidaAtual = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, 0) + 1;
        DataPartidaUtcTicks = DateTime.UtcNow.Ticks;

        PlayerPrefs.SetInt(ChaveUltimaPartidaSalva, PartidaAtual);
        PlayerPrefs.SetString(Prefixo + "NomeJogador", NomeJogador);
        PlayerPrefs.SetString(Prefixo + "DataPartidaUtcTicks", DataPartidaUtcTicks.ToString());

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

        PlayerPrefs.SetInt(ChaveRankingPendente, 1);
        PlayerPrefs.Save();
    }

    public static void CarregarResultados()
    {
        // Reconstroi o ultimo snapshot salvo para o Scoreboard e para o envio ao Firebase.
        PartidaAtual = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, PartidaAtual);
        NomeJogador = PlayerPrefs.GetString(Prefixo + "NomeJogador", NomeJogador);
        long.TryParse(PlayerPrefs.GetString(Prefixo + "DataPartidaUtcTicks", DataPartidaUtcTicks.ToString()), out DataPartidaUtcTicks);
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

    public static bool RankingEstaPendente()
    {
        // O ranking fica pendente enquanto a ultima partida salva ainda nao foi confirmada no Firebase.
        int ultimaPartidaSalva = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, 0);
        int ultimaPartidaEnviada = PlayerPrefs.GetInt(ChaveUltimaPartidaEnviada, 0);

        return ultimaPartidaSalva > ultimaPartidaEnviada || PlayerPrefs.GetInt(ChaveRankingPendente, 0) == 1;
    }

    public static void MarcarRankingComoEnviado()
    {
        // Marca a ultima partida persistida como sincronizada para evitar reenvio em loop.
        int ultimaPartidaSalva = PlayerPrefs.GetInt(ChaveUltimaPartidaSalva, PartidaAtual);

        PlayerPrefs.SetInt(ChaveUltimaPartidaEnviada, ultimaPartidaSalva);
        PlayerPrefs.SetInt(ChaveRankingPendente, 0);
        PlayerPrefs.Save();
    }
}
