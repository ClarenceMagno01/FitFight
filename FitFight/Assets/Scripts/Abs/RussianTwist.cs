using UnityEngine;

public class RussianTwist : MonoBehaviour
{
    private bool isSquatting = false;        // Tracks if the user is in the squat position
    private bool hasTwistedLeft = false;    // Flag to detect a twist to the left
    private bool hasTwistedRight = false;   // Flag to detect a twist to the right

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float horizontal = Input.GetAxis("Horizontal");

        // Check if the user is in the squat position (detected via Back Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
        {
            if (!isSquatting)
            {
                Debug.Log("Squat position detected. Ready for Russian Twists.");
                isSquatting = true; // Enter squat position state
            }
        }

        // Detect twists only if the user is in the squat position
        if (isSquatting)
        {
            // Detect twist to the left (horizontal == -1)
            if (horizontal == -1f && !hasTwistedLeft)
            {
                Debug.Log("Russian Twist to the Left detected.");
                hasTwistedLeft = true;
                hasTwistedRight = false; // Reset right twist flag
            }

            // Detect twist to the right (horizontal == 1)
            if (horizontal == 1f && !hasTwistedRight)
            {
                Debug.Log("Russian Twist to the Right detected.");
                hasTwistedRight = true;
                hasTwistedLeft = false; // Reset left twist flag
            }
        }
    }
}
