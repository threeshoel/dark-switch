using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelEndTrigger : MonoBehaviour
{
    [Header("Player detection")]
    public string playerTag = "Player";

    [Header("Level Complete Image")]
    [Tooltip("Assign an Image GameObject that will appear briefly when the level is completed.")]
    public GameObject levelCompleteImage;
    public float displayTime = 1.5f; // time the image is shown before loading next level

    [Header("Behavior")]
    public float delayBeforeAction = 0.2f; // optional small delay for sound/VFX
    public UnityEngine.Events.UnityEvent onLevelComplete;

    bool triggered = false;

    private void Reset()
    {
        // Ensure collider is a trigger by default
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (!IsPlayerCollider(other))
        {
            Debug.Log($"LevelEndTrigger: collided by '{other.name}' but it's not the player (tag={other.tag}).");
            return;
        }

        Debug.Log($"LevelEndTrigger: Player detected by '{other.name}'. Triggering level end.");
        triggered = true;
        StartCoroutine(CompleteRoutine());
    }

    bool IsPlayerCollider(Collider2D col)
    {
        if (col == null) return false;

        // Check component
        if (col.GetComponentInParent<PlayerCharacter>() != null) return true;

        // Check tag
        if (!string.IsNullOrEmpty(playerTag) && col.CompareTag(playerTag))
            return true;

        return false;
    }

    IEnumerator CompleteRoutine()
    {
        // Invoke any immediate hooks (sound, VFX)
        onLevelComplete?.Invoke();

        // Optional small delay
        if (delayBeforeAction > 0f)
            yield return new WaitForSeconds(delayBeforeAction);

        // Show Level Complete image
        if (levelCompleteImage != null)
        {
            levelCompleteImage.SetActive(true);
        }

        // Wait for display time
        yield return new WaitForSeconds(displayTime);

        // Hide image
        if (levelCompleteImage != null)
        {
            levelCompleteImage.SetActive(false);
        }

        // Load next level
        LevelManager.Instance?.LoadNextLevel();
    }
}
