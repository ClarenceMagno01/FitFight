using UnityEngine;
using UnityEngine.UI; // For UI elements

public class WarriorThreePose : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isStandingOnLeftLeg = false;
    private bool isStandingOnRightLeg = false;
    private bool isLeaningForward = false;
    private int poseCount = 0; // Count of completed poses
    private float leanThreshold = -0.7f; // Minimum tilt to count as leaning forward

    void Update()
    {
        float vertical = Input.GetAxis("Vertical"); // Detect torso bending forward

        // Detect Standing on One Leg
        if (Input.GetKeyDown(KeyCode.JoystickButton10)) // Left Leg Support
        {
            isStandingOnLeftLeg = true;
            isStandingOnRightLeg = false;
            feedbackText.text = "Standing on Left Leg. Lean Forward Slowly!";
            Debug.Log("Standing on Left Leg.");
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton11)) // Right Leg Support
        {
            isStandingOnRightLeg = true;
            isStandingOnLeftLeg = false;
            feedbackText.text = "Standing on Right Leg. Lean Forward Slowly!";
            Debug.Log("Standing on Right Leg.");
        }

        // Detect Torso Leaning Forward
        if ((vertical <= leanThreshold) && (isStandingOnLeftLeg || isStandingOnRightLeg) && !isLeaningForward)
        {
            isLeaningForward = true;
            feedbackText.text = "Leaning Forward... Hold!";
            Debug.Log("Leaning Forward.");
        }

        // Confirm Pose Completion
        if (isLeaningForward && (isStandingOnLeftLeg || isStandingOnRightLeg))
        {
            poseCount++;
            feedbackText.text = $"Warrior III Pose Complete! Total: {poseCount}";
            Debug.Log($"Warrior III Pose Complete. Total: {poseCount}");

            ResetPose();
        }
    }

    private void ResetPose()
    {
        isLeaningForward = false;
        isStandingOnLeftLeg = false;
        isStandingOnRightLeg = false;
    }
}
