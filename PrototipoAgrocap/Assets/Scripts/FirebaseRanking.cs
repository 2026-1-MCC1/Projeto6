using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Firestore;
using UnityEngine;

public class FirebaseRanking : MonoBehaviour
{
    private const string RankingCollectionName = "ranking";

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
            await db.Collection(RankingCollectionName).AddAsync(MontarDadosRanking());
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
            QuerySnapshot querySnapshot = await db.Collection(RankingCollectionName)
                .OrderByDescending("pontos")
                .Limit(6)
                .GetSnapshotAsync();

            List<RankingFirebaseEntry> ranking = new List<RankingFirebaseEntry>();
            foreach (DocumentSnapshot document in querySnapshot.Documents)
            {
                string nome = document.ContainsField("nome")
                    ? document.GetValue<string>("nome")
                    : "Jogador";

                int pontos = document.ContainsField("pontos")
                    ? Convert.ToInt32(document.GetValue<long>("pontos"))
                    : 0;

                ranking.Add(new RankingFirebaseEntry(nome, pontos));
            }

            aoFinalizar?.Invoke(ranking);
        }
        catch (Exception exception)
        {
            Debug.LogError("FirebaseRanking: erro ao buscar ranking.\n" + exception);
            aoFinalizar?.Invoke(null);
        }
    }

    private Dictionary<string, object> MontarDadosRanking()
    {
        string nomeJogador = string.IsNullOrWhiteSpace(GameResults.NomeJogador)
            ? MenuController.ObterNomeJogador()
            : GameResults.NomeJogador;

        return new Dictionary<string, object>
        {
            { "nome", nomeJogador },
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
