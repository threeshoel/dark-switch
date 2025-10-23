using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GoalAutoAdvance : MonoBehaviour
{
    public string playerTag = "Player";
    public float delay = 0.2f;

    private bool triggered = false;

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (!other.CompareTag(playerTag))
        {
            Debug.Log($"GoalAdvanceTrigger: Collided with {other.name} but not player");
            return;
        }

        triggered = true;
        Debug.Log($"GoalAdvanceTrigger: Player reached goal '{name}'");
        StartCoroutine(AdvanceAfterDelay());
    }

    private IEnumerator AdvanceAfterDelay()
    {
        Debug.Log("GoalAdvanceTrigger: Waiting before advancing...");
        yield return new WaitForSeconds(delay);

        if (LevelManager.Instance == null)
        {
            Debug.LogError("GoalAdvanceTrigger: LevelManager.Instance is NULL!");
            yield break;
        }

        Debug.Log("GoalAdvanceTrigger: Calling LoadNextLevel()");
        LevelManager.Instance.LoadNextLevel();
    }
}
