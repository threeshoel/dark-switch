using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformActiveWhenBulbOff : MonoBehaviour
{
    [Header("References")]
    public LightBulbController bulb; // assign in inspector or auto-find

    [Header("Visuals (optional)")]
    public Color activeColor = Color.cyan;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
    public bool useColorTint = true;

    private Renderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D;

    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col2D = GetComponent<Collider2D>();

        if (bulb == null)
        {
            bulb = FindObjectOfType<LightBulbController>();
        }
    }

    private void Start()
    {
        if (bulb != null)
        {
            UpdateState(bulb.IsOn);
            bulb.OnStateChanged.AddListener(UpdateState);
        }
        else
        {
            Debug.LogWarning($"[PlatformActiveWhenBulbOff] No LightBulbController found for {name}. Defaulting to inactive.");
            UpdateState(false);
        }
    }

    private void OnDestroy()
    {
        if (bulb != null)
            bulb.OnStateChanged.RemoveListener(UpdateState);
    }

    /// <summary>
    /// Called when the bulb state changes. Platform is active when bulb is OFF.
    /// </summary>
    public void UpdateState(bool bulbOn)
    {
        bool active = !bulbOn; // platform active when bulb is OFF

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = active;
            if (useColorTint)
                spriteRenderer.color = active ? activeColor : inactiveColor;
        }
        else if (meshRenderer != null)
        {
            meshRenderer.enabled = active;
            if (useColorTint)
                meshRenderer.material.color = active ? activeColor : inactiveColor;
        }
        if (col2D != null)
            col2D.enabled = active;
    }
}

