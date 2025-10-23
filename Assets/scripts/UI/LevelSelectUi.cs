using UnityEngine;

public class LevelSelectUI : MonoBehaviour
{
    public GameObject levelSelectPanel;

    // Open the Level Select panel
    public void OpenLevelSelect()
    {
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(true);
    }

    // Close the Level Select panel
    public void CloseLevelSelect()
    {
        if (levelSelectPanel != null)
            levelSelectPanel.SetActive(false);
    }
}