using UnityEngine;

public class Bolinha : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Parede"))
        {
            Debug.Log("Bateu na parede!");
        }
    }
}