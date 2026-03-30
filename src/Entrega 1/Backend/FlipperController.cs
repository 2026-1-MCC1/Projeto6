using UnityEngine;

public class FlipperScript : MonoBehaviour
{
    // Referência ao Hinge Joint do flipper (responsável pela rotação)
    private HingeJoint hinge;
    // Motor do Hinge (controla velocidade e força)
    private JointMotor motor;

    // Tecla que ativa o flipper
    public KeyCode key;
    [Header("Configurações do Flipper")]
    // Força aplicada pelo motor
    public float force = 5000f;
    // Velocidade de rotação do flipper
    public float speed = 1000f;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        motor = hinge.motor;
    }

    void Update()
    {
        // Define direção
        float direction = -1f;

        // Se a tecla estiver pressionada
        if (Input.GetKey(key))
        {
            // Move o flipper para cima
            motor.force = force;
            motor.targetVelocity = speed * direction;
        }
        else
        {
            // Retorna o flipper para posição inicial
            motor.force = force;
            motor.targetVelocity = -speed * direction;
        }
        // Aplica o motor atualizado ao Hinge
        hinge.motor = motor;
    }
}
