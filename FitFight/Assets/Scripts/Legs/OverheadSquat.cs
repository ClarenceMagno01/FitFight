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
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI elements

public class OverheadSquat : MonoBehaviour
{
    public Text feedbackText; // UI text for feedback
    public InputSystem_Actions inputActions; // Input System reference

    private bool isRingOverhead = false;
    private bool isSquatting = false;
    private int squatCount = 0;

    private void Awake()
    {
        // Initialize input actions
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.RingconRotate.performed += OnRingconRotate;
        inputActions.Player.RingconRotate.canceled += OnRingconRotate;
        inputActions.Player.StrapconDown.started += OnSquat;
        inputActions.Player.StrapconDown.canceled += OnSquat;
        inputActions.Player.RingconHeavyPress.started += OnSquat; // Alternative squat trigger
    }

    private void OnDisable()
    {
        inputActions.Player.RingconRotate.performed -= OnRingconRotate;
        inputActions.Player.RingconRotate.canceled -= OnRingconRotate;
        inputActions.Player.StrapconDown.started -= OnSquat;
        inputActions.Player.StrapconDown.canceled -= OnSquat;
        inputActions.Player.RingconHeavyPress.started -= OnSquat;
        inputActions.Player.Disable();
    }

    private void OnRingconRotate(InputAction.CallbackContext context)
    {
        Vector2 rotation = context.ReadValue<Vector2>();

        if (rotation.y <= -0.9f) // Threshold for overhead position
        {
            if (!isRingOverhead)
            {
                Debug.Log("Ringcon is now overhead. Ready for Overhead Squats.");
                feedbackText.text = "Hold the Ringcon Overhead!";
                isRingOverhead = true;
            }
        }
        else
        {
            if (isRingOverhead)
            {
                Debug.Log("Ringcon is no longer overhead.");
                feedbackText.text = "Raise the Ringcon!";
                isRingOverhead = false;
                isSquatting = false;
            }
        }
    }

    private void OnSquat(InputAction.CallbackContext context)
    {
        if (isRingOverhead)
        {
            if (context.started && !isSquatting)
            {
                isSquatting = true;
                squatCount++;
                Debug.Log($"Overhead Squat detected. Total Squats: {squatCount}");
                feedbackText.text = $"Squat Complete! Total: {squatCount}";
            }
            else if (context.canceled)
            {
                isSquatting = false;
            }
        }
    }
}
