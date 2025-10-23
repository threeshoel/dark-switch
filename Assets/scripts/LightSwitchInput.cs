using UnityEngine;

public class LightSwitchInput : MonoBehaviour
{
    public LightBulbController bulb;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E pressed - LightSwitchInput detected.");
            if (bulb != null)
            {
                bulb.Toggle();
                Debug.Log("Called bulb.Toggle(), bulb.IsOn after Toggle = " + bulb.IsOn);
            }
            else
            {
                Debug.LogWarning("LightSwitchInput: bulb reference not assigned in Inspector.");
            }
        }
    }
}
