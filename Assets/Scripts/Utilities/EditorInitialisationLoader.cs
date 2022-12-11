using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script loads the initial Scene, to allow to start the game from any gameplay Scene
/// </summary>
public class EditorInitialisationLoader : MonoBehaviour
{
#if UNITY_EDITOR
    public string initializationScene;

    private void Awake()
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == initializationScene)
            {
                return;
            }
        }
        SceneManager.LoadSceneAsync(initializationScene, LoadSceneMode.Additive);
    }
#endif
}
