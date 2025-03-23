using UnityEngine;
using UnityEngine.UI; // For UI elements

public class WideSquat : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isWideStance = false;
    private bool isSquatting = false;
    private int squatCount = 0; // Count of completed squats

    void Update()
    {
        // Check if legs are wide apart (Strapcon Left - Button 10 & Strapcon Right - Button 11)
        bool leftLegOut = Input.GetKey(KeyCode.JoystickButton10);
        bool rightLegOut = Input.GetKey(KeyCode.JoystickButton11);

        isWideStance = leftLegOut && rightLegOut;

        if (isWideStance)
        {
            feedbackText.text = "Wide stance detected. Ready to squat!";
        }
        else
        {
            feedbackText.text = "Move legs apart for wide squat!";
            isSquatting = false; // Reset squat state if legs are not wide apart
        }

        // Wide Squat Detection (Back Button - Button 6)
        if (isWideStance && Input.GetKeyDown(KeyCode.JoystickButton6) && !isSquatting)
        {
            isSquatting = true;
            feedbackText.text = "Squatting... Hold position!";
            Debug.Log("Player is performing a Wide Squat.");
        }

        // Squat Completion (Strapcon Down - Button 12)
        if (isSquatting && Input.GetKeyDown(KeyCode.JoystickButton12))
        {
            isSquatting = false;
            squatCount++;
            feedbackText.text = $"Wide Squat Complete! Total: {squatCount}";
            Debug.Log($"Wide Squat detected. Total: {squatCount}");
        }
    }
}
