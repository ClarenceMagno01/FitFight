using UnityEngine;

public class OverheadSquat : MonoBehaviour
{
    private bool isRingOverhead = false; // Tracks if the Ringcon is held overhead

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");

        // Check if the Ringcon is held overhead (vertical == -1f)
        if (vertical == -1f)
        {
            if (!isRingOverhead)
            {
                Debug.Log("Ringcon is now held overhead. Ready for Overhead Squats.");
                isRingOverhead = true; // Mark as overhead position
            }

            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
            {
                Debug.Log("Overhead Squat detected.");
            }
        }
        else
        {
            // Reset overhead state when the Ringcon is no longer overhead
            if (isRingOverhead)
            {
                Debug.Log("Ringcon is no longer held overhead.");
                isRingOverhead = false;
            }
        }
    }
}
