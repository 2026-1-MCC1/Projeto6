using UnityEngine;

// Aplica um impulso na bola quando ela colide com o bumper da mesa.
public class Bumper : MonoBehaviour
{
    public float power = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Usa a normal da colisao invertida para empurrar a bola para fora do bumper.
            Vector3 dir = collision.contacts[0].normal * -1;
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }
}
