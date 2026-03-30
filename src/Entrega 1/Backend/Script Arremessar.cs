using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Plungler : MonoBehaviour
{
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
                if (power <= maxPower)
                {
                    power += 50 * Time.deltaTime;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                foreach (Rigidbody r in balllist)
                {
                    r.AddForce(power * Vector3.forward, ForceMode.Impulse);
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
        if(other.gameObject.CompareTag("Bolinha"))
        {
            balllist.Add(other.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bolinha"))
        {
            balllist.Remove(other.GetComponent<Rigidbody>());
            power = 0f;
        }
        
    }
}