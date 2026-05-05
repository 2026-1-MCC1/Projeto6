using TMPro;
using UnityEngine;

// Controla a tela de Ranking
// Alterna entre Scoreboard e Ranking
// e preenche as posições com dados vindos do banco
public class RankingManager : MonoBehaviour
{
    [Header("Canvas")]

    // Canvas principal do Scoreboard
    [SerializeField] private GameObject canvasScoreboard;

    // Canvas da tela de Ranking
    [SerializeField] private GameObject canvasRanking;


    [Header("Integração com API")]

    // Script responsável por enviar e buscar ranking no banco
    [SerializeField] private RankingAPI rankingAPI;


    [Header("Ranking - Linha 1")]
    [SerializeField] private TextMeshProUGUI textoNome1;
    [SerializeField] private TextMeshProUGUI textoPontos1;


    [Header("Ranking - Linha 2")]
    [SerializeField] private TextMeshProUGUI textoNome2;
    [SerializeField] private TextMeshProUGUI textoPontos2;


    [Header("Ranking - Linha 3")]
    [SerializeField] private TextMeshProUGUI textoNome3;
    [SerializeField] private TextMeshProUGUI textoPontos3;


    [Header("Ranking - Linha 4")]
    [SerializeField] private TextMeshProUGUI textoNome4;
    [SerializeField] private TextMeshProUGUI textoPontos4;


    [Header("Ranking - Linha 5")]
    [SerializeField] private TextMeshProUGUI textoNome5;
    [SerializeField] private TextMeshProUGUI textoPontos5;


    [Header("Ranking - Linha 6")]
    [SerializeField] private TextMeshProUGUI textoNome6;
    [SerializeField] private TextMeshProUGUI textoPontos6;


    // Arrays usados para facilitar o preenchimento das linhas
    private TextMeshProUGUI[] textosNomes;
    private TextMeshProUGUI[] textosPontos;

    // Evita clicar varias vezes enquanto envia/busca o ranking
    private bool carregandoRanking = false;


    // Configuração inicial da tela
    private void Start()
    {
        // Começa mostrando o Scoreboard
        canvasScoreboard.SetActive(true);

        // Esconde o Ranking no início
        canvasRanking.SetActive(false);

        // Junta os textos de nome em um array
        textosNomes = new TextMeshProUGUI[]
        {
            textoNome1, textoNome2, textoNome3,
            textoNome4, textoNome5, textoNome6
        };

        // Junta os textos de pontos em um array
        textosPontos = new TextMeshProUGUI[]
        {
            textoPontos1, textoPontos2, textoPontos3,
            textoPontos4, textoPontos5, textoPontos6
        };

        // Salva a partida no banco assim que o Scoreboard abre
        SalvarRankingPendente();
    }


    // Chamado pelo botão "Ranking"
    public void AbrirRanking()
    {
        // Evita chamadas duplicadas antes da API responder
        if (carregandoRanking)
        {
            return;
        }

        // Verifica se a API foi conectada no Inspector
        if (rankingAPI == null)
        {
            Debug.LogError("RankingAPI não foi conectada.");
            return;
        }

        // Troca a tela visível
        canvasScoreboard.SetActive(false);
        canvasRanking.SetActive(true);

        // Marca que o ranking esta carregando
        carregandoRanking = true;

        // Se a partida ainda nao foi salva, envia antes de buscar a lista
        if (GameResults.RankingEstaPendente())
        {
            rankingAPI.EnviarRanking((enviado) =>
            {
                BuscarRankingAtualizado();
            });
        }
        else
        {
            BuscarRankingAtualizado();
        }
    }


    // Envia a partida para o banco caso ela ainda esteja pendente
    private void SalvarRankingPendente()
    {
        // Verifica se a API foi conectada no Inspector
        if (rankingAPI == null)
        {
            Debug.LogError("RankingAPI não foi conectada.");
            return;
        }

        // Se a partida ja foi enviada, nao envia de novo
        if (!GameResults.RankingEstaPendente())
        {
            return;
        }

        // Evita outro envio enquanto a API responde
        carregandoRanking = true;

        // Envia automaticamente o resultado salvo da partida
        rankingAPI.EnviarRanking((enviado) =>
        {
            // Libera o botao para abrir o ranking depois do envio
            carregandoRanking = false;
        });
    }


    // Busca o ranking atualizado e libera o botao ao terminar
    private void BuscarRankingAtualizado()
    {
        rankingAPI.BuscarRanking(
            (lista) =>
            {
                MostrarRanking(lista);
                carregandoRanking = false;
            },
            () =>
            {
                carregandoRanking = false;
            }
        );
    }


    // Chamado pelo botão "Voltar"
    public void VoltarScoreboard()
    {
        // Esconde o ranking
        canvasRanking.SetActive(false);

        // Mostra novamente o scoreboard
        canvasScoreboard.SetActive(true);
    }


    // Preenche a interface com os dados recebidos do banco
    private void MostrarRanking(RankingLista lista)
    {
        // Percorre todas as linhas disponíveis na UI
        for (int i = 0; i < textosNomes.Length; i++)
        {
            // Se existir jogador nessa posição, mostra os dados reais
            if (i < lista.ranking.Length)
            {
                textosNomes[i].text = (i + 1) + "º  " + lista.ranking[i].nome;
                textosPontos[i].text = lista.ranking[i].pontos.ToString();
            }
            else
            {
                // Se não existir jogador suficiente, mostra linha vazia
                textosNomes[i].text = (i + 1) + "º  ---";
                textosPontos[i].text = "---";
            }
        }
    }
}
