using UnityEngine;

public class Luzes : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    public GameObject[] luzesVermelhas; // As luzes vermelhas vao ser adicionadas nesse local
    public GameObject[] luzesAzuis;     // As luzes azuis vao ser adicionadas nesse local
    public GameObject[] luzesAmarelas;  // As luzes amarelas vao ser adicionadas nesse local
    public GameObject[] luzesMarrons;   // As luzes marrons vao ser adicionadas nesse local
    public GameObject[] luzesBrancas;   // As luzes brancas vao ser adicionadas nesse local

    // Variáveis para rastrear a quantidade anterior e detectar a coleta
    private int qtdMorangoAnt; // Variável para rastrear a quantidade anterior e detectar a coleta
    private int qtdLeiteAnt;   // Variável para rastrear a quantidade anterior e detectar a coleta
    private int qtdTrigoAnt;   // Variável para rastrear a quantidade anterior e detectar a coleta
    private int qtdOvoAnt;   // Variável para rastrear a quantidade anterior e detectar a coleta
    private int qtdChocolateAnt;   // Variável para rastrear a quantidade anterior e detectar a coleta

    void Start()
    {
        // Para comecar com todas as luzes apagadas
        SetLuzes(luzesVermelhas, false);
        SetLuzes(luzesAzuis, false);
        SetLuzes(luzesAmarelas, false);
        SetLuzes(luzesMarrons, false);
        SetLuzes(luzesBrancas, false);

        // inicia com os valores atuais das quantidades
        if (inventory != null)
        {
            qtdMorangoAnt = inventory.Morango;
            qtdLeiteAnt = inventory.Leite;
            qtdTrigoAnt = inventory.Trigo;
            qtdOvoAnt = inventory.Ovo;
            qtdChocolateAnt = inventory.Chocolate;
        }         
    }

    void Update()
    {
        if (inventory == null) return;

        // Note que usei 'inventory.Morango' em vez de 'GetQtdMorango()'
        if (inventory.Morango > qtdMorangoAnt)
        {
            TrocarLuz(luzesVermelhas);
            qtdMorangoAnt = inventory.Morango;
        }

        if (inventory.Leite > qtdLeiteAnt)
        {
            TrocarLuz(luzesAzuis);
            qtdLeiteAnt = inventory.Leite;
        }

        if (inventory.Trigo > qtdTrigoAnt)
        {
            TrocarLuz(luzesAmarelas);
            qtdTrigoAnt = inventory.Trigo;
        }

        if (inventory.Ovo > qtdOvoAnt)
        {
            TrocarLuz(luzesBrancas);
            qtdOvoAnt = inventory.Ovo;
        }

        if (inventory.Chocolate > qtdChocolateAnt)
        {
            TrocarLuz(luzesMarrons);
            qtdChocolateAnt = inventory.Chocolate;
        }
    }

    // Função que apaga tudo e liga apenas o grupo necessário
    void TrocarLuz(GameObject[] grupo)
    {
        ResetarTodas();
        SetLuzes(grupo, true);
    }

    // Desliga todos os objetos de luz
    void ResetarTodas()
    {
        SetLuzes(luzesVermelhas, false); // Desliga as luzes vermelhas
        SetLuzes(luzesAzuis, false);     // Desliga as luzes azuis
        SetLuzes(luzesAmarelas, false);  // Desliga as luzes amarelas
        SetLuzes(luzesMarrons, false);  // Desliga as luzes marrons
        SetLuzes(luzesBrancas, false);  // Desliga as luzes brancas

    }

    // Função auxiliar para ligar/desligar um array de objetos
    // Essa parte do código é o que chamamos de função utilitária. 
    // Ela serve para você não ter que escrever o mesmo comando repetidas vezes para cada uma das 
    // 18 luzes (6 vermelhas, 6 azuis, 6 amarelas).
    void SetLuzes(GameObject[] grupo, bool estado)
    {
        foreach (GameObject luz in grupo)
        {
            if (luz != null) luz.SetActive(estado);
        }
    }
}