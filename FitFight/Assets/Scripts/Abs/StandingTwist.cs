using UnityEngine;

public class StandingTwist : MonoBehaviour
{
    private bool isStanding = false;        // Tracks if the user is in the standing position
    private bool hasTwistedLeft = false;    // Flag to detect a twist to the left
    private bool hasTwistedRight = false;   // Flag to detect a twist to the right

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Check if the user is in the standing position
        if (vertical == 1f)
        {
            if (!isStanding)
            {
                Debug.Log("Standing position detected.");
                isStanding = true; // Enter standing position state
            }

            // Detect twist to the left (horizontal == -1)
            if (horizontal == -1f && !hasTwistedLeft)
            {
                Debug.Log("Standing Twist to the Left detected.");
                hasTwistedLeft = true;
                hasTwistedRight = false; // Reset right twist flag
            }

            // Detect twist to the right (horizontal == 1)
            if (horizontal == 1f && !hasTwistedRight)
            {
                Debug.Log("Standing Twist to the Right detected.");
                hasTwistedRight = true;
                hasTwistedLeft = false; // Reset left twist flag
            }
        }
        else
        {
            if (isStanding)
            {
                Debug.Log("Not in standing position.");
                isStanding = false; // Exit standing position state
            }
        }
    }
}
