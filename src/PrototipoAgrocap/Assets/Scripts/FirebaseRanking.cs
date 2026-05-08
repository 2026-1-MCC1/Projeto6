using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

// Centraliza o envio e a leitura do ranking online no Firestore.
public class FirebaseRanking : MonoBehaviour
{
    private const string RankingCollectionName = "ranking";
    private const string DefaultPlayerName = "Jogador";
    private const int RankingDisplayLimit = 6;
    private const int RankingQueryLimit = 30;

    private FirebaseFirestore db;

    private void Awake()
    {
        FirebaseBootstrap.EnsureInstance();
    }

    private async void Start()
    {
        bool ready = await FirebaseBootstrap.EnsureInitializedAsyncStatic();
        if (!ready)
        {
            Debug.LogError("FirebaseRanking: nao foi possivel inicializar o Firebase. " + FirebaseBootstrap.StatusMessage);
            return;
        }

        db = FirebaseBootstrap.Firestore;
    }

    public void SalvarRanking(Action<bool> aoFinalizar = null)
    {
        // Mantem a API publica simples para a UI e executa o fluxo assincrono por tras.
        _ = SalvarRankingAsync(aoFinalizar);
    }

    private async Task SalvarRankingAsync(Action<bool> aoFinalizar)
    {
        GameResults.CarregarResultados();

        if (!GameResults.RankingEstaPendente())
        {
            Debug.Log("FirebaseRanking: nao existe partida pendente para envio.");
            aoFinalizar?.Invoke(true);
            return;
        }

        bool ready = await FirebaseBootstrap.EnsureInitializedAsyncStatic();
        if (!ready)
        {
            Debug.LogError("FirebaseRanking: Firebase indisponivel. " + FirebaseBootstrap.StatusMessage);
            aoFinalizar?.Invoke(false);
            return;
        }

        db = FirebaseBootstrap.Firestore;

        try
        {
            string nomeJogador = ObterNomeJogadorAtual();
            string nomeNormalizado = NormalizarNomeJogador(nomeJogador);
            string documentId = CriarRankingDocumentId(nomeJogador);

            // O nome do jogador vira a chave do documento para evitar duplicatas no ranking.
            await db.Collection(RankingCollectionName)
                .Document(documentId)
                .SetAsync(MontarDadosRanking(nomeJogador, nomeNormalizado));

            await RemoverDuplicatasAsync(documentId, nomeJogador, nomeNormalizado);

            GameResults.MarcarRankingComoEnviado();
            Debug.Log("FirebaseRanking: ranking salvo com sucesso.");
            aoFinalizar?.Invoke(true);
        }
        catch (Exception exception)
        {
            Debug.LogError("FirebaseRanking: erro ao salvar ranking.\n" + exception);
            aoFinalizar?.Invoke(false);
        }
    }

    public void BuscarRanking(Action<List<RankingFirebaseEntry>> aoFinalizar)
    {
        // Mantem o acesso ao top 6 encapsulado no mesmo componente do Firebase.
        _ = BuscarRankingAsync(aoFinalizar);
    }

    private async Task BuscarRankingAsync(Action<List<RankingFirebaseEntry>> aoFinalizar)
    {
        bool ready = await FirebaseBootstrap.EnsureInitializedAsyncStatic();
        if (!ready)
        {
            Debug.LogError("FirebaseRanking: nao foi possivel buscar ranking. " + FirebaseBootstrap.StatusMessage);
            aoFinalizar?.Invoke(null);
            return;
        }

        db = FirebaseBootstrap.Firestore;

        try
        {
            // O ranking continua ordenado pela maior pontuacao salva no banco.
            QuerySnapshot querySnapshot = await db.Collection(RankingCollectionName)
                .OrderByDescending("pontos")
                .Limit(RankingQueryLimit)
                .GetSnapshotAsync();

            List<RankingFirebaseEntry> ranking = new List<RankingFirebaseEntry>();
            HashSet<string> nomesJaAdicionados = new HashSet<string>();
            foreach (DocumentSnapshot document in querySnapshot.Documents)
            {
                string nome = document.ContainsField("nome")
                    ? document.GetValue<string>("nome")
                    : "Jogador";

                string nomeNormalizado = document.ContainsField("nomeNormalizado")
                    ? document.GetValue<string>("nomeNormalizado")
                    : NormalizarNomeJogador(nome);

                if (!nomesJaAdicionados.Add(nomeNormalizado))
                {
                    continue;
                }

                int pontos = document.ContainsField("pontos")
                    ? Convert.ToInt32(document.GetValue<long>("pontos"))
                    : 0;

                ranking.Add(new RankingFirebaseEntry(nome, pontos));

                if (ranking.Count >= RankingDisplayLimit)
                {
                    break;
                }
            }

            aoFinalizar?.Invoke(ranking);
        }
        catch (Exception exception)
        {
            Debug.LogError("FirebaseRanking: erro ao buscar ranking.\n" + exception);
            aoFinalizar?.Invoke(null);
        }
    }

    private Dictionary<string, object> MontarDadosRanking(string nomeJogador, string nomeNormalizado)
    {
        return new Dictionary<string, object>
        {
            { "nome", nomeJogador },
            { "nomeNormalizado", nomeNormalizado },
            { "pontos", GameResults.ScoreFinal },
            { "boloEspecial", GameResults.BoloEspecial },
            { "boloChocolate", GameResults.BoloChocolate },
            { "boloMorango", GameResults.BoloMorango },
            { "boloSimples", GameResults.BoloSimples },
            { "trigo", GameResults.Trigo },
            { "ovo", GameResults.Ovo },
            { "leite", GameResults.Leite },
            { "chocolate", GameResults.Chocolate },
            { "morango", GameResults.Morango },
            { "trigoRestante", GameResults.TrigoRestante },
            { "ovoRestante", GameResults.OvoRestante },
            { "leiteRestante", GameResults.LeiteRestante },
            { "chocolateRestante", GameResults.ChocolateRestante },
            { "morangoRestante", GameResults.MorangoRestante },
            { "data", Timestamp.FromDateTime(GameResults.ObterDataPartidaUtc()) }
        };
    }

    private static string ObterNomeJogadorAtual()
    {
        string nomeJogador = string.IsNullOrWhiteSpace(GameResults.NomeJogador)
            ? MenuController.ObterNomeJogador()
            : GameResults.NomeJogador;

        if (string.IsNullOrWhiteSpace(nomeJogador))
        {
            return DefaultPlayerName;
        }

        return nomeJogador.Trim();
    }

    private async Task RemoverDuplicatasAsync(string documentId, string nomeJogador, string nomeNormalizado)
    {
        HashSet<string> documentosRemovidos = new HashSet<string>();
        CollectionReference rankingCollection = db.Collection(RankingCollectionName);

        await RemoverDuplicatasDaConsultaAsync(
            rankingCollection.WhereEqualTo("nome", nomeJogador),
            documentId,
            documentosRemovidos);

        await RemoverDuplicatasDaConsultaAsync(
            rankingCollection.WhereEqualTo("nomeNormalizado", nomeNormalizado),
            documentId,
            documentosRemovidos);
    }

    private static async Task RemoverDuplicatasDaConsultaAsync(
        Query query,
        string documentId,
        HashSet<string> documentosRemovidos)
    {
        QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
        foreach (DocumentSnapshot document in querySnapshot.Documents)
        {
            if (document.Id == documentId || !documentosRemovidos.Add(document.Id))
            {
                continue;
            }

            await document.Reference.DeleteAsync();
        }
    }

    private static string CriarRankingDocumentId(string nomeJogador)
    {
        string nomeNormalizado = NormalizarNomeJogador(nomeJogador).Replace("/", "_");

        if (string.IsNullOrWhiteSpace(nomeNormalizado) || nomeNormalizado == "." || nomeNormalizado == "..")
        {
            return DefaultPlayerName.ToLowerInvariant();
        }

        return nomeNormalizado;
    }

    private static string NormalizarNomeJogador(string nomeJogador)
    {
        if (string.IsNullOrWhiteSpace(nomeJogador))
        {
            return DefaultPlayerName.ToLowerInvariant();
        }

        return nomeJogador.Trim().ToLowerInvariant();
    }
}

public class RankingFirebaseEntry
{
    public RankingFirebaseEntry(string nome, int pontos)
    {
        Nome = nome;
        Pontos = pontos;
    }

    public string Nome { get; }
    public int Pontos { get; }
}
