using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioBrightnessLoader : MonoBehaviour
{
    [Header("Música (Resources)")]
    [SerializeField] private string musicResourcePath = "Audio/musica"; 
    [SerializeField] private string musicObjectName = "BackgroundMusic";

    [Header("Escenas donde DEBE sonar música")]
    [SerializeField] private string[] musicScenes = { "MainMenu", "Settings", "Cube" };

    void Start()
    {
        // 1) Cargar valores guardados (si no existen, usa 1f por defecto)
        float volume = PlayerPrefs.GetFloat("volume", 1f);
        float brightness = PlayerPrefs.GetFloat("brightness", 1f);

        // 2) Aplicar volumen global
        AudioListener.volume = volume;

        // 3) Música de fondo (si la escena lo permite)
        TryEnsureBackgroundMusic(volume);

        // 4) Aplicar brillo global (luz ambiental)
        UpdateBrightness(brightness);
    }

    private void TryEnsureBackgroundMusic(float volume)
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Si esta escena NO está en la lista, no crear música
        if (!IsMusicScene(sceneName)) return;

        // Si ya existe, no duplicar
        GameObject musicObj = GameObject.Find(musicObjectName);
        if (musicObj != null) return;

        // Crear objeto
        musicObj = new GameObject(musicObjectName);
        AudioSource audioSource = musicObj.AddComponent<AudioSource>();

        // Cargar clip desde Resources
        AudioClip clip = Resources.Load<AudioClip>(musicResourcePath);
        if (clip == null)
        {
            Debug.LogWarning($"No se encontró el AudioClip en Resources: '{musicResourcePath}'. " +
                             $"Asegúrate de tenerlo en Assets/Resources/{musicResourcePath}.wav");
            Destroy(musicObj);
            return;
        }

        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 1f; // IMPORTANTE: el volumen lo controla AudioListener.volume
        audioSource.Play();

        // Persistencia controlada por escenas
        musicObj.AddComponent<MusicPersistence>();
    }

    private bool IsMusicScene(string sceneName)
    {
        for (int i = 0; i < musicScenes.Length; i++)
        {
            if (musicScenes[i] == sceneName) return true;
        }
        return false;
    }

    public void UpdateBrightness(float brightness)
    {
        BrightnessManager.SetBrightness(brightness);
    }
}
