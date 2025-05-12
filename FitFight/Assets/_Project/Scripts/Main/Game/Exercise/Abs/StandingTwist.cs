using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class StandingTwist : ExerciseBase
    {
        private bool isStanding = false;        // Tracks if the user is in the standing position
        private bool hasTwistedLeft = false;    // Flag to detect a twist to the left
        private bool hasTwistedRight = false;   // Flag to detect a twist to the right

        public override void Restart()
        {
            base.Restart();
            isStanding = false;
            hasTwistedLeft = false;
            hasTwistedRight = false;
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the user is in the standing position
            if (Mathf.Approximately(vertical, 1f))
            {
                if (!isStanding)
                {
                    Debug.Log("Standing position detected.");
                    isStanding = true; // Enter standing position state
                }

                // Detect twist to the left (horizontal == -1)
                if (Mathf.Approximately(horizontal, -1f) && !hasTwistedLeft)
                {
                    Debug.Log("Standing Twist to the Left detected.");
                    hasTwistedLeft = true;
                    hasTwistedRight = false; // Reset right twist flag
                    counter++;
                }

                // Detect twist to the right (horizontal == 1)
                if (Mathf.Approximately(horizontal, 1f) && !hasTwistedRight)
                {
                    Debug.Log("Standing Twist to the Right detected.");
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
            else
            {
                if (isStanding)
                {
                    Debug.Log("Not in standing position.");
                    isStanding = false; // Exit standing position state
                }
            }
        }
    }
}