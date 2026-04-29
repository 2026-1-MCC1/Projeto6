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
    }


    // Chamado pelo botão "Ranking"
    public void AbrirRanking()
    {
        // Verifica se a API foi conectada no Inspector
        if (rankingAPI == null)
        {
            Debug.LogError("RankingAPI não foi conectada.");
            return;
        }

        // Envia a pontuação atual para o banco
        rankingAPI.EnviarRanking();

        // Busca o ranking atualizado no banco
        rankingAPI.BuscarRanking(MostrarRanking);

        // Troca a tela visível
        canvasScoreboard.SetActive(false);
        canvasRanking.SetActive(true);
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