using UnityEngine;
using UnityEngine.UI; // For UI elements

public class TreePose : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isStandingOnLeftLeg = false;
    private bool isStandingOnRightLeg = false;
    private bool isLeaningLeft = false;
    private bool isLeaningRight = false;
    private int poseCount = 0; // Count of completed poses
    private float leanThreshold = 0.7f; // Minimum tilt to count as leaning

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // Detect torso leaning

        // Detect Standing on One Leg
        if (Input.GetKeyDown(KeyCode.JoystickButton10)) // Left Leg Support
        {
            isStandingOnLeftLeg = true;
            isStandingOnRightLeg = false;
            feedbackText.text = "Standing on Left Leg. Lean Slowly!";
            Debug.Log("Standing on Left Leg.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton11)) // Right Leg Support
        {
            isStandingOnRightLeg = true;
            isStandingOnLeftLeg = false;
            feedbackText.text = "Standing on Right Leg. Lean Slowly!";
            Debug.Log("Standing on Right Leg.");
        }

        // Detect Leaning
        if ((horizontal <= -leanThreshold && isStandingOnLeftLeg) && !isLeaningLeft)
        {
            isLeaningLeft = true;
            feedbackText.text = "Leaning Left... Hold!";
            Debug.Log("Leaning Left.");
        }
        else if ((horizontal >= leanThreshold && isStandingOnRightLeg) && !isLeaningRight)
        {
            isLeaningRight = true;
            feedbackText.text = "Leaning Right... Hold!";
            Debug.Log("Leaning Right.");
        }

        // Confirm Pose Completion
        if ((isLeaningLeft && isStandingOnLeftLeg) || (isLeaningRight && isStandingOnRightLeg))
        {
            poseCount++;
            feedbackText.text = $"Tree Pose Complete! Total: {poseCount}";
            Debug.Log($"Tree Pose Complete. Total: {poseCount}");

            ResetPose();
        }
    }

    private void ResetPose()
    {
        isLeaningLeft = false;
        isLeaningRight = false;
        isStandingOnLeftLeg = false;
        isStandingOnRightLeg = false;
    }
}
