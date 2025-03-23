using UnityEngine;

public class BowPull : MonoBehaviour
{
    private float detectionThreshold = 1f; // Threshold for horizontal orientation
    private bool isLightPulling = false;   // Flag for light pull detection
    private bool isHorizontal = false;    // Flag for horizontal orientation detection
    private bool hasDetectedBowPull = false; // Flag to prevent multiple BowPull logs

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Detect light pull (Right Shoulder Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) // Right Shoulder Button
        {
            isLightPulling = true;
        }
        else if (Input.GetKeyUp(KeyCode.JoystickButton5)) // Reset light pull state
        {
            isLightPulling = false;
            hasDetectedBowPull = false; // Allow BowPull detection again
        }

        // Check if the Ringcon is held in a bow manner (horizontally)
        if (Mathf.Abs(horizontal) >= detectionThreshold)
        {
            if (!isHorizontal) // Log when transitioning to horizontal
            {
                Debug.Log($"Ringcon is now held in a bow manner. Horizontal = {horizontal}, Vertical = {vertical}");
                isHorizontal = true;
            }
        }
        else if (horizontal == 0f) // Reset to not horizontal only when horizontal is 0
        {
            if (isHorizontal) // Log when transitioning out of horizontal
            {
                Debug.Log($"Ringcon is no longer held in a bow manner. Horizontal = {horizontal}, Vertical = {vertical}");
                isHorizontal = false;
            }
        }

        // Detect BowPull when both conditions are met
        if (isLightPulling && isHorizontal && !hasDetectedBowPull)
        {
            Debug.Log("BowPull detected");
            hasDetectedBowPull = true; // Prevent repeated logs
        }
    }
}
