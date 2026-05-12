using UnityEngine;

// Controla a barreira do lançador.
// Quando a bola está no Plunger, a passagem fica aberta.
// Quando a bola sai e passa pelo trigger, a passagem fecha.
public class Bloqueador : MonoBehaviour
{
    // Collider da barreira física.
    public Collider barreiraCollider;

    // Trigger que detecta quando a bola saiu do lançador.
    public Collider triggerSaida;

    // Guarda se a bola principal ja entrou no sensor antes de sair dele totalmente.
    private bool bolaEntrouNoSensor = false;

    void Start()
    {
        LiberarPassagem();
    }

    // Libera a passagem para permitir que a bola seja lançada.
    public void LiberarPassagem()
    { 
        // Desativa a barreira física, permitindo que a bola passe.
        barreiraCollider.enabled = false;

        // Ativa o trigger de saída, para detectar quando a bola sair.
        triggerSaida.enabled = true;
    }

    // Fecha a passagem depois que a bola já saiu.
    public void FecharPassagem()
    {
        // Ativa a barreira física, impedindo que a bola volte.
        barreiraCollider.enabled = true;

        // Desativa o trigger, pois não precisa detectar e não queremos que atravesse a parede.
        triggerSaida.enabled = false;
    }

    // Detecta quando algum objeto entra no trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto que entrou foi a bola.
        if (other.CompareTag("Ball"))
        {
            // Se a bola passou pelo trigger, fechamos a passagem.
            FecharPassagem();
        }
    }
}
