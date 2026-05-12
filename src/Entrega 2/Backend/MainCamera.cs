using UnityEngine;

// Controle de Cameras Pinball
public class CameraPinball : MonoBehaviour // Define o nome do Script que Controla as cameras
{
    [Header("Posições da camera")]
    // Uma lista conhecida como (array) que guarda os pontos onde a camera pode ficar
    public Transform[] cameraPositions; 

    [Header("Configuração")]
    // Define a velocidade do movimento suave da camera.
    public float smoothSpeed = 5f;

    //Guarda a numeração da camera que esta ativa no momento.
    private int currentIndex = 0;

    [Header("Referencias")]
    //O objeto para onde a camera deve sempre apontar (exemplo: mesa de pinball)
    // Ajustar futuramente possivelmente a nova mesa.
    public Transform ReferencePoint;

    void Update()
    {
        //Ao apertar a tecla "C" verifica se o jogador apertou
        if (Input.GetKeyDown(KeyCode.C))
        { //É o comando que diz va para a proxima posição da lista
            currentIndex++;

            // Observa se chegamos ao fim da lista de cameras
            if (currentIndex >= cameraPositions.Length)
            { // Se chegou ao fim, volta para a primeira camera da lista
                currentIndex = 0;
            }
        }
    }

    void LateUpdate()
    {
        if (cameraPositions.Length == 0) return;
            //Precisa de posições cadastradas dentro dela
            //Se não houver, o script não ira fazer nada para não causar erros

        if (currentIndex >= cameraPositions.Length)
            //Verifica se o numero final passou no final da lista
            currentIndex = 0;
            // Se passou, volta para a primeira camera novamente

             // Para identificar qual é a posição que a camera deve alcançar agora
        Transform target = cameraPositions[currentIndex];

        // Faz o movimento de teletransporte suave entre ambas as cameras
        // A posição atual e a nova posição
        transform.position = Vector3.Lerp(
            transform.position,
            target.position,
            smoothSpeed * Time.deltaTime // Garante que o movimento seja fluido
            //Independente da velocidade
        );
        // Verifica se voce colocou um ponto de referencia para a camera
        if (ReferencePoint != null)
            transform.LookAt(ReferencePoint);
            // Faz a camera girar automaticamente para ficar sempre apontando
            // Para a referencia
            //Ajustar novamente futuramente para se adptar aos novos pontos
    }
}
