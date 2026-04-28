using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float power = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 dir = collision.contacts[0].normal * -1;
            rb.AddForce(dir * power, ForceMode.Impulse);
        }
    }
}