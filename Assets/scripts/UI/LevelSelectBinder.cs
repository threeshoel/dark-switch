using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
#if TMP_PRESENT || UNITY_TEXTMESHPRO
using TMPro;
#endif

[RequireComponent(typeof(Canvas))]
public class LevelSelectBinder : MonoBehaviour
{
    [Tooltip("Run automatically when the panel is enabled")]
    public bool runOnEnable = true;

    void OnEnable()
    {
        if (runOnEnable) BindButtons();
    }

    [ContextMenu("BindButtonsNow")]
    public void BindButtons()
    {
        // Make sure time and EventSystem are correct
        Time.timeScale = 1f;

        if (EventSystem.current == null)
        {
            var es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<StandaloneInputModule>();
            DontDestroyOnLoad(es);
            Debug.Log("[LevelSelectRuntimeBinder] Created missing EventSystem.");
        }

        // Force UI to update layout
        Canvas.ForceUpdateCanvases();
        var rect = GetComponent<RectTransform>();
        if (rect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        var buttons = GetComponentsInChildren<Button>(true);
        Debug.Log($"[LevelSelectRuntimeBinder] Found {buttons.Length} buttons to bind.");

        foreach (var b in buttons)
        {
            string sceneName = GetButtonSceneName(b);

            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning($"[LevelSelectRuntimeBinder] Could not determine scene for button '{b.name}'. Skipping.");
                continue;
            }

            if (!SceneInBuild(sceneName))
            {
                Debug.LogWarning($"[LevelSelectRuntimeBinder] Scene '{sceneName}' is not in Build Settings. Button '{b.name}' left unbound.");
                continue;
            }

            b.onClick.RemoveAllListeners();
            string sceneCopy = sceneName;

            b.onClick.AddListener(() =>
            {
                if (LevelManager.Instance == null)
                {
                    Debug.LogError("[LevelSelectRuntimeBinder] LevelManager.Instance is null. Can't load scene.");
                    return;
                }

                Debug.Log($"[LevelSelectRuntimeBinder] Loading scene '{sceneCopy}'");
                LevelManager.Instance.LoadSceneByName(sceneCopy);
            });
        }

        // Select first button for controller navigation
        if (buttons.Length > 0 && EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
    }

    string GetButtonSceneName(Button b)
    {
        string label = null;

        // 1. Try TextMeshProUGUI first (TMP)
#if TMP_PRESENT || UNITY_TEXTMESHPRO
        var tmp = b.GetComponentInChildren<TextMeshProUGUI>(true);
        if (tmp != null && !string.IsNullOrWhiteSpace(tmp.text))
            label = tmp.text.Trim();
#endif

        // 2. Try legacy UI.Text if TMP wasn’t found
        if (string.IsNullOrEmpty(label))
        {
            var uiText = b.GetComponentInChildren<Text>(true);
            if (uiText != null && !string.IsNullOrWhiteSpace(uiText.text))
                label = uiText.text.Trim();
        }

        // 3. Fallback to GameObject name
        if (string.IsNullOrEmpty(label))
            label = b.gameObject.name.Trim();

        // Clean up text like "Level 1: Scene1"
        if (label.Contains(":"))
            label = label.Substring(label.LastIndexOf(':') + 1).Trim();

        return label;
    }

    bool SceneInBuild(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName)
                return true;
        }
        return false;
    }
}
