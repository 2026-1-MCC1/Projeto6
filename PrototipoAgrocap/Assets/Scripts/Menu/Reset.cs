using UnityEngine;
using UnityEngine.SceneManagement;  //Nescessario para gerenciar as cenas

public class ReseT : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Recarrega a cena atual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
