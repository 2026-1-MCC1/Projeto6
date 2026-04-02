using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPinball : MonoBehaviour
{
    // Array para guardar as 3 posições da câmera
    public Transform[] cameraPositions;

    // Velocidade de transição da câmera (quanto maior, mais rápido)
    public float smoothSpeed = 5f;

    // Índice da câmera atual (começa na 0)
    private int currentIndex = 0;

    public Transform Capsule { get; private set; }

    public Transform bola;
 
    void Update()
    {
        // Verifica se o jogador apertou a tecla C
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Passa para a próxima câmera
            currentIndex++;

            // Se passar da última, volta para a primeira
            if (currentIndex >= cameraPositions.Length)
            {
                currentIndex = 0;
            }
        }
    }

    void LateUpdate()
    {
        // LateUpdate é melhor para câmera (evita tremedeira)

        // Pega a posição e rotação do alvo atual
        Transform target = cameraPositions[currentIndex];

        // Faz uma transição suave de posição
        transform.position = Vector3.Lerp(
            transform.position,      // posição atual
            target.position,         // posição alvo
            smoothSpeed * Time.deltaTime // velocidade suavizada
        );

        // olha pra bola// apagar depois (provavelmente)
        void LateUpdate()
        {
            // Pega o alvo atual (posição da câmera)
            Transform target = cameraPositions[currentIndex];

            // Move suavemente a câmera até a posição desejada
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                smoothSpeed * Time.deltaTime
            );

            // Faz a câmera sempre olhar para a bola
            transform.LookAt(bola);
        }

        // Faz uma transição suave de rotação
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            target.rotation,
            smoothSpeed * Time.deltaTime
        );
    }
}