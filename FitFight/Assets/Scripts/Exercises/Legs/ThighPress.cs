<<<<<<< Updated upstream
using UnityEngine;

public class ThighPress : MonoBehaviour
{
    private bool isInRunPosition = false; // Tracks if the user is in the run position

    void Update()
    {
        // Detect Run Position (Left Thumb Stick Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
        {
            if (!isInRunPosition)
            {
                Debug.Log("Run position detected. Ready for Thigh Press.");
                isInRunPosition = true; // Enter run position state
            }
        }

        // Detect Press Action (Left Shoulder Button) after Run position is detected
        if (Input.GetKeyDown(KeyCode.JoystickButton4) && isInRunPosition) // Left Shoulder Button
        {
            Debug.Log("Thigh Press detected.");
=======
//using UnityEngine;

//public class ThighPress : MonoBehaviour
//{
//    private bool isInRunPosition = false; // Tracks if the user is in the run position

//    void Update()
//    {
//        // Detect Run Position (Left Thumb Stick Button)
//        if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
//        {
//            if (!isInRunPosition)
//            {
//                Debug.Log("Run position detected. Ready for Thigh Press.");
//                isInRunPosition = true; // Enter run position state
//            }
//        }

//        // Detect Press Action (Left Shoulder Button) after Run position is detected
//        if (Input.GetKeyDown(KeyCode.JoystickButton4) && isInRunPosition) // Left Shoulder Button
//        {
//            Debug.Log("Thigh Press detected.");
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI; // For UI elements

public class ThighPress : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isPressing = false;
    private int thighPressCount = 0; // Count of completed presses

    void Update()
    {
        // Ensure player is seated (Strapcon Down - Button 12)
        bool isSeated = Input.GetKey(KeyCode.JoystickButton12);

        if (isSeated)
        {
            feedbackText.text = "Seated position detected. Squeeze the Ring-Con!";
        }
        else
        {
            feedbackText.text = "Sit down before pressing!";
            isPressing = false; // Reset pressing state if player stands up
        }

        // Detect Thigh Press (Left Shoulder Button - Button 4)
        if (isSeated && Input.GetKeyDown(KeyCode.JoystickButton4) && !isPressing)
        {
            isPressing = true;
            thighPressCount++;
            feedbackText.text = $"Thigh Press Complete! Total: {thighPressCount}";
            Debug.Log($"Thigh Press detected. Total: {thighPressCount}");
        }

        // Reset state when button is released
        if (Input.GetKeyUp(KeyCode.JoystickButton4))
        {
            isPressing = false;
>>>>>>> Stashed changes
        }
    }
}
