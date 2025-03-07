using UnityEngine;

public class FrontPress : MonoBehaviour
{
    private float detectionThreshold = 0.8f; // Threshold for downward orientation
    private bool isHeavyPressing = false;  // Flag for heavy press detection
    private bool isHeldDown = false;       // Flag for downward detection

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

        // Check if the Ringcon is held down (Joycon facing downward)
        if (vertical >= detectionThreshold) // Detect if vertical axis indicates "downward"
        {
            if (!isHeldDown) // Prevent repeated logs
            {
                Debug.Log($"Ringcon is now held down. Horizontal = {horizontal}, Vertical = {vertical}");
                isHeldDown = true;
            }
        }
        else if (vertical == 0f) // Reset state when the Ringcon is no longer pointing downward
        {
            if (isHeldDown)
            {
                Debug.Log($"Ringcon is no longer held down. Horizontal = {horizontal}, Vertical = {vertical}");
                isHeldDown = false;
            }
        }

        // Detect FrontPress when both conditions are met
        if (isHeavyPressing && isHeldDown)
        {
            Debug.Log("FrontPress detected");
        }
    }
}
