using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Yoga
{
    public class WarriorTwoPose : ExerciseBase
    {
       // public Text feedbackText; // UI feedback for player

        private bool isLeftLegForward = false;
        private bool isRightLegForward = false;
        private bool areArmsWide = false;
        private bool isTwistingArms = false;
        private int poseCount = 0; // Count of completed poses
        private float armSpreadThreshold = 0.7f; // Minimum rotation to confirm arms wide
        private float twistThreshold = 0.7f;
        
        public override void Restart()
        {
            base.Restart();
            isLeftLegForward = false;
            isRightLegForward = false;
            areArmsWide = false;
            isTwistingArms = false;
            poseCount = 0; // Count of completed poses
            armSpreadThreshold = 0.7f; // Minimum rotation to confirm arms wide
            twistThreshold = 0.7f;
        }

        public override void Run(Action onRep)
        {
            float horizontal = Input.GetAxis("Horizontal"); // Detect arm spreading
            float vertical = Input.GetAxis("Vertical"); // Detect arm twisting

            // Detect Forward Leg Position
            if (Input.GetKeyDown(KeyCode.JoystickButton10)) // Left Leg Forward
            {
                isLeftLegForward = true;
                isRightLegForward = false;
                //feedbackText.text = "Left Leg Forward. Spread Arms Wide!";
                Debug.Log("Left Leg Forward.");
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton11)) // Right Leg Forward
            {
                isRightLegForward = true;
                isLeftLegForward = false;
                //feedbackText.text = "Right Leg Forward. Spread Arms Wide!";
                Debug.Log("Right Leg Forward.");
            }

            // Detect Arm Spreading
            if ((horizontal <= -armSpreadThreshold || horizontal >= armSpreadThreshold) && (isLeftLegForward || isRightLegForward))
            {
                areArmsWide = true;
                //feedbackText.text = "Arms Spread! Now Twist Slowly!";
                Debug.Log("Arms Spread Wide.");
            }

            // Detect Arm Twisting
            if ((vertical <= -twistThreshold || vertical >= twistThreshold) && areArmsWide)
            {
                isTwistingArms = true;
                //feedbackText.text = "Twisting Arms... Hold!";
                Debug.Log("Twisting Arms.");
            }

            // Confirm Pose Completion
            if (isTwistingArms && areArmsWide && (isLeftLegForward || isRightLegForward))
            {
                poseCount++;
                //feedbackText.text = $"Warrior II Pose Complete! Total: {poseCount}";
                Debug.Log($"Warrior II Pose Complete. Total: {poseCount}");
                ResetPose();
                counter++;
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }
        }
        
        private void ResetPose()
        {
            isTwistingArms = false;
            areArmsWide = false;
            isLeftLegForward = false;
            isRightLegForward = false;
        }
    }
}