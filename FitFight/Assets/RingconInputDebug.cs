using UnityEngine;

public class RingconInputDebug : MonoBehaviour
{
    private float timeSinceLastLog = 0f; // Timer for controlling log interval
    private float logInterval = 1f;     // Interval in seconds between logs

    void Update()
    {
        // Update the timer
        timeSinceLastLog += Time.deltaTime;

        // Log Ringcon Rotate values every 2 seconds
        if (timeSinceLastLog >= logInterval)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                Debug.Log($"Ringcon Rotate: Horizontal = {horizontal}, Vertical = {vertical}");
            }

            timeSinceLastLog = 0f; // Reset the timer
        }

        // Ringcon Light Pull (Right Shoulder Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton5)) // Right Shoulder Button
        {
            Debug.Log("Ringcon Light Pull detected.");
        }

        // Ringcon Light Press (Left Shoulder Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton4)) // Left Shoulder Button
        {
            Debug.Log("Ringcon Light Press detected.");
        }

        // Run (Left Thumb Stick Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Left Stick Button
        {
            Debug.Log("Run detected.");
        }

        // Sprint (Right Thumb Stick Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton9)) // Right Stick Button
        {
            Debug.Log("Sprint detected.");
        }

        // Squat (Back Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
        {
            Debug.Log("Squat detected.");
        }

        // Strapcon Directions
        if (Input.GetKeyDown(KeyCode.JoystickButton10)) // Strapcon Left
        {
            Debug.Log("Strapcon Left detected.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton11)) // Strapcon Right
        {
            Debug.Log("Strapcon Right detected.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton12)) // Strapcon Down
        {
            Debug.Log("Strapcon Down detected.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton13)) // Strapcon Up
        {
            Debug.Log("Strapcon Up detected.");
        }
    }
}
