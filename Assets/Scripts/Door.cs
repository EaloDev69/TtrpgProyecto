using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Door : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset targetScene;

    private void OnValidate()
    {
        if (targetScene != null)
        {
            sceneName = targetScene.name;
            AddSceneToBuildSettings(targetScene);
        }
    }

    private void AddSceneToBuildSettings(SceneAsset scene)
    {
        string scenePath = AssetDatabase.GetAssetPath(scene);
        var buildScenes = EditorBuildSettings.scenes;

        // Verifica si ya está en el build settings
        foreach (var s in buildScenes)
        {
            if (s.path == scenePath) return; // Ya está, no hace nada
        }

        // Si no está, la agrega
        var newBuildScenes = new EditorBuildSettingsScene[buildScenes.Length + 1];
        for (int i = 0; i < buildScenes.Length; i++)
        {
            newBuildScenes[i] = buildScenes[i];
        }
        newBuildScenes[buildScenes.Length] = new EditorBuildSettingsScene(scenePath, true);
        EditorBuildSettings.scenes = newBuildScenes;

        Debug.Log($"Escena '{scene.name}' agregada al Build Settings automáticamente");
    }
#endif

    [HideInInspector] public string sceneName;

    private bool isPlayerInRange;

    void Update()
    {
        if (isPlayerInRange)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}