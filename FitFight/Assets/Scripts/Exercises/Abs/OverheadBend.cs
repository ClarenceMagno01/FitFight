using UnityEngine;

public class OverheadBend : MonoBehaviour
{
    private bool isOverhead = false;            // Flag for overhead position detection
    private bool hasDetectedBend = false;      // Flag to prevent repeated bend detection

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Check if the Ringcon is in the overhead position (vertical == -1)
        if (vertical == -1f)
        {
            if (!isOverhead)
            {
                isOverhead = true;
            }
            Debug.Log("Ringcon is now overhead.");
            hasDetectedBend = false; // Reset bend detection state
        }

        // Detect forward bend (vertical > 0)
        if (isOverhead && vertical > 0f)
        {
            if (!hasDetectedBend)
            {
                Debug.Log("Overhead Bend detected.");
                hasDetectedBend = true; // Prevent repeated detection until reset
            }
        }

        // Reset overhead state when returning to neutral
        if (vertical > 0.5f && isOverhead)
        {
            isOverhead = false;
            Debug.Log("Ringcon is no longer overhead.");
        }
    }
}
