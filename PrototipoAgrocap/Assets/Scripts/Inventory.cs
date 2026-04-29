using TMPro;
using UnityEngine;

// Gerencia o inventário de ingredientes do jogador
// Armazena os itens coletados e atualiza a interface
public class Inventory : MonoBehaviour
{
    [Header("Ingredientes")]

    // Quantidade de cada ingrediente
    public int Trigo = 0;
    public int Ovo = 0;
    public int Leite = 0;
    public int Chocolate = 0;
    public int Morango = 0;
    public GameObject[] Luzes;
    private GameObject LuzAtual;


    [Header("UI")]

    // Texto que mostra apenas o último item coletado
    [SerializeField] private TextMeshProUGUI textoUltimoItem;

    // Adiciona um ingrediente ao inventário
    public void AdicionarIngrediente(IngredienteTipo tipo)
    {
        int indiceLuz = -1; // -1 pq  é uma convençao de programação  
        // se fosse "0" no lugar de -1 o programa iria entender que 
        // a luz que precisa acender é a 0 (a representação do -1 é vazio ou invalido)

        // Verifica qual ingrediente foi coletado
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

        //Parte para acender e apagar as luzes da mesa conforme a coleta de itens

 
        //Apaga a luz anterior caso tenha uma acessa
        if (LuzAtual != null) LuzAtual.SetActive(false);
        
        //Acende uma nova luz
        if (indiceLuz != -1 && indiceLuz < Luzes.Length)
        {
            Luzes[indiceLuz].SetActive(true);
            LuzAtual = Luzes[indiceLuz];
        }

        // Exibe no console (debug)
        Debug.Log($"Coletado: {tipo}");

        // Atualiza a UI do último item coletado
        AtualizarUltimoItem(tipo);
    }

    // Atualiza o texto exibindo apenas o último item coletado
    private void AtualizarUltimoItem(IngredienteTipo tipo)
    {
        if (textoUltimoItem != null)
        {
            textoUltimoItem.text = "Último item: " + tipo;
        }
        else
        {
            Debug.LogWarning("Texto do último item não está conectado!");
            //Criar uma função para cada item depois para deixar organizado
            //Arreumar o range
        }
    }
}