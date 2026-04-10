using UnityEngine;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    [Header("Configurao")]
    //Ponto onde a bola ser reposicionada
    [SerializeField] private Transform SpawnPoint;
    //Tempo de espera antes do respawn
    [SerializeField] private float respawnDelay = 1f;

    [Header("Referencias")]
    // Refer�ncia ao GameManager para controlar vidas
    [SerializeField] private GameManager GameManager;

    private void OnTriggerEnter(Collider other)
    {
        // Garante que apenas a bola ativa o sistema
        if (!other.CompareTag("Ball")) return;
        // Se o jogo acabou, no faz nada
        if (GameManager.JogoAcabou()) return;
        // Informa ao GameManager que o jogador perdeu uma vida
        GameManager.PerderVida();
        // Inicia o respawn da bola
        StartCoroutine(Respawn(other));
    }

    //Para a bola, cria um tempo de espera antes de reposicionar para evitar problemas de fsica ou colisoes indesejadas.
    private IEnumerator Respawn(Collider ball)
    {
        Rigidbody rb = ball.attachedRigidbody;
        // Segurança: evita erro caso exista Rigidbody
        if (rb == null) yield break;

        // Zera movimento antes do respawn
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Espera antes de reposicionar
        yield return new WaitForSeconds(respawnDelay);
        // Move a bola para o SpawnPoint
        rb.position = SpawnPoint.position;
    }
}

