using System.Collections;
using UnityEngine;

// Controla o trigger do power up da mesa.
// Quando a bolinha principal entra nele, ela recebe um impulso de volta para a mesa.
// Depois de um pequeno atraso, uma bolinha extra preta aparece para ajudar na coleta.
public class PowerUpLauncher : MonoBehaviour
{
    [Header("Impulso da bolinha principal")]
    [SerializeField] private Transform tableReturnTarget;
    [SerializeField] private float launchForce = 18f;
    [SerializeField] private float upwardBlend = 0.25f;

    [Header("Bolinha extra")]
    [SerializeField] private GameObject secondaryBallPrefab;
    [SerializeField] private Transform secondaryBallSpawnPoint;
    [SerializeField] private float spawnDelay = 2f;

    private void OnTriggerEnter(Collider other)
    {
        // Mantem a coleta funcionando com a mesma tag da bola original.
        if (!other.CompareTag("Ball")) return;

        // A bolinha extra tambem tem tag Ball, entao ela precisa ser ignorada aqui.
        if (other.GetComponentInParent<PowerUpBall>() != null) return;

        Rigidbody ballRigidbody = other.attachedRigidbody;
        if (ballRigidbody == null) return;

        // Cada nova entrada da bolinha principal no trigger ativa o power up de novo.
        LaunchMainBallBackToTable(ballRigidbody);
        StartCoroutine(SpawnSecondaryBallAfterDelay());
    }

    private void LaunchMainBallBackToTable(Rigidbody ballRigidbody)
    {
        // Usa um alvo configurado na cena para indicar para onde a bola deve voltar.
        Vector3 targetPosition = tableReturnTarget != null
            ? tableReturnTarget.position
            : transform.position + transform.forward;

        // Mistura um pouco de forca para cima para a bola nao raspar no trigger ao sair.
        Vector3 launchDirection = (targetPosition - ballRigidbody.position).normalized;
        launchDirection = (launchDirection + Vector3.up * upwardBlend).normalized;

        // Zera a velocidade antiga para o power up sempre dar um impulso previsivel.
        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.AddForce(launchDirection * launchForce, ForceMode.Impulse);
    }

    private IEnumerator SpawnSecondaryBallAfterDelay()
    {
        // Espera o tempo pedido antes de colocar a bolinha preta na mesa.
        yield return new WaitForSeconds(spawnDelay);

        if (secondaryBallPrefab == null || secondaryBallSpawnPoint == null)
        {
            Debug.LogError("Power up sem prefab ou ponto de spawn da bolinha preta.");
            yield break;
        }

        // A bolinha preta ja vem configurada no prefab com material preto, tag Ball e marcador PowerUpBall.
        Instantiate(
            secondaryBallPrefab,
            secondaryBallSpawnPoint.position,
            secondaryBallSpawnPoint.rotation);
    }
}
