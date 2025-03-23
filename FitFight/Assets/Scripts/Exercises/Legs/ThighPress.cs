using UnityEngine;

public class ThighPress : MonoBehaviour
{
    private bool isInRunPosition = false; // Tracks if the user is in the run position

    void Update()
    {
        // Detect Run Position (Left Thumb Stick Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
        {
            if (!isInRunPosition)
            {
                Debug.Log("Run position detected. Ready for Thigh Press.");
                isInRunPosition = true; // Enter run position state
            }
        }

        // Detect Press Action (Left Shoulder Button) after Run position is detected
        if (Input.GetKeyDown(KeyCode.JoystickButton4) && isInRunPosition) // Left Shoulder Button
        {
            Debug.Log("Thigh Press detected.");
        }
    }
}
