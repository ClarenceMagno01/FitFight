//using UnityEngine;

//public class OverheadSquat : MonoBehaviour
//{
//    private bool isRingOverhead = false; // Tracks if the Ringcon is held overhead

//    void Update()
//    {
//        // Get the rotation/tilt inputs from the Ringcon
//        float vertical = Input.GetAxis("Vertical");

//        // Check if the Ringcon is held overhead (vertical == -1f)
//        if (vertical == -1f)
//        {
//            if (!isRingOverhead)
//            {
//                Debug.Log("Ringcon is now held overhead. Ready for Overhead Squats.");
//                isRingOverhead = true; // Mark as overhead position
//            }

//            // Detect Squat (Back Button)
//            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
//            {
//                Debug.Log("Overhead Squat detected.");
//            }
//        }
//        else
//        {
//            // Reset overhead state when the Ringcon is no longer overhead
//            if (isRingOverhead)
//            {
//                Debug.Log("Ringcon is no longer held overhead.");
//                isRingOverhead = false;
//            }
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI; // For UI elements

public class OverheadSquat : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isRingOverhead = false;
    private bool isSquatting = false;
    private int squatCount = 0; // Count of completed squats

    void Update()
    {
        // Detect if the Ring-Con is held overhead using the Vertical axis
        float vertical = Input.GetAxis("Vertical");
        isRingOverhead = vertical <= -0.9f; // Threshold for overhead position

        if (isRingOverhead)
        {
            feedbackText.text = "Ring-Con Overhead! Ready to squat!";
        }
        else
        {
            feedbackText.text = "Raise the Ring-Con Overhead!";
            isSquatting = false; // Reset squat state if arms drop
        }

        // Overhead Squat Detection (Back Button - Button 6)
        if (isRingOverhead && Input.GetKeyDown(KeyCode.JoystickButton6) && !isSquatting)
        {
            isSquatting = true;
            feedbackText.text = "Squatting... Hold position!";
            Debug.Log("Player is performing an Overhead Squat.");
        }

        // Squat Completion (Strapcon Down - Button 12)
        if (isSquatting && Input.GetKeyDown(KeyCode.JoystickButton12))
        {
            isSquatting = false;
            squatCount++;
            feedbackText.text = $"Overhead Squat Complete! Total: {squatCount}";
            Debug.Log($"Overhead Squat detected. Total Squats: {squatCount}");
        }
    }
}
