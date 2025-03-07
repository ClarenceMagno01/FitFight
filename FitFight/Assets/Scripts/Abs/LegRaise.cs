using UnityEngine;

public class LegRaise : MonoBehaviour
{
    private bool isResting = true; // Start in the resting position (Squat)

    void Update()
    {
        // Detect Squat (Back Button) for resting position
        if (Input.GetKeyDown(KeyCode.JoystickButton6) && !isResting) // Back Button
        {
            Debug.Log("Resting Position (Squat) detected.");
            isResting = true; // Switch to resting position
        }

        // Detect Run (Left Thumb Stick Button) for legs raised
        if (Input.GetKeyDown(KeyCode.JoystickButton8) && isResting) // Run Button
        {
            Debug.Log("Legs Raised (Run) detected.");
            isResting = false; // Switch to legs raised position
        }
    }
}
