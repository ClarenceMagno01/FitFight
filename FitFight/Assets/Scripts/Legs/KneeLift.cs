using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // For UI elements

public class KneeLift : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player
    public InputSystem_Actions inputActions; // Input System reference

    private bool isLiftingLeft = false;
    private bool isLiftingRight = false;
    private int kneeLiftCount = 0; // Count of completed knee lifts

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.StrapconLeft.started += OnKneeLiftLeft;
        inputActions.Player.StrapconRight.started += OnKneeLiftRight;
    }

    private void OnDisable()
    {
        inputActions.Player.StrapconLeft.started -= OnKneeLiftLeft;
        inputActions.Player.StrapconRight.started -= OnKneeLiftRight;
        inputActions.Player.Disable();
    }

    private void OnKneeLiftLeft(InputAction.CallbackContext context)
    {
        if (!isLiftingLeft)
        {
            isLiftingLeft = true;
            kneeLiftCount++;
            feedbackText.text = $"Left Knee Up! Total: {kneeLiftCount}";
            Debug.Log($"Left Knee Lifted. Total: {kneeLiftCount}");
            ResetKneeLift();
        }
    }

    private void OnKneeLiftRight(InputAction.CallbackContext context)
    {
        if (!isLiftingRight)
        {
            isLiftingRight = true;
            kneeLiftCount++;
            feedbackText.text = $"Right Knee Up! Total: {kneeLiftCount}";
            Debug.Log($"Right Knee Lifted. Total: {kneeLiftCount}");
            ResetKneeLift();
        }
    }

    private void ResetKneeLift()
    {
        isLiftingLeft = false;
        isLiftingRight = false;
    }
}
