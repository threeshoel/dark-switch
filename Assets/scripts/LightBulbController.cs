using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

public class LightBulbController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool isOn = true;
    public bool IsOn => isOn;

    [Header("Visual (optional)")]
    public Renderer bulbRenderer; // not used during SetState now - animation handles visuals
    public Color onColor = new Color(1f, 0.9f, 0.4f);
    public Color offColor = new Color(1f, 1f, 1f);

    [Header("Animation (optional)")]
    public Animator animator;
    public bool useTrigger = false;
    public string toggleTrigger = "Toggle";
    public string boolParamName = "IsOn"; // make sure this EXACTLY matches your Animator parameter

    [Header("Events")]
    public BoolEvent OnStateChanged;

    private void Start()
    {
        if (bulbRenderer == null) bulbRenderer = GetComponent<Renderer>();
        // Do NOT apply visual here if the animation should control visuals. 
        // If you want initial visual to follow isOn, you can uncomment:
        // ApplyVisual();

        if (animator != null && !useTrigger && !string.IsNullOrEmpty(boolParamName))
        {
            animator.SetBool(boolParamName, isOn);
        }

        Debug.Log($"LightBulbController.Start: initial isOn={isOn}, animatorAssigned={(animator!=null)}");
    }

    public void Toggle() => SetState(!isOn);

    public void SetState(bool newState)
    {
        if (isOn == newState) 
        {
            Debug.Log($"LightBulbController.SetState: state unchanged ({isOn})");
            return;
        }

        isOn = newState;
        Debug.Log($"LightBulbController.SetState: newState={newState}");

        // Drive animator
        if (animator != null)
        {
            if (useTrigger)
            {
                if (!string.IsNullOrEmpty(toggleTrigger))
                {
                    animator.SetTrigger(toggleTrigger);
                    Debug.Log($"Animator.SetTrigger('{toggleTrigger}') called");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(boolParamName))
                {
                    animator.SetBool(boolParamName, isOn);
                    Debug.Log($"Animator.SetBool('{boolParamName}', {isOn}) called. Animator now reports = {animator.GetBool(boolParamName)}");
                }
            }
        }
        else
        {
            Debug.LogWarning("LightBulbController.SetState: No Animator assigned.");
        }

        // DO NOT call OnStateChanged or ApplyVisual here if you want platform/visual changes to wait for Animation Event
        // OnStateChanged?.Invoke(isOn);
        // ApplyVisual();
    }

    // Call this from Animation Event when the animation reaches the visual "on/off" moment:
    public void InvokeStateChanged()
    {
        Debug.Log($"InvokeStateChanged called from animation. isOn={isOn}");
        ApplyVisual(); // apply the color now (animation event sync)
        OnStateChanged?.Invoke(isOn);
    }

    public void ApplyVisualFromAnimation()
    {
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (bulbRenderer != null)
        {
            var mat = bulbRenderer.material;
            mat.color = isOn ? onColor : offColor;
            Debug.Log($"ApplyVisual: set color to {(isOn ? "onColor" : "offColor")}");
        }
    }
}
