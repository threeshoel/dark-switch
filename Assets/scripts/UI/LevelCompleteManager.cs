using System.Collections;
using UnityEngine;

public class LevelCompleteManager : MonoBehaviour
{
    public static LevelCompleteManager Instance { get; private set; }

    [Header("Level Complete Image")]
    public GameObject levelCompleteImage; // drag your Image GameObject here
    public float displayTime = 1.5f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (levelCompleteImage != null)
            levelCompleteImage.SetActive(false);
    }

    public void CompleteLevel()
    {
        StartCoroutine(CompleteRoutine());
    }

    private IEnumerator CompleteRoutine()
    {
        // Show image
        if (levelCompleteImage != null)
            levelCompleteImage.SetActive(true);

        // Wait
        yield return new WaitForSeconds(displayTime);

        // Hide image
        if (levelCompleteImage != null)
            levelCompleteImage.SetActive(false);

        // Load next level
        LevelManager.Instance?.LoadNextLevel();
    }
}
