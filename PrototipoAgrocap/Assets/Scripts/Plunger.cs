using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Plungler : MonoBehaviour
{
    // força maxima da bolinha / medidor de força        ( O PLUNGER SÓ FUNCIONA COM O POWERSLIDER Q É O MEDIDOR DE FORÇA )
    float power;
    float minPower = 0f;
    public float maxPower = 100f;
    public Slider powerSlider;
    List<Rigidbody> balllist = new List<Rigidbody>();
    bool ballReady;

    void Start()
    {
        powerSlider.minValue = 0f;
        powerSlider.maxValue = maxPower;
    }
    void Update()
    {
        if ((ballReady))
        {
            powerSlider.gameObject.SetActive(true);
        }
        else
        {
            powerSlider.gameObject.SetActive(false);
        }
        powerSlider.value = power;
        if (balllist.Count > 0)
        {
            ballReady = true;
            if (Input.GetKey(KeyCode.Space))
            {
                //velocidade do medidor da barrinha encher (é proporcional a força tambem)
                if (power <= maxPower)
                {
                    power += 50 * Time.deltaTime;
                }
            }
            // força da bolinha ao pressionar barra de espaço, direção de impulso com base de onde ta apontado
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            balllist.Add(other.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            balllist.Remove(other.GetComponent<Rigidbody>());
            power = 0f;
        }

    }
}
