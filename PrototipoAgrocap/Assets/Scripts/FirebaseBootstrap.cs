using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class FirebaseBootstrap : MonoBehaviour
{
    private const string DefaultObjectName = "FirebaseBootstrap";

    private static FirebaseBootstrap instance;
    private static Task<bool> initializationTask;

    public static bool IsReady { get; private set; }
    public static string StatusMessage { get; private set; } = "Firebase ainda nao foi inicializado.";
    public static FirebaseApp App { get; private set; }
    public static FirebaseFirestore Firestore { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        gameObject.name = DefaultObjectName;
        DontDestroyOnLoad(gameObject);

        EnsureInitializedAsync();
    }

    public static FirebaseBootstrap EnsureInstance()
    {
        if (instance != null)
        {
            return instance;
        }

        instance = FindFirstObjectByType<FirebaseBootstrap>();
        if (instance != null)
        {
            instance.gameObject.name = DefaultObjectName;
            DontDestroyOnLoad(instance.gameObject);
            return instance;
        }

        GameObject bootstrapObject = new GameObject(DefaultObjectName);
        instance = bootstrapObject.AddComponent<FirebaseBootstrap>();
        return instance;
    }

    public Task<bool> EnsureInitializedAsync()
    {
        return EnsureInitializedAsyncStatic();
    }

    public static Task<bool> EnsureInitializedAsyncStatic()
    {
        EnsureInstance();

        if (initializationTask != null)
        {
            return initializationTask;
        }

        TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
        initializationTask = completionSource.Task;
        StatusMessage = "Verificando dependencias do Firebase...";

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                IsReady = false;
                StatusMessage = "Falha ao verificar dependencias do Firebase.";
                Debug.LogError("FirebaseBootstrap: falha ao verificar dependencias.\n" + task.Exception);
                completionSource.TrySetResult(false);
                return;
            }

            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus != DependencyStatus.Available)
            {
                IsReady = false;
                StatusMessage = "Dependencias do Firebase indisponiveis: " + dependencyStatus;
                Debug.LogError("FirebaseBootstrap: dependencias indisponiveis: " + dependencyStatus);
                completionSource.TrySetResult(false);
                return;
            }

            App = FirebaseApp.DefaultInstance;
            Firestore = FirebaseFirestore.DefaultInstance;
            IsReady = true;
            StatusMessage = "Firebase inicializado com sucesso.";
            Debug.Log("FirebaseBootstrap: Firebase pronto para uso.");
            completionSource.TrySetResult(true);
        });

        return initializationTask;
    }
}
