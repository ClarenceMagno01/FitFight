using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class OverheadArmSpin : ExerciseBase
    {
        private bool isOverhead = false;        // Flag for overhead position detection
        private bool hasDetectedSpin = false; 
        
        public override void Restart()
        {
            base.Restart();
            isOverhead = false;        // Flag for overhead position detection
            hasDetectedSpin = false; 
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the Ringcon is in the overhead position (vertical == 1)
            if (vertical == -1f)
            {
                if (!isOverhead)
                {
                    Debug.Log("Ringcon is now overhead.");
                    isOverhead = true;
                }

                // Detect spin when horizontal is within the target range
                if (horizontal <= 0.6f && horizontal >= -0.6f && !hasDetectedSpin)
                {
                    Debug.Log("Overhead Arm Spin detected.");
                    hasDetectedSpin = true; // Prevent repeated detection until reset
                    counter++;
                }
                
                if (counter >= 1)
                {
                    onRep.Invoke();
                    counter = 0;
                }

                // Allow re-detection of spin when horizontal moves outside the target range
                if (horizontal > 0.6f || horizontal < -0.6f)
                {
                    hasDetectedSpin = false;
                }
            }
            else
            {
                // Reset the overhead state if the Ringcon is no longer overhead
                isOverhead = false;
            }
        }
    }
}