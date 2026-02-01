using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPersistence : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Música permitida en estas escenas:
        if (sceneName != "MainMenu" && sceneName != "Settings" && sceneName != "Cube")
        {
            Destroy(gameObject);
        }
    }
}
