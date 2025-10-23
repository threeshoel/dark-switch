using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlatformActiveWhenBulbOn : MonoBehaviour
{
    [Header("References")]
    public LightBulbController bulb; // assign in inspector or auto-find

    [Header("Visuals (optional)")]
    public Color activeColor = Color.green;
    public Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);
    public bool useColorTint = true; // set false if you don't want tinting

    // cached components
    private Renderer meshRenderer;
    private SpriteRenderer spriteRenderer;
    private Collider2D col2D;

    private void Awake()
    {
        // Try to find components
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
            // initialize to bulb's current state
            UpdateState(bulb.IsOn);
            bulb.OnStateChanged.AddListener(UpdateState);
        }
        else
        {
            Debug.LogWarning($"[PlatformActiveWhenBulbOn] No LightBulbController found for {name}. Defaulting to active.");
            UpdateState(true);
        }
    }

    private void OnDestroy()
    {
        if (bulb != null)
            bulb.OnStateChanged.RemoveListener(UpdateState);
    }

    /// <summary>
    /// Called when the bulb state changes. 'bulbOn' is true when the bulb is ON.
    /// </summary>
    public void UpdateState(bool bulbOn)
    {
        bool active = bulbOn; // platform active when bulb is ON

        // Renderer / Sprite visibility
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = active;
            if (useColorTint)
            {
                // tint by modifying spriteRenderer.color (no material instancing needed)
                spriteRenderer.color = active ? activeColor : inactiveColor;
            }
        }
        else if (meshRenderer != null)
        {
            meshRenderer.enabled = active;
            if (useColorTint)
            {
                // create instance material (touching .material creates an instance)
                meshRenderer.material.color = active ? activeColor : inactiveColor;
            }
        }

        // Colliders
        if (col2D != null)
            col2D.enabled = active;
    }
}
