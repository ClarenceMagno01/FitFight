using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class BowPull : ExerciseBase
    {
        private float detectionThreshold = 1f; // Threshold for horizontal orientation
        private bool isLightPulling = false;   // Flag for light pull detection
        private bool isHorizontal = false;    // Flag for horizontal orientation detection
        private bool hasDetectedBowPull = false;
        
        public override void Restart()
        {
            base.Restart();
            detectionThreshold = 1f; // Threshold for horizontal orientation
            isLightPulling = false;   // Flag for light pull detection
            isHorizontal = false;    // Flag for horizontal orientation detection
            hasDetectedBowPull = false;
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Detect light pull (Right Shoulder Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton5)) // Right Shoulder Button
            {
                isLightPulling = true;
            }
            else if (Input.GetKeyUp(KeyCode.JoystickButton5)) // Reset light pull state
            {
                isLightPulling = false;
                hasDetectedBowPull = false; // Allow BowPull detection again
            }

            // Check if the Ringcon is held in a bow manner (horizontally)
            if (Mathf.Abs(horizontal) >= detectionThreshold)
            {
                if (!isHorizontal) // Log when transitioning to horizontal
                {
                    Debug.Log($"Ringcon is now held in a bow manner. Horizontal = {horizontal}, Vertical = {vertical}");
                    isHorizontal = true;
                }
            }
            else if (horizontal == 0f) // Reset to not horizontal only when horizontal is 0
            {
                if (isHorizontal) // Log when transitioning out of horizontal
                {
                    Debug.Log($"Ringcon is no longer held in a bow manner. Horizontal = {horizontal}, Vertical = {vertical}");
                    isHorizontal = false;
                }
            }

            // Detect BowPull when both conditions are met
            if (isLightPulling && isHorizontal && !hasDetectedBowPull)
            {
                Debug.Log("BowPull detected");
                hasDetectedBowPull = true; // Prevent repeated logs
                counter++;
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }
        }
    }
}