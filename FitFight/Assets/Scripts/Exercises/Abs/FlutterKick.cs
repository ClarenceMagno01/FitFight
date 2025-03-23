using UnityEngine;

public class FlutterKick : MonoBehaviour
{
    private bool isSquatting = false; // Tracks if the squat position is detected
    private bool isOverhead = false; // Tracks if the Ringcon is held overhead

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");

        // Check if the Ringcon is held overhead (vertical == 1)
        if (vertical == -1f)
        {
            if (!isOverhead)
            {
                Debug.Log("Ringcon is now held overhead. Ready for Flutter Kicks.");
                isOverhead = true; // Enter overhead state
            }

            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
            {
                if (!isSquatting)
                {
                    Debug.Log("Squat detected. Right Leg Up.");
                    isSquatting = true; // Mark as in squat position
                }
            }

            // Detect Run (Right Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
            {
                if (isSquatting)
                {
                    Debug.Log("Run detected. Left Leg Up.");
                    isSquatting = false; // Reset to allow alternating actions
                }
            }
        }
        else
        {
            // Reset overhead state if the Ringcon is no longer overhead
            if (isOverhead)
            {
                Debug.Log("Ringcon is no longer held overhead.");
                isOverhead = false;
            }
        }
    }
}
