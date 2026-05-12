using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

// Controla o lancamento da bola pelo plunger e o medidor de forca.
public class Plungler : MonoBehaviour
{
    // força maxima da bolinha / medidor de força( O PLUNGER SÓ FUNCIONA COM O POWERSLIDER Q É O MEDIDOR DE FORÇA ).
    float power;
    float minPower = 0f;
    public float maxPower = 100f;
    public Slider powerSlider;
    List<Rigidbody> balllist = new List<Rigidbody>();
    bool ballReady;
    // referência do script do bloqueador.
    public Bloqueador bloqueador;

    void Start()
    {
        // configura o slider.
        powerSlider.minValue = 0f;
        powerSlider.maxValue = maxPower;
    }
    void Update()
    {
        // se a bolinha estiver no hitbox, o slider aparece, se nao tiver deixa escondido.
        if ((ballReady))
        {
            powerSlider.gameObject.SetActive(true);
        }
        else
        {
            powerSlider.gameObject.SetActive(false);
        }
        // slider evolui com a força acumulada.
        powerSlider.value = power;

        // se existir bola dentro da área.
        if (balllist.Count > 0)
        {
            ballReady = true;
            if (Input.GetKey(KeyCode.Space))
            {
                //velocidade do medidor da barrinha encher (é proporcional a força tambem).
                if (power <= maxPower)
                {
                    power += 50 * Time.deltaTime;
                }
            }
            // aplica força na bolinha ao pressionar barra de espaço, direção de impulso com base de onde ta apontado.
            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (Rigidbody r in balllist)
                {
                    r.AddForce(power * transform.forward, ForceMode.Impulse);
                }
            }
        }
        else
        {
            ballReady = false;
            power = 0f;
        }
    }

    // Quando algo entra no trigger do plunger.
    private void OnTriggerEnter(Collider other)
    {
        // verifica se foi a bolinha
        if (other.gameObject.CompareTag("Ball"))
        {
            balllist.Add(other.GetComponent<Rigidbody>());
            //libera a passagem do bloqueador quando a bolinha entra no trigger do plunger.
            bloqueador.LiberarPassagem();
        }
    }

    // Quando algo sai do trigger do plunger.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            balllist.Remove(other.GetComponent<Rigidbody>());
            power = 0f;
        }

    }
}
