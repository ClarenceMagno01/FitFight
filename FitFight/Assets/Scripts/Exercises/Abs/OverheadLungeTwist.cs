using UnityEngine;

public class OverheadLungeTwist : MonoBehaviour
{
    private bool isOverhead = false;           // Flag for overhead position detection
    private bool hasSquatted = false;         // Flag to detect a squat action
    private bool hasTwistedLeft = false;      // Flag to detect a twist to the left
    private bool hasTwistedRight = false;     // Flag to detect a twist to the right

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Detect the squat action (Back Button) only if it hasn’t been detected yet
        if (Input.GetKeyDown(KeyCode.JoystickButton6) && !hasSquatted) // Back Button
        {
            Debug.Log("Squat detected.");
            hasSquatted = true; // Allow twists to be detected
        }

        // Check if the Ringcon is in the overhead position (vertical == -1)
        if (vertical == -1f && hasSquatted)
        {
            if (!isOverhead)
            {
                Debug.Log("Ringcon is now overhead.");
                isOverhead = true;
                ResetTwistFlags(); // Reset twist flags
            }

            // Detect twist to the left (horizontal == -1)
            if (horizontal == -1f && !hasTwistedLeft)
            {
                Debug.Log("Overhead Lunge Twist to the Left detected.");
                hasTwistedLeft = true;
                hasTwistedRight = false; // Reset opposite twist
            }

            // Detect twist to the right (horizontal == 1)
            if (horizontal == 1f && !hasTwistedRight)
            {
                Debug.Log("Overhead Lunge Twist to the Right detected.");
                hasTwistedRight = true;
                hasTwistedLeft = false; // Reset opposite twist
            }
        }
        else
        {
            // Reset the overhead state if the Ringcon is no longer overhead
            if (isOverhead)
            {
                Debug.Log("Ringcon is no longer overhead.");
                isOverhead = false;
                ResetTwistFlags(); // Reset twist flags
            }
        }

        // Reset squat detection if vertical position moves significantly upward (e.g., > 0)
        if (vertical > 0f)
        {
            hasSquatted = false;
        }
    }

    // Helper method to reset twist flags
    private void ResetTwistFlags()
    {
        hasTwistedLeft = false;
        hasTwistedRight = false;
    }
}
