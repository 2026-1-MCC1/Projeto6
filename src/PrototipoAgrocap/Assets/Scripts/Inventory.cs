using TMPro;
using UnityEngine;

// Gerencia o inventario de ingredientes do jogador.
public class Inventory : MonoBehaviour
{
    [Header("Ingredientes")]
    public int Trigo = 0;
    public int Ovo = 0;
    public int Leite = 0;
    public int Chocolate = 0;
    public int Morango = 0;
    public GameObject[] Luzes;

    private GameObject LuzAtual;

    [Header("UI")]
    [SerializeField] private ScoreManager scoreManager;

    [System.Serializable]
    private class ItemHUD
    {
        public TextMeshProUGUI quantidade = null;
    }

    [Header("UI Itens")]
    [SerializeField] private ItemHUD trigoHUD;
    [SerializeField] private ItemHUD ovoHUD;
    [SerializeField] private ItemHUD leiteHUD;
    [SerializeField] private ItemHUD chocolateHUD;
    [SerializeField] private ItemHUD morangoHUD;

    private void Awake()
    {
        if (scoreManager == null)
        {
            scoreManager = FindAnyObjectByType<ScoreManager>();
        }
    }

    private void Start()
    {
        AtualizarTextoItens();
    }

    public void AdicionarIngrediente(IngredienteTipo tipo)
    {
        int indiceLuz = -1;

        switch (tipo)
        {
            case IngredienteTipo.Trigo:
                Trigo++;
                indiceLuz = 0;
                break;

            case IngredienteTipo.Ovo:
                Ovo++;
                indiceLuz = 1;
                break;

            case IngredienteTipo.Leite:
                Leite++;
                indiceLuz = 2;
                break;

            case IngredienteTipo.Chocolate:
                Chocolate++;
                indiceLuz = 3;
                break;

            case IngredienteTipo.Morango:
                Morango++;
                indiceLuz = 4;
                break;
        }

        if (LuzAtual != null)
        {
            LuzAtual.SetActive(false);
        }

        if (indiceLuz != -1 && indiceLuz < Luzes.Length)
        {
            Luzes[indiceLuz].SetActive(true);
            LuzAtual = Luzes[indiceLuz];
        }

        Debug.Log($"Coletado: {tipo}");

        AtualizarTextoItens();

        if (scoreManager != null)
        {
            scoreManager.AdicionarPontos(tipo);
        }
    }

    private void AtualizarTextoItens()
    {
        AtualizarItem(trigoHUD, Trigo);
        AtualizarItem(ovoHUD, Ovo);
        AtualizarItem(leiteHUD, Leite);
        AtualizarItem(chocolateHUD, Chocolate);
        AtualizarItem(morangoHUD, Morango);
    }

    private void AtualizarItem(ItemHUD item, int quantidade)
    {
        if (item != null && item.quantidade != null)
        {
            item.quantidade.text = quantidade.ToString();
        }
    }
}
