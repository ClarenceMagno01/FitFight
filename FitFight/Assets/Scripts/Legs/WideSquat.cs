using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI elements

public class WideSquat : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player
    public InputSystem_Actions inputActions; // Input System reference

    private bool isSquatting = false;
    private bool isWideStance = false;
    private int squatCount = 0; // Count of completed squats

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.StrapconLeft.started += OnLegsMove;
        inputActions.Player.StrapconRight.started += OnLegsMove;
        inputActions.Player.StrapconDown.started += OnSquatStart;
        inputActions.Player.StrapconDown.canceled += OnSquatEnd;
    }

    private void OnDisable()
    {
        inputActions.Player.StrapconLeft.started -= OnLegsMove;
        inputActions.Player.StrapconRight.started -= OnLegsMove;
        inputActions.Player.StrapconDown.started -= OnSquatStart;
        inputActions.Player.StrapconDown.canceled -= OnSquatEnd;
        inputActions.Player.Disable();
    }

    private void OnLegsMove(InputAction.CallbackContext context)
    {
        isWideStance = true;
        feedbackText.text = "Wide stance detected. Ready to squat!";
        Debug.Log("Wide stance detected.");
    }

    private void OnSquatStart(InputAction.CallbackContext context)
    {
        if (isWideStance && !isSquatting)
        {
            isSquatting = true;
            feedbackText.text = "Squatting... Hold position!";
            Debug.Log("Player is performing a Wide Squat.");
        }
    }

    private void OnSquatEnd(InputAction.CallbackContext context)
    {
        if (isSquatting)
        {
            isSquatting = false;
            squatCount++;
            feedbackText.text = $"Wide Squat Complete! Total: {squatCount}";
            Debug.Log($"Wide Squat detected. Total: {squatCount}");
        }
    }
}
