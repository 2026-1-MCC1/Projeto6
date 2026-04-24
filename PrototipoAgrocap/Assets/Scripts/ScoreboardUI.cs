using TMPro;
using UnityEngine;

// Responsável por exibir os resultados finais na tela de Game Over / Scoreboard
public class ScoreboardUI : MonoBehaviour
{
    [Header("Referência UI")]

    // Texto onde todas as informações serão exibidas
    [SerializeField] private TextMeshProUGUI textoResultado;

    // Executado ao iniciar a cena de Scoreboard
    void Start()
    {
        AtualizarTela();
    }

    // Atualiza o texto da UI com os dados do GameResults
    private void AtualizarTela()
    {
        if (textoResultado == null)
        {
            Debug.LogError("Texto do Scoreboard NÃO está conectado!");
            return;
        }

        textoResultado.text =
            "=== RESULTADO FINAL ===\n\n" +

        "Pontuação: " + GameResults.ScoreFinal + "\n\n" +

        "Bolos Criados:\n" +
        "- Especial: " + GameResults.BoloEspecial + "\n" +
        "- Chocolate: " + GameResults.BoloChocolate + "\n" +
        "- Morango: " + GameResults.BoloMorango + "\n" +
        "- Simples: " + GameResults.BoloSimples + "\n\n" +
        "Ingredientes restantes:\n" +
        "- Trigo: " + GameResults.TrigoRestante + "\n" +
        "- Ovo: " + GameResults.OvoRestante + "\n" +
        "- Leite: " + GameResults.LeiteRestante + "\n" +
        "- Chocolate: " + GameResults.ChocolateRestante + "\n" +
        "- Morango: " + GameResults.MorangoRestante;
    }
}