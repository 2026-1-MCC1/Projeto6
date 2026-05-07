using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

// Responsável por iniciar automaticamente a API Node.js
public class StartAPI : MonoBehaviour
{
    [Header("Caminho da API")]

    // Caminho da pasta onde está o arquivo server.js (deve ser a pasta da sua API Node)
    [SerializeField]
    private string caminhoAPI = @"C:\Users\26029305\Documents\GitHub\Projeto6\PrototipoAgrocap\Assets\API";


    private void Start()
    {
        // Impede que esse objeto seja destruído ao trocar de cena
        DontDestroyOnLoad(gameObject);
        // Evita criar vários processos caso a cena Menu seja carregada novamente
        if (FindObjectsOfType<StartAPI>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Inicia a API automaticamente
        IniciarAPI();
    }


    // Executa o comando:
    // node server.js
    private void IniciarAPI()
    {
        // Configuração do processo que será aberto
        ProcessStartInfo startInfo = new ProcessStartInfo();

        // Executa diretamente o Node.js
        startInfo.FileName = "node";
        // Arquivo que será executado
        startInfo.Arguments = "server.js";
        // Pasta onde está o server.js
        startInfo.WorkingDirectory = caminhoAPI;

        // Não abre janela do CMD
        startInfo.CreateNoWindow = true;
        // Necessário para esconder a janela
        startInfo.UseShellExecute = false;

        // Inicia o processo
        Process.Start(startInfo);
    }
    
}