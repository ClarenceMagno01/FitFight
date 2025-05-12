using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class RussianTwist : ExerciseBase
    {
        private bool isSquatting = false;        // Tracks if the user is in the squat position
        private bool hasTwistedLeft = false;    // Flag to detect a twist to the left
        private bool hasTwistedRight = false;  
        
        public override void Restart()
        {
            base.Restart();
            isSquatting = false;        // Tracks if the user is in the squat position
            hasTwistedLeft = false;    // Flag to detect a twist to the left
            hasTwistedRight = false;  
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the user is in the squat position (detected via Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
            {
                if (!isSquatting)
                {
                    Debug.Log("Squat position detected. Ready for Russian Twists.");
                    isSquatting = true; // Enter squat position state
                }
            }

            // Detect twists only if the user is in the squat position
            if (isSquatting)
            {
                // Detect twist to the left (horizontal == -1)
                if (Mathf.Approximately(horizontal, -1f) && !hasTwistedLeft)
                {
                    Debug.Log("Russian Twist to the Left detected.");
                    hasTwistedLeft = true;
                    hasTwistedRight = false; // Reset right twist flag
                    counter++;
                }

                // Detect twist to the right (horizontal == 1)
                if (Mathf.Approximately(horizontal, 1f) && !hasTwistedRight)
                {
                    Debug.Log("Russian Twist to the Right detected.");
                    hasTwistedRight = true;
                    hasTwistedLeft = false; // Reset left twist flag
                    counter++;
                }
                
                if (counter >= 2)
                {
                    onRep.Invoke();
                    counter = 0;
                }
            }
        }
    }
}