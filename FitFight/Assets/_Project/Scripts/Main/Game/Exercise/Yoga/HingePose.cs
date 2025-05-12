using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Yoga
{
    public class HingePose : ExerciseBase
    {
        //public Text feedbackText; // UI feedback for player

        private bool isTorsoBent = false;
        private bool isArmRaised = false;
        private bool isArmLowered = false;
        private int poseCount = 0; // Count of completed reps
        private float torsoBendThreshold = -0.7f; // Minimum tilt to count as bent forward
        private float armMoveThreshold = 0.7f;
        
        public override void Restart()
        {
            base.Restart();
            isTorsoBent = false;
            isArmRaised = false;
            isArmLowered = false;
            poseCount = 0; // Count of completed reps
            torsoBendThreshold = -0.7f; // Minimum tilt to count as bent forward
            armMoveThreshold = 0.7f;
        }

        public override void Run(Action onRep)
        {
            float vertical = Input.GetAxis("Vertical"); // Detect torso bending forward
            float horizontal = Input.GetAxis("Horizontal"); // Detect arm movement

            // Detect Torso Bending Forward
            if (vertical <= torsoBendThreshold && !isTorsoBent)
            {
                isTorsoBent = true;
                //feedbackText.text = "Torso Bent Forward. Raise and Lower Arm Slowly!";
                Debug.Log("Torso Bent Forward.");
            }

            // Detect Arm Raising
            if (isTorsoBent && horizontal >= armMoveThreshold && !isArmRaised)
            {
                isArmRaised = true;
                //feedbackText.text = "Arm Raised... Now Lower Slowly!";
                Debug.Log("Arm Raised.");
            }

            // Detect Arm Lowering
            if (isTorsoBent && horizontal <= -armMoveThreshold && isArmRaised && !isArmLowered)
            {
                isArmLowered = true;
                //feedbackText.text = "Arm Lowered... Pose Complete!";
                Debug.Log("Arm Lowered.");
            }

            // Confirm Pose Completion
            if (isTorsoBent && isArmRaised && isArmLowered)
            {
                poseCount++;
                //feedbackText.text = $"Hinge Pose Complete! Total: {poseCount}";
                Debug.Log($"Hinge Pose Complete. Total: {poseCount}");
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
            isTorsoBent = false;
            isArmRaised = false;
            isArmLowered = false;
        }
    }
}