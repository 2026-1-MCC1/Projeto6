using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject MenuOpcoes;
    public RawImage imagemDoVideo;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (videoPlayer.isPlaying && Input.anyKeyDown)
        {
            videoPlayer.Stop();
            if (imagemDoVideo != null) imagemDoVideo.enabled = false; // Desliga o componente de imagem e para o video
            videoPlayer.gameObject.SetActive(false);
            MenuOpcoes.SetActive(true);


        }    
    }
}
