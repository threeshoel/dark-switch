using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Main Menu")]
    public string mainMenuSceneName = "MainMenu";

    [Header("Game Levels (in order)")]
    public List<string> sceneNames = new List<string>();

    [Header("Transition")]
    public float transitionDelay = 0.5f;
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 0.4f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        if (sceneNames.Count > 0)
            LoadSceneByName(sceneNames[0]);
        else
            LoadSceneByName(mainMenuSceneName);
    }

    public void LoadNextLevel()
    {
        int nextIndex = GetCurrentSceneIndexInList() + 1;

        if (nextIndex >= sceneNames.Count)
        {
            LoadSceneByName(mainMenuSceneName);
            return;
        }

        LoadSceneByName(sceneNames[nextIndex]);
    }

    public void LoadSceneByName(string name)
    {
        StartCoroutine(LoadSceneRoutine(() => SceneManager.LoadScene(name)));
    }

    IEnumerator LoadSceneRoutine(System.Action loadAction)
    {
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f, fadeDuration));
        }

        if (transitionDelay > 0f)
            yield return new WaitForSeconds(transitionDelay);

        loadAction?.Invoke();

        yield return null;

        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(0f, fadeDuration));
        }
    }

    int GetCurrentSceneIndexInList()
    {
        string cur = SceneManager.GetActiveScene().name;
        return Mathf.Max(0, sceneNames.IndexOf(cur));
    }

    IEnumerator Fade(float targetAlpha, float duration)
    {
        if (fadeCanvasGroup == null) yield break;
        float start = fadeCanvasGroup.alpha;
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(start, targetAlpha, t / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }

    public void QuitGame()
    {
        Debug.Log("Quit called.");
        Application.Quit();
    }
}
