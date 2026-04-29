using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

// Responsável pela comunicação entre Unity e API
public class RankingAPI : MonoBehaviour
{
    [Header("Configuração da API")]

    // URL da rota de ranking da API
    [SerializeField] private string url = "http://localhost:3000/ranking";

    // Nome do jogador atual
    // Futuramente esse valor será definido pelo input do menu
    public static string NomeJogador = "Jogador";


    // Envia os dados da partida atual para o banco
    public void EnviarRanking()
    {
        StartCoroutine(EnviarDados());
    }


    // Busca o ranking salvo no banco
    // O Action permite devolver os dados para outro script depois que a busca terminar
    public void BuscarRanking(Action<RankingLista> aoFinalizar)
    {
        StartCoroutine(BuscarDados(aoFinalizar));
    }


    // Envia os dados da partida para a API usando POST
    private IEnumerator EnviarDados()
    {
        // Monta os dados que serão enviados para o banco
        RankingData dados = new RankingData
        {
            nome = NomeJogador,
            pontos = GameResults.ScoreFinal,

            boloEspecial = GameResults.BoloEspecial,
            boloChocolate = GameResults.BoloChocolate,
            boloMorango = GameResults.BoloMorango,
            boloSimples = GameResults.BoloSimples,

            trigoRestante = GameResults.TrigoRestante,
            ovoRestante = GameResults.OvoRestante,
            leiteRestante = GameResults.LeiteRestante,
            chocolateRestante = GameResults.ChocolateRestante,
            morangoRestante = GameResults.MorangoRestante
        };

        // Converte o objeto para JSON
        string json = JsonUtility.ToJson(dados);

        // Converte o JSON para bytes
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        // Cria uma requisição POST para a API
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // Define o corpo da requisição
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);

        // Define onde a resposta será armazenada
        request.downloadHandler = new DownloadHandlerBuffer();

        // Informa que os dados enviados estão em JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Envia a requisição e aguarda a resposta
        yield return request.SendWebRequest();

        // Verifica se o envio deu certo
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Ranking enviado com sucesso!");
        }
        else
        {
            Debug.LogError("Erro ao enviar ranking: " + request.error);
        }
    }


    // Busca os dados do ranking na API usando GET
    private IEnumerator BuscarDados(Action<RankingLista> aoFinalizar)
    {
        // Cria uma requisição GET para buscar o ranking
        UnityWebRequest request = UnityWebRequest.Get(url);

        // Envia a requisição e aguarda a resposta
        yield return request.SendWebRequest();

        // Verifica se a busca deu certo
        if (request.result == UnityWebRequest.Result.Success)
        {
            // A API retorna uma lista JSON pura
            // O JsonUtility precisa de um objeto "pai", então adicionamos {"ranking": ...}
            string json = "{\"ranking\":" + request.downloadHandler.text + "}";

            // Converte o JSON recebido para objeto C#
            RankingLista lista = JsonUtility.FromJson<RankingLista>(json);

            // Devolve a lista para quem chamou o método
            aoFinalizar?.Invoke(lista);
        }
        else
        {
            Debug.LogError("Erro ao buscar ranking: " + request.error);
        }
    }
}


// Dados enviados para o banco
[System.Serializable]
public class RankingData
{
    public string nome;
    public int pontos;

    public int boloEspecial;
    public int boloChocolate;
    public int boloMorango;
    public int boloSimples;

    public int trigoRestante;
    public int ovoRestante;
    public int leiteRestante;
    public int chocolateRestante;
    public int morangoRestante;
}


// Representa uma linha do ranking recebida da API
[System.Serializable]
public class RankingEntry
{
    public string nome;
    public int pontos;
}


// Classe auxiliar para o JsonUtility conseguir ler a lista
[System.Serializable]
public class RankingLista
{
    public RankingEntry[] ranking;
}