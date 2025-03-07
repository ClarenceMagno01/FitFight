using UnityEngine;

public class Plank : MonoBehaviour
{
    private bool isResting = true; // Start in the resting position

    void Update()
    {
        // Detect Run (Left Thumb Stick Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
        {
            if (isResting)
            {
                Debug.Log("Currently Planking.");
                isResting = false; // Switch to planking
            }
            else
            {
                Debug.Log("Resting Position.");
                isResting = true; // Switch to resting
            }
        }
    }
}
