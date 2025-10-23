using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIHelpers
{
    // Call this after activating the panel
    public static void OpenMenuPanel(GameObject panel, Button firstButton = null)
    {
        if (panel == null) return;

        // Ensure an EventSystem exists before using it
        if (EventSystem.current == null)
        {   
            var esGO = new GameObject("EventSystem");
#if ENABLE_INPUT_SYSTEM && !ENABLE_LEGACY_INPUT_MANAGER
            // new Input System
            esGO.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
#else
            // legacy input system
            esGO.AddComponent<StandaloneInputModule>();
#endif
            esGO.AddComponent<EventSystem>();
            Object.DontDestroyOnLoad(esGO);
            Debug.Log("[UIHelpers] Created EventSystem at runtime.");
        }

        // Make sure panel is active
        panel.SetActive(true);

        // Ensure game isn't paused
        Time.timeScale = 1f;

        // Force UI rebuild
        Canvas.ForceUpdateCanvases();
        var rect = panel.GetComponent<RectTransform>();
        if (rect != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        // Now safely set the first selected button
        var es = EventSystem.current;
        if (firstButton != null)
            es.SetSelectedGameObject(firstButton.gameObject);
        else
        {
            var btn = panel.GetComponentInChildren<Button>();
            if (btn != null)
                es.SetSelectedGameObject(btn.gameObject);
        }
    }
}


