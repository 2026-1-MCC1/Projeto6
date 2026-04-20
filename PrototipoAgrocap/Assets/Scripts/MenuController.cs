using UnityEngine;

public class MenuController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menuOpcoes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     if (videoPlayer.isPlaying && Input.anyKeyDown)
        {
            videoPlayer.Stop();
            menuOpcoes.SetActive(true);
        }    
    }
}
