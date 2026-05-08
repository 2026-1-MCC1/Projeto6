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

    // Nome padrao usado quando o jogador nao digita nada
    private const string NomePadrao = "Jogador";

    // Chave usada para guardar o nome entre as cenas
    private const string ChaveNomeJogador = "NomeJogador";

    // Controla se o nome salvo ja foi carregado
    private static bool nomeFoiCarregado = false;

    // Nome do jogador atual
    public static string NomeJogador = NomePadrao;


    // Salva o nome digitado no menu para ser usado no ranking
    public static void SalvarNomeJogador(string nome)
    {
        // Se nao digitar nada, usa nome padrao
        if (string.IsNullOrWhiteSpace(nome))
        {
            nome = NomePadrao;
        }

        // Guarda o nome sem espacos extras
        NomeJogador = nome.Trim();

        // Marca que o nome atual ja foi carregado/salvo
        nomeFoiCarregado = true;

        // Salva o nome para continuar disponivel entre cenas
        PlayerPrefs.SetString(ChaveNomeJogador, NomeJogador);
        PlayerPrefs.Save();
    }


    // Busca o nome atual do jogador
    public static string ObterNomeJogador()
    {
        // Carrega o nome salvo apenas uma vez
        if (!nomeFoiCarregado)
        {
            NomeJogador = PlayerPrefs.GetString(ChaveNomeJogador, NomePadrao);
            nomeFoiCarregado = true;
        }

        // Garante que o ranking nunca receba nome vazio
        if (string.IsNullOrWhiteSpace(NomeJogador))
        {
            NomeJogador = NomePadrao;
        }

        return NomeJogador.Trim();
    }


    // Envia os dados da partida atual para o banco
    public void EnviarRanking(Action<bool> aoFinalizar = null)
    {
        StartCoroutine(EnviarDados(aoFinalizar));
    }


    // Busca o ranking salvo no banco
    // O Action permite devolver os dados para outro script depois que a busca terminar
    public void BuscarRanking(Action<RankingLista> aoFinalizar, Action aoErro = null)
    {
        StartCoroutine(BuscarDados(aoFinalizar, aoErro));
    }


    // Envia os dados da partida para a API usando POST
    private IEnumerator EnviarDados(Action<bool> aoFinalizar)
    {
        // Carrega os resultados salvos antes de enviar ao banco
        GameResults.CarregarResultados();

        // Monta os dados que serão enviados para o banco
        RankingData dados = new RankingData
        {
            nome = ObterNomeJogador(),
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
            GameResults.MarcarRankingComoEnviado();
            aoFinalizar?.Invoke(true);
        }
        else
        {
            Debug.LogError("Erro ao enviar ranking: " + request.error);
            aoFinalizar?.Invoke(false);
        }
    }


    // Busca os dados do ranking na API usando GET
    private IEnumerator BuscarDados(Action<RankingLista> aoFinalizar, Action aoErro)
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
            aoErro?.Invoke();
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