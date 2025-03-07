using UnityEngine;

public class OverheadArmTwist : MonoBehaviour
{
    private bool isOverhead = false;            // Flag for overhead position detection
    private bool hasDetectedArmTwist = false;  // Flag to prevent multiple OverheadArmTwist logs

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Check if the Ringcon is in the overhead position (vertical == 1)
        if (vertical == 1f)
        {
            if (!isOverhead) // Log only on transition to overhead
            {
                Debug.Log("Half Twist.");
                isOverhead = true;
                hasDetectedArmTwist = false; // Reset the twist detection flag
            }
        }


        // Check if the Ringcon has twisted downward (vertical == -1)
        if (isOverhead && vertical == -1f)
        {
            if (!hasDetectedArmTwist)
            {
                Debug.Log($"Overhead Arm Twist detected.");
                hasDetectedArmTwist = true; // Prevent repeated detection until reset
                isOverhead = false;
            }
        }
    }
}
