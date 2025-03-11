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
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI elements

public class ThighPress : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player
    public InputSystem_Actions inputActions; // Input System reference

    private int thighPressCount = 0; // Count of completed thigh presses

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RingconHeavyPress.started += OnThighPress; // Detect heavy press
        inputActions.Player.RingconHeavyPress.canceled += OnThighPressRelease; // Detect release
    }

    private void OnDisable()
    {
        inputActions.Player.RingconHeavyPress.started -= OnThighPress;
        inputActions.Player.RingconHeavyPress.canceled -= OnThighPressRelease;
        inputActions.Player.Disable();
    }

    private void OnThighPress(InputAction.CallbackContext context)
    {
        thighPressCount++;
        Debug.Log($"Thigh Press detected. Total Presses: {thighPressCount}");
        feedbackText.text = $"Thigh Press Complete! Total: {thighPressCount}";
    }

    private void OnThighPressRelease(InputAction.CallbackContext context)
    {
        feedbackText.text = "Squeeze the Ring-Con with your thighs!";
    }
}
