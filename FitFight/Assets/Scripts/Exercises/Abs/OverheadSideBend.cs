using UnityEngine;

public class OverheadSideBend : MonoBehaviour
{
    private bool isOverhead = false;         // Flag for overhead position detection
    private bool hasBendedLeft = false;     // Flag to detect a left bend
    private bool hasBendedRight = false;    // Flag to detect a right bend
    private float sideBendThreshold = 0.3f; // Horizontal threshold for side bends

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
                Debug.Log("Ringcon is now overhead.");
                isOverhead = true;
                ResetBendFlags(); // Reset side bend flags
            }

            // Detect left side bend
            if (horizontal <= -sideBendThreshold && !hasBendedLeft)
            {
                Debug.Log("Overhead Side Bend to the Left detected.");
                hasBendedLeft = true;
                hasBendedRight = false; // Reset opposite bend
            }

            // Detect right side bend
            if (horizontal >= sideBendThreshold && !hasBendedRight)
            {
                Debug.Log("Overhead Side Bend to the Right detected.");
                hasBendedRight = true;
                hasBendedLeft = false; // Reset opposite bend
            }
        }
        else
        {
            // Reset the overhead state if the Ringcon is no longer overhead
            if (isOverhead)
            {
                Debug.Log("Ringcon is no longer overhead.");
                isOverhead = false;
                ResetBendFlags(); // Reset side bend flags
            }
        }
    }

    // Helper method to reset bend flags
    private void ResetBendFlags()
    {
        hasBendedLeft = false;
        hasBendedRight = false;
    }
}
