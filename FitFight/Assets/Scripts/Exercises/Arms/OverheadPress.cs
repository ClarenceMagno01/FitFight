using UnityEngine;

public class OverheadPress : MonoBehaviour
{
    private float detectionThreshold = -1f; // Threshold for overhead orientation
    private bool isHeavyPressing = false;  // Flag for heavy press detection
    private bool isOverhead = false;       // Flag for overhead detection

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Detect heavy press (right trigger)
        if (Input.GetKeyDown(KeyCode.JoystickButton4)) // Adjust if "TriggerAxis" is different for your setup
        {
            isHeavyPressing = true;
        }
        else
        {
            isHeavyPressing = false;
        }

        // Check if the Ringcon is overhead (Joycon facing downward)
        if (vertical <= detectionThreshold)
        {
            if (!isOverhead) // Check to prevent repeated logs
            {
                Debug.Log($"Ringcon is now held overhead. Horizontal = {horizontal}, Vertical = {vertical}");
                isOverhead = true;
            }
        }
        else if (vertical == 0f)
        {
            if (isOverhead) // Reset flag and log when vertical returns to 0
            {
                Debug.Log($"Ringcon is no longer held overhead. Horizontal = {horizontal}, Vertical = {vertical}");
                isOverhead = false;
            }
        }

        // Detect OverheadPress when both conditions are met
        if (isHeavyPressing && isOverhead)
        {
            Debug.Log("OverheadPress detected");
        }
    }
}
