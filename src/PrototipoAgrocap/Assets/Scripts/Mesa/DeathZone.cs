using UnityEngine;
using System.Collections;

// Detecta quando a bola cai da mesa e coordena perda de vida com respawn.
public class DeathZone : MonoBehaviour
{
    [Header("Configuração")]
    //Ponto onde a bola será reposicionada
    [SerializeField] private Transform SpawnPoint;
    //Tempo de espera antes do respawn
    [SerializeField] private float respawnDelay = 1f;

    [Header("Referências")]
    // Referência ao GameManager para controlar vidas
    [SerializeField] private GameManager GameManager;
    // Referencia ao bloqueador da saida para reabrir o corredor no respawn
    [SerializeField] private Bloqueador bloqueadorSaida;

    private void OnTriggerEnter(Collider other)
    {
        // Garante que apenas a bola ativa o sistema
        if (!other.CompareTag("Ball")) return;
        // Bolinhas extras do power up continuam coletando itens, mas nao custam vida ao cair.
        if (other.GetComponentInParent<PowerUpBall>() != null)
        {
            Destroy(other.attachedRigidbody != null ? other.attachedRigidbody.gameObject : other.gameObject);
            return;
        }
        // Se o jogo já acabou, não faz nada
        if (GameManager.JogoAcabou()) return;
        // Informa ao GameManager que o jogador perdeu uma vida
        GameManager.PerderVida();
        // Inicia o respawn da bola
        StartCoroutine(Respawn(other));
    }

    //Para a bola, cria um tempo de espera antes de reposicionar para evitar problemas de física ou colisões indesejadas.
    private IEnumerator Respawn(Collider ball)
    {
        Rigidbody rb = ball.attachedRigidbody;
        // Segurança: evita erro caso não exista Rigidbody
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

