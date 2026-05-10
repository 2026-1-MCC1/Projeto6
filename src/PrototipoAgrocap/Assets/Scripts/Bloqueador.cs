using UnityEngine;

// Fecha a saida do corredor de lancamento depois que a bola entra na mesa.
public class Bloqueador : MonoBehaviour
{
    [Header("Sensor")]
    [SerializeField] private Collider sensorSaida; // Collider usado apenas para detectar quando a bola passou pela saida.

    [Header("Bloqueio")]
    [SerializeField] private Collider paredeFisica; // Collider do objeto SaidaBola, que vira parede solida depois da passagem.

    // Guarda se a bola principal ja entrou no sensor antes de sair dele totalmente.
    private bool bolaEntrouNoSensor = false;

    private void Start()
    {
        // Se o sensor nao foi arrastado no Inspector, usamos o collider do proprio Bloqueador.
        if (sensorSaida == null)
        {
            sensorSaida = GetComponent<Collider>();
        }

        // O sensor precisa ser trigger para detectar a bola sem bater nela.
        if (sensorSaida != null)
        {
            sensorSaida.isTrigger = true;
        }

        // O SaidaBola comeca como trigger para deixar a bola sair livremente do corredor.
        AbrirPassagem();
    }

    private void OnTriggerEnter(Collider other)
    {
        // So a bola principal, marcada com a tag Ball, pode ativar o bloqueio da saida.
        if (!other.CompareTag("Ball"))
        {
            return;
        }

        // A bola entrou no sensor; quando ela sair dele, a passagem sera fechada.
        bolaEntrouNoSensor = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Ignora qualquer objeto que nao seja a bola principal.
        if (!bolaEntrouNoSensor || !other.CompareTag("Ball"))
        {
            return;
        }

        // A bola saiu totalmente do sensor, entao o SaidaBola vira parede fisica.
        FecharPassagem();
    }

    private void FecharPassagem()
    {
        // Trocar isTrigger para false faz o SaidaBola bloquear a volta da bola.
        if (paredeFisica != null)
        {
            paredeFisica.isTrigger = false;
        }
    }

    private void AbrirPassagem()
    {
        // Trocar isTrigger para true deixa a bola atravessar o SaidaBola no lancamento.
        if (paredeFisica != null)
        {
            paredeFisica.isTrigger = true;
        }
    }

    public void ResetarPassagem()
    {
        // Reinicia o estado para permitir outro lancamento depois de perder vida.
        bolaEntrouNoSensor = false;

        // Reabre o SaidaBola para a proxima saida da bola pelo corredor.
        AbrirPassagem();
    }
}
