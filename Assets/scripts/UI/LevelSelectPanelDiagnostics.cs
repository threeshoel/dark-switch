using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

[RequireComponent(typeof(Canvas))]
public class LevelSelectPanelDiagnostics : MonoBehaviour
{
    void OnEnable()
    {
        Debug.Log("[Diag] LevelSelectPanel enabled. Running diagnostics...");
        PrintGlobalState();
        PrintCanvasState();
        PrintButtonsInfo();
    }

    void PrintGlobalState()
    {
        Debug.Log($"[Diag] Time.timeScale = {Time.timeScale}");
        Debug.Log($"[Diag] EventSystem.current = {(EventSystem.current != null ? EventSystem.current.gameObject.name : "null")}");
    }

    void PrintCanvasState()
    {
        var canvas = GetComponent<Canvas>();
        var gr = canvas.GetComponent<GraphicRaycaster>();
        Debug.Log($"[Diag] Canvas '{canvas.name}' enabled={canvas.enabled} sortOrder={canvas.sortingOrder} renderMode={canvas.renderMode}");
        Debug.Log($"[Diag] GraphicRaycaster present = {(gr != null)}, enabled = {(gr != null ? gr.enabled.ToString() : "n/a")}");
        var cg = GetComponent<CanvasGroup>();
        if (cg != null) Debug.Log($"[Diag] CanvasGroup: interactable={cg.interactable}, blocksRaycasts={cg.blocksRaycasts}, alpha={cg.alpha}");
        else Debug.Log("[Diag] No CanvasGroup on panel.");
    }

    void PrintButtonsInfo()
    {
        var buttons = GetComponentsInChildren<Button>(true);
        Debug.Log($"[Diag] Found {buttons.Length} button(s) under this panel.");
        for (int i = 0; i < buttons.Length; i++)
        {
            var b = buttons[i];
            var active = b.gameObject.activeInHierarchy;
            var interact = b.interactable;
            var img = b.GetComponent<Image>();
            bool raycastTarget = img != null ? img.raycastTarget : true;

            var sb = new StringBuilder();
            sb.AppendLine($"[Diag] Button[{i}] name='{b.name}' active={active} interactable={interact} raycastTarget={raycastTarget}");

            // Does any parent CanvasGroup block raycasts?
            var parents = b.GetComponentsInParent<CanvasGroup>(true);
            foreach (var p in parents)
            {
                sb.AppendLine($"   parent CanvasGroup on '{p.gameObject.name}': interactable={p.interactable}, blocksRaycasts={p.blocksRaycasts}, alpha={p.alpha}");
            }

            // Print OnClick persistent targets (may reveal missing references)
            var oc = b.onClick;
            int pc = oc.GetPersistentEventCount();
            sb.AppendLine($"   OnClick persistent count = {pc}");
            for (int j = 0; j < pc; j++)
            {
                var target = oc.GetPersistentTarget(j);
                var method = oc.GetPersistentMethodName(j);
                sb.AppendLine($"     [{j}] target = {(target != null ? target.name : "NULL")}  method = '{method}'");
            }

            Debug.Log(sb.ToString());
        }
    }
}

