using UnityEngine;

public class TricepKickback : MonoBehaviour
{
    private bool isNeutralPosition = false;    // Flag for neutral position detection
    private bool hasDetectedTricepKickback = false; // Flag to prevent multiple TricepKickback logs

    void Update()
    {
        // Get the rotation/tilt inputs from the Ringcon
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        // Check if the Ringcon is in the neutral position (-1)
        if (vertical == -1f)
        {
            if (!isNeutralPosition)
            {
                isNeutralPosition = true;
                hasDetectedTricepKickback = false;
            }
        }
        // Check if the Ringcon has moved (vertical > -0.5)
        else if (vertical > -0.5f)
        {
            if (isNeutralPosition)
            {
                Debug.Log($"Ringcon movement detected. Horizontal = {horizontal}, Vertical = {vertical}");
                isNeutralPosition = false;
            }
        }

        // Detect TricepKickback when the movement and heavy press are both detected
        if (isNeutralPosition && !hasDetectedTricepKickback)
        {
            Debug.Log("TricepKickback detected");
            hasDetectedTricepKickback = true; // Prevent repeated logs
        }
    }
}
