using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class MenuController : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public GameObject MenuOpcoes;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!VideoPlayer.isPlaying && Input.anyKeyDown)     //Verifica se o videoplayer esta rolando e para clicar em qualquer tecla
        {
          VideoPlayer.Stop();
            VideoPlayer.gameObject.SetActive(false);

            // Ativa o menu de opções
            if (MenuOpcoes != null)
            {
                MenuOpcoes.SetActive(true);
            }

        }
    }
}
