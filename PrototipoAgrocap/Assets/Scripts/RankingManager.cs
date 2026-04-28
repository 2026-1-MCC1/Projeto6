using TMPro;
using UnityEngine;

// Controla a tela de Ranking
// Alterna entre o Canvas do Scoreboard e o Canvas do Ranking e preenche os textos com posição, nome e pontuação.
public class RankingManager : MonoBehaviour
{
    [Header("Canvas")]

    // Canvas principal do Scoreboard
    [SerializeField] private GameObject canvasScoreboard;

    // Canvas da tela de Ranking
    [SerializeField] private GameObject canvasRanking;


    [Header("Ranking - Linha 1")]
    // Texto da esquerda (posição + nome)
    [SerializeField] private TextMeshProUGUI textoNome1;
    // Texto da direita (pontuação)
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


    [Header("Jogador")]

    // Nome do jogador principal
    // Pode ser alterado no Inspector futuramente
    [SerializeField] private string nomeJogador = "Otavio";


    // Quando a cena começa mostra o Scoreboard e deixa o Ranking escondido
    private void Start()
    {
        canvasScoreboard.SetActive(true);
        canvasRanking.SetActive(false);
    }

    // Chamado no botão "Ranking". Preenche os dados e troca de tela
    public void AbrirRanking()
    {
        // Preenche os textos do ranking
        MostrarRanking();

        // Esconde o Scoreboard
        canvasScoreboard.SetActive(false);

        // Mostra o Ranking
        canvasRanking.SetActive(true);
    }

    // Botão de voltar para o Scoreboard
    public void VoltarScoreboard()
    {
        // Esconde o Ranking
        canvasRanking.SetActive(false);

        // Mostra novamente o Scoreboard
        canvasScoreboard.SetActive(true);
    }

    // Preenche os textos do ranking
    // Aqui estamos usando um ranking fixo/local
    // com o jogador em primeiro lugar
    private void MostrarRanking()
    {
        // Pega a pontuação final calculada no GameResults
        int pontos = GameResults.ScoreFinal;

        // Linha 1 
        textoNome1.text = "1º  " + nomeJogador;
        textoPontos1.text = pontos.ToString();

        // Linha 2
        textoNome2.text = "2º  Julia";
        textoPontos2.text = "2800";

        // Linha 3
        textoNome3.text = "3º  Kaua";
        textoPontos3.text = "2400";

        // Linha 4
        textoNome4.text = "4º  Flavio";
        textoPontos4.text = "1900";

        // Linha 5
        textoNome5.text = "5º  Flavio";
        textoPontos5.text = "1500";

        // Linha 6
        textoNome6.text = "6º  Kaua";
        textoPontos6.text = "1200";
    }
}