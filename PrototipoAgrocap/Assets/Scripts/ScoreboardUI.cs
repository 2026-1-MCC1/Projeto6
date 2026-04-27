using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Monta dinamicamente a UI do Scoreboard.
// Decisões importantes:
// Nao usei LayoutGroup nos cards pois precisamos posicionamento visual exato por conta da moldura e texto.
// Usei GridLayoutGroup apenas para distribuicao dos cards pequenos
// Tamanhos sao calculados em tempo real para manter responsividade
public class ScoreboardLayout : MonoBehaviour
{
    [Header("Referências")]
    // Painel onde todo o layout sera criado.(Deve ser um RectTransform dentro do Canvas.)
    [SerializeField] private RectTransform panelScoreboard;

    [Header("Visual")]
    // Sprite usado como fundo dos cards (moldura).
    [SerializeField] private Sprite cardSprite;
    // Cor base do card (se não quiser usar sprite)
    [SerializeField] private Color cardColor = Color.white;

    // Grid responsável por organizar os cards pequenos.
    private GridLayoutGroup gridCardsPequenos;

    // Área (lado direito) onde o grid vive.
    // Usada para calcular tamanho disponível.
    private RectTransform areaCardsPequenos;

    // Para quando entrar na cena montar tudo.
    private void Start()
    {
        MontarLayout();
    }

    // Limpa o painel e recria toda a UI do zero.
    // Isso evita lixo de execuções anteriores.
    private void MontarLayout()
    {
        // Remove qualquer elemento antigo do painel
        foreach (Transform child in panelScoreboard)
        {
            Destroy(child.gameObject);
        }

        // Cria o card do bolo especial (lado esquerdo)
        CriarCardGrande("Bolo Especial", GameResults.BoloEspecial + "x");

        // Cria a area da direita (container do grid)
        RectTransform areaDireita = CriarAreaDireita();

        // Cria todos os cards pequenos (ordem importa visualmente)
        CriarCardPequeno(areaDireita, "Bolo Chocolate", GameResults.BoloChocolate + "x");
        CriarCardPequeno(areaDireita, "Bolo Morango", GameResults.BoloMorango + "x");
        CriarCardPequeno(areaDireita, "Bolo Simples", GameResults.BoloSimples + "x");
        CriarCardPequeno(areaDireita, "Chocolate", GameResults.ChocolateRestante + "x");

        CriarCardPequeno(areaDireita, "Morango", GameResults.MorangoRestante + "x");
        CriarCardPequeno(areaDireita, "Farinha", GameResults.TrigoRestante + "x");
        CriarCardPequeno(areaDireita, "Leite", GameResults.LeiteRestante + "x");
        CriarCardPequeno(areaDireita, "Ovo", GameResults.OvoRestante + "x");

        // Ajusta tamanho dos cards baseado na tela atual
        ConfigurarGridResponsivo();
    }

    // Cria o card grande da esquerda.
    // Ele ocupa 32 porcento da largura da tela.
    private void CriarCardGrande(string nome, string quantidade)
    {
        GameObject card = new GameObject("Card_" + nome);
        card.transform.SetParent(panelScoreboard, false);

        RectTransform rect = card.AddComponent<RectTransform>();

        // Anchors definem a area relativa (0 a 1)
        // Aqui: ocupa do lado esquerdo ate 32 porcento da largura total
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(0.32f, 1f);

        // Offset zero = gruda exatamente nos anchors
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Visual do card
        Image img = card.AddComponent<Image>();
        img.color = cardColor;

        if (cardSprite != null)
            img.sprite = cardSprite;

        // Quantidade de ingredientes
        CriarTextoPosicionado(
            card.transform,
            quantidade,
            "Quantidade",
            new Vector2(0.23f, 0.55f),
            new Vector2(90f, 40f),
            28
        );

        // Nome do ingrediente
        CriarTextoPosicionado(
            card.transform,
            nome,
            "Nome",
            new Vector2(0.5f, 0.48f),
            new Vector2(220f, 45f),
            28
        );
    }

    // Cria a area da direita onde ficam os cards pequenos.
    // Essa area ocupa o restante da tela (68%).
    private RectTransform CriarAreaDireita()
    {
        GameObject area = new GameObject("AreaCardsPequenos");
        area.transform.SetParent(panelScoreboard, false);

        RectTransform rect = area.AddComponent<RectTransform>();

        // Começa onde o card grande termina
        rect.anchorMin = new Vector2(0.32f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        // Grid que organiza automaticamente os filhos
        gridCardsPequenos = area.AddComponent<GridLayoutGroup>();
        areaCardsPequenos = rect;

        return rect;
    }

    // Calcula automaticamente o tamanho dos cards pequenos baseado no tamanho atual da tela.
    private void ConfigurarGridResponsivo()
    {
        if (gridCardsPequenos == null || areaCardsPequenos == null)
            return;

        int colunas = 4;
        int linhas = 2;

        // Define estrutura do grid
        gridCardsPequenos.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridCardsPequenos.constraintCount = colunas;

        // Deixar sem espaçamento entre os cards 
        gridCardsPequenos.spacing = Vector2.zero;
        gridCardsPequenos.padding = new RectOffset(0, 0, 0, 0);

        // Garante que o layout está atualizado antes de medir
        Canvas.ForceUpdateCanvases();

        // Divide o espaço igualmente entre colunas e linhas
        float largura = areaCardsPequenos.rect.width / colunas;
        float altura = areaCardsPequenos.rect.height / linhas;

        gridCardsPequenos.cellSize = new Vector2(largura, altura);
    }

    // Chamado automaticamente quando o tamanho da UI muda
    private void OnRectTransformDimensionsChange()
    {
        ConfigurarGridResponsivo();
    }

    // Cria um card pequeno dentro do grid.
    private void CriarCardPequeno(RectTransform parent, string nome, string quantidade)
    {
        GameObject card = new GameObject("Card_" + nome);
        card.transform.SetParent(parent, false);

        Image img = card.AddComponent<Image>();
        img.color = cardColor;

        if (cardSprite != null)
            img.sprite = cardSprite;

        // Quantidade de ingredientes
        CriarTextoPosicionado(
            card.transform,
            quantidade,
            "Quantidade",
            new Vector2(0.23f, 0.55f),
            new Vector2(70f, 30f),
            16
        );

        // Nome do ingrediente
        CriarTextoPosicionado(
            card.transform,
            nome,
            "Nome",
            new Vector2(0.5f, 0.48f),
            new Vector2(140f, 28f),
            14
        );
    }

    // Cria um texto com posicionamento relativo ao card.
    // IMPORTANTE:
    // anchor (0 a 1) define posição proporcional
    // isso mantém o texto no lugar mesmo com resize
    private void CriarTextoPosicionado(
        Transform parent,
        string conteudo,
        string nomeObj,
        Vector2 anchor,
        Vector2 tamanho,
        int tamanhoFonte)
    {
        GameObject obj = new GameObject("Text_" + nomeObj);
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI texto = obj.AddComponent<TextMeshProUGUI>();

        texto.text = conteudo;
        texto.fontStyle = FontStyles.Bold;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.black;

        // Ajuste automatico da fonte
        texto.enableAutoSizing = true;
        texto.fontSizeMin = 6;
        texto.fontSizeMax = tamanhoFonte;

        RectTransform rect = obj.GetComponent<RectTransform>();

        // Define posicao relativa (0 = esquerda/baixo 1 = direita/cima)
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;

        // Pivot centralizado para evitar deslocamentos estranhos
        rect.pivot = new Vector2(0.5f, 0.5f);

        // Define tamanho da caixa de texto
        rect.sizeDelta = tamanho;

        // Mantém exatamente no ponto do anchor
        rect.anchoredPosition = Vector2.zero;
    }
}