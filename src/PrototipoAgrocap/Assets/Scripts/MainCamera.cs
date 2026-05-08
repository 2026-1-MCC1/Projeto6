using UnityEngine;

// Controla troca de câmeras no pinball
// Mantem a transicao suave entre os pontos de vista da mesa.
public class CameraPinball : MonoBehaviour
{
    [Header("Posições da câmera")]
    // Array com posições pré-definidas
    public Transform[] cameraPositions;

    [Header("Configuração")]
    // Velocidade de transição
    public float smoothSpeed = 5f;

    // Índice da câmera atual
    private int currentIndex = 0;

    [Header("Referências")]
    // Objeto que a câmera vai usar como ponto de refencia (para onde ela vai olhar)
    public Transform ReferencePoint;

    void Update()
    {
        // Troca de câmera ao apertar C
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
        // Garante que o índice não ultrapasse o tamanho do array
        if (currentIndex >= cameraPositions.Length)
            currentIndex = 0;
        // Define o alvo atual da câmera (posição desejada)
        Transform target = cameraPositions[currentIndex];
        // Move a câmera suavemente até a posição do alvo
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            smoothSpeed * Time.deltaTime
        );
        // Move a câmera suavemente até a posição do alvo
        if (ReferencePoint != null)
            transform.LookAt(ReferencePoint);
    }
}
