using UnityEngine;

public class Squat : MonoBehaviour
{
    void Update()
    {
        // Detect Squat (Back Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
        {
            Debug.Log("Squat detected.");
        }
    }
}
