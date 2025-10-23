using UnityEngine;
using UnityEngine.SceneManagement;

public class FallDeathHandler : MonoBehaviour
{
    [Tooltip("If the player's Y position goes below this value, they will 'die'.")]
    public float killY = -20f;

    [Tooltip("Time delay before restarting scene after death.")]
    public float respawnDelay = 1f;

    private bool isDead = false;

    void Update()
    {
        if (!isDead && transform.position.y < killY)
        {
            isDead = true;
            Debug.Log("Player fell! Restarting game from Level 1...");
            Invoke(nameof(RestartFromLevel1), respawnDelay);
        }
    }

    void RestartFromLevel1()
    {
        // Use LevelManager if available
        if (LevelManager.Instance != null && LevelManager.Instance.sceneNames.Count > 0)
        {
            LevelManager.Instance.LoadSceneByName(LevelManager.Instance.sceneNames[0]);
        }
        else
        {
            // Fallback: direct load by index
            Debug.LogWarning("LevelManager not found — loading Level 1 directly by index.");
            SceneManager.LoadScene(1); // assuming Level 1 is build index 1
        }
    }
}

