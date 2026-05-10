using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    // All GIF frames
    public Texture2D[] frames;

    // Frames per second
    public float framesPorSegundo = 10f;

    private Renderer renderizador;

    void Start()
    {
        renderizador = GetComponent<Renderer>();

        if (renderizador == null)
        {
            Debug.LogError("No Renderer found on this object!");
        }
    }

    void Update()
    {
        // Safety checks
        if (renderizador == null) return;
        if (frames == null || frames.Length == 0) return;

        // Calculate current frame
        int currentFrame =
            (int)(Time.time * framesPorSegundo) % frames.Length;

        // Apply texture
        renderizador.material.mainTexture =
            frames[currentFrame];
    }
}