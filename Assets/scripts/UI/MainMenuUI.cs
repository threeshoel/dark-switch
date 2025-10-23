using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlay);
        quitButton.onClick.AddListener(OnQuit);
    }

    private void OnDestroy()
    {
        playButton.onClick.RemoveListener(OnPlay);
        quitButton.onClick.RemoveListener(OnQuit);
    }

    void OnPlay()
    {
        LevelManager.Instance?.StartGame();
    }

    void OnQuit()
    {
        LevelManager.Instance?.QuitGame();
    }
}
