using UnityEngine;
using UnityEngine.UI; // For UI elements

public class WarriorOnePose : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isLeftLegForward = false;
    private bool isRightLegForward = false;
    private bool isArmsRaised = false;
    private int poseCount = 0; // Count of completed poses
    private float armRaiseThreshold = 0.7f; // Minimum vertical tilt to confirm raised arms

    void Update()
    {
        float vertical = Input.GetAxis("Vertical"); // Detect Ring-Con arm movement

        // Detect Forward Leg Position
        if (Input.GetKeyDown(KeyCode.JoystickButton10)) // Left Leg Forward
        {
            isLeftLegForward = true;
            isRightLegForward = false;
            feedbackText.text = "Left Leg Forward. Raise Arms!";
            Debug.Log("Left Leg Forward.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton11)) // Right Leg Forward
        {
            isRightLegForward = true;
            isLeftLegForward = false;
            feedbackText.text = "Right Leg Forward. Raise Arms!";
            Debug.Log("Right Leg Forward.");
        }

        // Detect Arm Raising
        if ((vertical >= armRaiseThreshold) && (isLeftLegForward || isRightLegForward) && !isArmsRaised)
        {
            isArmsRaised = true;
            feedbackText.text = "Arms Raised... Hold!";
            Debug.Log("Arms Raised.");
        }

        // Confirm Pose Completion
        if (isArmsRaised && (isLeftLegForward || isRightLegForward))
        {
            poseCount++;
            feedbackText.text = $"Warrior I Pose Complete! Total: {poseCount}";
            Debug.Log($"Warrior I Pose Complete. Total: {poseCount}");

            ResetPose();
        }
    }

    private void ResetPose()
    {
        isArmsRaised = false;
        isLeftLegForward = false;
        isRightLegForward = false;
    }
}
