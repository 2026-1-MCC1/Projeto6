using UnityEngine;
using static UnityEditor.ShaderData;

public class Bumper : MonoBehaviour
{
    public float forca = 20f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // Direção do impacto (do bumper para a bola)
                Vector3 direcao = (collision.transform.position - transform.position).normalized;

                // Aplica força
                rb.AddForce(direcao * forca, ForceMode.Impulse);
            }
        }
    }
}

