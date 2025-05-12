using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class SeatedRingRaise : ExerciseBase
    {
        private bool isInRunPosition = false; // Tracks if the user is in the run position
        private bool isRingOverhead = false;
        
        public override void Restart()
        {
            base.Restart();
            isInRunPosition = false; // Tracks if the user is in the run position
            isRingOverhead = false;
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");

            // Detect Run Position (Left Thumb Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton8) && !isInRunPosition) // Run Button
            {
                Debug.Log("Run position detected. Ready for Seated Ring Raise.");
                isInRunPosition = true; // Mark as in run position
            }

            // Perform Ring Raise actions only if in Run position
            if (isInRunPosition)
            {
                // Detect Ringcon Overhead (vertical == -1)
                if (Mathf.Approximately(vertical, -1f) && !isRingOverhead)
                {
                    Debug.Log("Ringcon is now overhead.");
                    isRingOverhead = true; // Mark as overhead position
                }

                // Detect Ringcon Held in Front (vertical > 0)
                if (vertical > 0f && isRingOverhead)
                {
                    Debug.Log("Seated Ring Raise complete!");
                    isRingOverhead = false; // Reset to allow alternating actions
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
}