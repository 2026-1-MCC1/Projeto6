using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Monta dinamicamente os cards da tela de resultados.
public class ScoreboardUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private RectTransform panelScoreboard;

    [Header("Fonte")]
    [SerializeField] private TMP_FontAsset fontePersonalizada;

    [Header("Sprites dos Cards")]
    [SerializeField] private Sprite spriteBoloEspecial;
    [SerializeField] private Sprite spriteBoloChocolate;
    [SerializeField] private Sprite spriteBoloMorango;
    [SerializeField] private Sprite spriteBoloSimples;
    [SerializeField] private Sprite spriteChocolate;
    [SerializeField] private Sprite spriteMorango;
    [SerializeField] private Sprite spriteFarinha;
    [SerializeField] private Sprite spriteLeite;
    [SerializeField] private Sprite spriteOvo;

    [Header("Visual")]
    [SerializeField] private Color cardColor = Color.white;

    private GridLayoutGroup gridCardsPequenos;
    private RectTransform areaCardsPequenos;

    private void Start()
    {
        // Usa o ultimo snapshot salvo ao fim da partida para reconstruir a tela.
        GameResults.CarregarResultados();
        MontarLayout();
    }

    private void MontarLayout()
    {
        // Limpa o painel antes de recriar os cards para evitar duplicidade ao reabrir a tela.
        foreach (Transform child in panelScoreboard)
        {
            Destroy(child.gameObject);
        }

        CriarCardGrande("Bolo Especial", GameResults.BoloEspecial + "x", spriteBoloEspecial);

        RectTransform areaDireita = CriarAreaDireita();

        CriarCardPequeno(areaDireita, "Bolo Chocolate", GameResults.BoloChocolate + "x", spriteBoloChocolate);
        CriarCardPequeno(areaDireita, "Bolo Morango", GameResults.BoloMorango + "x", spriteBoloMorango);
        CriarCardPequeno(areaDireita, "Bolo Simples", GameResults.BoloSimples + "x", spriteBoloSimples);
        CriarCardPequeno(areaDireita, "Chocolate", GameResults.ChocolateRestante + "x", spriteChocolate);
        CriarCardPequeno(areaDireita, "Morango", GameResults.MorangoRestante + "x", spriteMorango);
        CriarCardPequeno(areaDireita, "Farinha", GameResults.TrigoRestante + "x", spriteFarinha);
        CriarCardPequeno(areaDireita, "Leite", GameResults.LeiteRestante + "x", spriteLeite);
        CriarCardPequeno(areaDireita, "Ovo", GameResults.OvoRestante + "x", spriteOvo);

        ConfigurarGridResponsivo();
    }

    private void CriarCardGrande(string nome, string quantidade, Sprite spriteDoCard)
    {
        GameObject card = new GameObject("Card_" + nome);
        card.transform.SetParent(panelScoreboard, false);

        RectTransform rect = card.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0f, 0f);
        rect.anchorMax = new Vector2(0.32f, 1f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image img = card.AddComponent<Image>();
        img.color = cardColor;

        if (spriteDoCard != null)
        {
            img.sprite = spriteDoCard;
        }

        CriarTextoPosicionado(card.transform, quantidade, "Quantidade", new Vector2(0.23f, 0.55f), new Vector2(90f, 40f), 28);
    }

    private RectTransform CriarAreaDireita()
    {
        GameObject area = new GameObject("AreaCardsPequenos");
        area.transform.SetParent(panelScoreboard, false);

        RectTransform rect = area.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.32f, 0f);
        rect.anchorMax = new Vector2(1f, 1f);
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        gridCardsPequenos = area.AddComponent<GridLayoutGroup>();
        areaCardsPequenos = rect;

        return rect;
    }

    private void ConfigurarGridResponsivo()
    {
        if (gridCardsPequenos == null || areaCardsPequenos == null)
        {
            return;
        }

        // Divide a area direita em quatro colunas por duas linhas para acomodar os oito cards.
        const int colunas = 4;
        const int linhas = 2;

        gridCardsPequenos.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridCardsPequenos.constraintCount = colunas;
        gridCardsPequenos.spacing = Vector2.zero;
        gridCardsPequenos.padding = new RectOffset(0, 0, 0, 0);

        Canvas.ForceUpdateCanvases();

        float largura = areaCardsPequenos.rect.width / colunas;
        float altura = areaCardsPequenos.rect.height / linhas;
        gridCardsPequenos.cellSize = new Vector2(largura, altura);
    }

    private void OnRectTransformDimensionsChange()
    {
        ConfigurarGridResponsivo();
    }

    private void CriarCardPequeno(RectTransform parent, string nome, string quantidade, Sprite spriteDoCard)
    {
        GameObject card = new GameObject("Card_" + nome);
        card.transform.SetParent(parent, false);

        Image img = card.AddComponent<Image>();
        img.color = cardColor;

        if (spriteDoCard != null)
        {
            img.sprite = spriteDoCard;
        }

        CriarTextoPosicionado(card.transform, quantidade, "Quantidade", new Vector2(0.23f, 0.55f), new Vector2(70f, 30f), 16);
    }

    private void CriarTextoPosicionado(Transform parent, string conteudo, string nomeObj, Vector2 anchor, Vector2 tamanho, int tamanhoFonte)
    {
        // Centraliza o texto dentro do card e deixa o auto-size ajustar diferentes quantidades.
        GameObject obj = new GameObject("Text_" + nomeObj);
        obj.transform.SetParent(parent, false);

        TextMeshProUGUI texto = obj.AddComponent<TextMeshProUGUI>();
        texto.text = conteudo;
        texto.fontStyle = FontStyles.Bold;
        texto.alignment = TextAlignmentOptions.Center;
        texto.color = Color.black;

        if (fontePersonalizada != null)
        {
            texto.font = fontePersonalizada;
        }

        texto.enableAutoSizing = true;
        texto.fontSizeMin = 6;
        texto.fontSizeMax = tamanhoFonte;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.anchorMin = anchor;
        rect.anchorMax = anchor;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = tamanho;
        rect.anchoredPosition = Vector2.zero;
    }
}
