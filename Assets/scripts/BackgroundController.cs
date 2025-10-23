using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public LightBulbController bulb;
    public SpriteRenderer backgroundRenderer;

    public Sprite lightOnBackground;
    public Sprite lightOffBackground;

    private void Start()
    {
        if (bulb == null)
            bulb = FindObjectOfType<LightBulbController>();

        if (bulb != null)
        {
            bulb.OnStateChanged.AddListener(UpdateBackground);
            UpdateBackground(bulb.IsOn);
        }
        else
        {
            Debug.LogWarning("No LightBulbController found!");
        }
    }

    private void UpdateBackground(bool isOn)
    {
        if (backgroundRenderer == null)
        {
            Debug.LogWarning("No Background Renderer assigned!");
            return;
        }

        backgroundRenderer.sprite = isOn ? lightOnBackground : lightOffBackground;
    }
}
