using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class RankingManager : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject canvasScoreboard;
    [SerializeField] private GameObject canvasRanking;

    [Header("Integracao com Firebase")]
    [FormerlySerializedAs("rankingAPI")]
    [SerializeField] private FirebaseRanking firebaseRanking;

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

    private TextMeshProUGUI[] textosNomes;
    private TextMeshProUGUI[] textosPontos;
    private bool carregandoRanking;

    private void Awake()
    {
        if (firebaseRanking == null)
        {
            firebaseRanking = FindFirstObjectByType<FirebaseRanking>();
        }
    }

    private void Start()
    {
        canvasScoreboard.SetActive(true);
        canvasRanking.SetActive(false);

        textosNomes = new[]
        {
            textoNome1, textoNome2, textoNome3,
            textoNome4, textoNome5, textoNome6
        };

        textosPontos = new[]
        {
            textoPontos1, textoPontos2, textoPontos3,
            textoPontos4, textoPontos5, textoPontos6
        };

        LimparRanking();
    }

    public void AbrirRanking()
    {
        if (carregandoRanking)
        {
            return;
        }

        if (firebaseRanking == null)
        {
            Debug.LogError("RankingManager: FirebaseRanking nao foi encontrado na cena.");
            return;
        }

        canvasScoreboard.SetActive(false);
        canvasRanking.SetActive(true);
        carregandoRanking = true;

        firebaseRanking.SalvarRanking(salvou =>
        {
            if (!salvou)
            {
                Debug.LogWarning("RankingManager: a partida atual nao foi enviada. O ranking exibido pode nao incluir a ultima pontuacao.");
            }

            firebaseRanking.BuscarRanking(ranking =>
            {
                MostrarRanking(ranking);
                carregandoRanking = false;
            });
        });
    }

    public void VoltarScoreboard()
    {
        canvasRanking.SetActive(false);
        canvasScoreboard.SetActive(true);
    }

    private void MostrarRanking(List<RankingFirebaseEntry> ranking)
    {
        if (ranking == null)
        {
            LimparRanking();
            return;
        }

        int index = 0;
        foreach (RankingFirebaseEntry entrada in ranking)
        {
            if (index >= textosNomes.Length)
            {
                break;
            }

            textosNomes[index].text = (index + 1) + "o  " + entrada.Nome;
            textosPontos[index].text = entrada.Pontos.ToString();
            index++;
        }

        for (int i = index; i < textosNomes.Length; i++)
        {
            textosNomes[i].text = (i + 1) + "o  ---";
            textosPontos[i].text = "---";
        }
    }

    private void LimparRanking()
    {
        for (int i = 0; i < textosNomes.Length; i++)
        {
            textosNomes[i].text = (i + 1) + "o  ---";
            textosPontos[i].text = "---";
        }
    }
}
