using UnityEngine;

[System.Serializable]
public class Gif
{
    public Texture2D[] frames;
}

public class AnimarGif : MonoBehaviour
{
    public Gif[] gifs;
    public int gifAtual = 0;

    public float framesPorSegundo = 10f;

    private Renderer renderizador;

    void Start()
    {
        renderizador = GetComponent<Renderer>();
    }

    void Update()
    {
        // segurança
        if (gifs == null || gifs.Length == 0) return;
        if (gifAtual < 0 || gifAtual >= gifs.Length) return;

        Texture2D[] frames = gifs[gifAtual].frames;

        if (frames == null || frames.Length == 0) return;

        int index = (int)(Time.time * framesPorSegundo) % frames.Length;
        renderizador.material.mainTexture = frames[index];
    }

    public void TrocarGif(int index)
    {
        if (gifs == null || gifs.Length == 0) return;

        // evita erro de índice errado
        if (index < 0 || index >= gifs.Length)
        {
            Debug.LogWarning("Índice de GIF inválido: " + index);
            return;
        }

        gifAtual = index;
    }
}