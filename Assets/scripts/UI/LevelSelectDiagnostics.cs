using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text;
using System.Collections.Generic;
using TMPro; // always include TMP - safe if package present

[RequireComponent(typeof(Canvas))]
public class LevelSelectDiagnostics : MonoBehaviour
{
    [Tooltip("Run automatically when panel enables")]
    public bool runOnEnable = true;

    void OnEnable()
    {
        if (runOnEnable) RunDetailedDiagnostics();
    }

    [ContextMenu("RunDetailedDiagnostics")]
    public void RunDetailedDiagnostics()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== LevelSelectDiagnosticsFull START ===");
        sb.AppendLine($"Time.timeScale = {Time.timeScale}");
        sb.AppendLine($"Active Scene = {SceneManager.GetActiveScene().name}");
        sb.AppendLine($"EventSystem.current = {(EventSystem.current != null ? EventSystem.current.gameObject.name : "null")}");

        var canvas = GetComponent<Canvas>();
        sb.AppendLine($"Canvas '{canvas.name}': enabled={canvas.enabled}, renderMode={canvas.renderMode}, sortOrder={canvas.sortingOrder}");
        var gr = canvas.GetComponent<GraphicRaycaster>();
        sb.AppendLine($"GraphicRaycaster present = {(gr != null)}, enabled = {(gr != null ? gr.enabled.ToString() : "n/a")}");
        var cg = canvas.GetComponent<CanvasGroup>();
        sb.AppendLine(cg != null ? $"CanvasGroup: interactable={cg.interactable}, blocksRaycasts={cg.blocksRaycasts}, alpha={cg.alpha}" : "CanvasGroup: (none)");

        sb.AppendLine($"LevelManager.Instance = {(LevelManager.Instance != null ? LevelManager.Instance.gameObject.name : "null")}");
        var buttons = GetComponentsInChildren<Button>(true);
        sb.AppendLine($"Found {buttons.Length} Button(s) under panel.");

        for (int i = 0; i < buttons.Length; i++)
        {
            var b = buttons[i];
            sb.AppendLine($"-- Button[{i}] name='{b.name}' activeInHierarchy={b.gameObject.activeInHierarchy} interactable={b.interactable}");

            // find all TMP components under this button
            var tmps = b.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (tmps != null && tmps.Length > 0)
            {
                sb.AppendLine($"   Found {tmps.Length} TextMeshProUGUI component(s):");
                for (int t = 0; t < tmps.Length; t++)
                {
                    var tmp = tmps[t];
                    sb.AppendLine($"     [{t}] GO='{tmp.gameObject.name}' enabled={tmp.enabled} activeInHierarchy={tmp.gameObject.activeInHierarchy} text='{(tmp.text ?? "<null>").Replace('\n',' ')}'");
                }
            }
            else
            {
                sb.AppendLine("   No TextMeshProUGUI components found under this button.");
            }

            // legacy Text components
            var uis = b.GetComponentsInChildren<Text>(true);
            if (uis != null && uis.Length > 0)
            {
                sb.AppendLine($"   Found {uis.Length} UnityEngine.UI.Text component(s):");
                for (int t = 0; t < uis.Length; t++)
                {
                    var u = uis[t];
                    sb.AppendLine($"     [{t}] GO='{u.gameObject.name}' enabled={u.enabled} activeInHierarchy={u.gameObject.activeInHierarchy} text='{(u.text ?? "<null>").Replace('\n',' ')}'");
                }
            }
            else
            {
                sb.AppendLine("   No UnityEngine.UI.Text components found under this button.");
            }

            // List other relevant components that might block raycasts
            var imgs = b.GetComponentsInChildren<Image>(true);
            foreach (var img in imgs)
            {
                sb.AppendLine($"   Image on '{img.gameObject.name}': raycastTarget={img.raycastTarget}, enabled={img.enabled}, activeInHierarchy={img.gameObject.activeInHierarchy}");
            }

            // Persistent OnClick info
            var oc = b.onClick;
            int pc = oc.GetPersistentEventCount();
            sb.AppendLine($"   OnClick persistent count = {pc}");
            for (int j = 0; j < pc; j++)
            {
                var target = oc.GetPersistentTarget(j);
                var method = oc.GetPersistentMethodName(j);
                sb.AppendLine($"     [{j}] target = {(target != null ? target.name : "NULL")}   method = '{method}'");
            }

            // Derived scene name guesses (TMP first, then UI.Text, then GameObject name)
            string derived = DeriveSceneNameFromButton(b);
            sb.AppendLine($"   Derived scene name = '{derived}'  InBuild = {SceneInBuild(derived)}");
        }

        sb.AppendLine("=== LevelSelectDiagnosticsFull END ===");
        Debug.Log(sb.ToString());
    }

    static string DeriveSceneNameFromButton(Button b)
    {
        // TMP preferred
        var tmps = b.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in tmps)
        {
            if (!string.IsNullOrWhiteSpace(tmp.text))
            {
                var s = tmp.text.Trim();
                if (s.Contains(":")) s = s.Substring(s.LastIndexOf(':') + 1).Trim();
                return s;
            }
        }

        // legacy UI text
        var uis = b.GetComponentsInChildren<Text>(true);
        foreach (var u in uis)
        {
            if (!string.IsNullOrWhiteSpace(u.text))
            {
                var s = u.text.Trim();
                if (s.Contains(":")) s = s.Substring(s.LastIndexOf(':') + 1).Trim();
                return s;
            }
        }

        // fallback to name and cleanup
        string name = b.gameObject.name.Trim();
        if (name.EndsWith("Button", System.StringComparison.OrdinalIgnoreCase))
            name = name.Substring(0, name.Length - "Button".Length).Trim();
        if (name.EndsWith("_btn", System.StringComparison.OrdinalIgnoreCase))
            name = name.Substring(0, name.Length - 4).Trim();
        return name;
    }

    static bool SceneInBuild(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name == sceneName) return true;
        }
        return false;
    }
}

