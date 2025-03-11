//using UnityEngine;

//public class Squat : MonoBehaviour
//{
//    void Update()
//    {
//        // Detect Squat (Back Button)
//        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
//        {
//            Debug.Log("Squat detected.");
//        }
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI elements

public class Squat : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player
    public InputSystem_Actions inputActions; // Input System reference

    private bool isSquatting = false;
    private int squatCount = 0; // Count of completed squats

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.StrapconDown.started += OnSquatStart;  // Detect downward motion
        inputActions.Player.StrapconDown.canceled += OnSquatEnd;   // Detect release
    }

    private void OnDisable()
    {
        inputActions.Player.StrapconDown.started -= OnSquatStart;
        inputActions.Player.StrapconDown.canceled -= OnSquatEnd;
        inputActions.Player.Disable();
    }

    private void OnSquatStart(InputAction.CallbackContext context)
    {
        if (!isSquatting)
        {
            isSquatting = true;
            feedbackText.text = "Squatting... Hold position!";
            Debug.Log("Player is squatting.");
        }
    }

    private void OnSquatEnd(InputAction.CallbackContext context)
    {
        if (isSquatting)
        {
            isSquatting = false;
            squatCount++;
            feedbackText.text = $"Squat Complete! Total: {squatCount}";
            Debug.Log($"Squat detected. Total Squats: {squatCount}");
        }
    }
}
