using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SimpleMenuHandler : MonoBehaviour
{
    public Button playButton;
    public Button optionsButton;
    public Button creditsButton;
    public Button quitButton;

    [Header("Scene Names")]
    public string playSceneName;
    public string optionsSceneName;
    public string creditsSceneName;

    private void Start()
    {
        playButton.onClick.AddListener(() => LoadScene(playSceneName));
        optionsButton.onClick.AddListener(() => LoadScene(optionsSceneName));
        creditsButton.onClick.AddListener(() => LoadScene(creditsSceneName));
        quitButton.onClick.AddListener(QuitGame);
    }

    private void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
