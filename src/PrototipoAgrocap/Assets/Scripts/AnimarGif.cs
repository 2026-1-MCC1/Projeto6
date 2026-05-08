using UnityEngine;
// Classe serializável usada para armazenar
// os frames de uma animação estilo GIF.
[System.Serializable]
public class Gif
{
    public Texture2D[] frames;
}

// Anima as telas da mesa trocando os frames das texturas ao longo do tempo.
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

        // Calcula o frame atual com base no tempo para criar a animacao em loop.
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
