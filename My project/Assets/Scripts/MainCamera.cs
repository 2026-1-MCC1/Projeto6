using UnityEngine;

// Controla troca de c�meras no pinball
public class CameraPinball : MonoBehaviour
{
    [Header("Posi��es da c�mera")]
    // Array com posiçoes pra-definidas
    public Transform[] cameraPositions;

    [Header("Configura��o")]
    // Velocidade de transiçao
    public float smoothSpeed = 5f;

    // �ndice da c�mera atual
    private int currentIndex = 0;

    [Header("Refer�ncias")]
    // Objeto que a camera vai usar como ponto de refencia (para onde ela vai olhar)
    public Transform ReferencePoint;

    void Update()
    {
        // Troca de camera ao apertar C
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentIndex++;

            // Volta para 0 se passar do limite
            if (currentIndex >= cameraPositions.Length)
            {
                currentIndex = 0;
            }
        }
    }

    void LateUpdate()
    {
        if (cameraPositions.Length == 0) return;
        // Garante que o indice no ultrapasse o tamanho do array
        if (currentIndex >= cameraPositions.Length)
            currentIndex = 0;
        // Define o alvo atual da camera (posiçao desejada)
        Transform target = cameraPositions[currentIndex];
        // Move a camera suavemente ate a posiçao do alvo
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            smoothSpeed * Time.deltaTime
        );
        // Move a c�mera suavemente at� a posi��o do alvo
        if (ReferencePoint != null)
            transform.LookAt(ReferencePoint);
    }
}
