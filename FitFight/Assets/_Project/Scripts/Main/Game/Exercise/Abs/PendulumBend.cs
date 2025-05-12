using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class PendulumBend : ExerciseBase
    {
        private bool isHeldDown = false;        // Flag for downward position detection
        private bool hasBendedLeft = false;    // Flag to detect a left bend
        private bool hasBendedRight = false;   // Flag to detect a right bend
        private float sideBendThreshold = 0.6f; // Horizontal threshold for side bends

        public override void Restart()
        {
            base.Restart();
            isHeldDown = false;
            hasBendedLeft = false;
            hasBendedRight = false;
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the Ringcon is held downward (vertical == -1)
            if (Mathf.Approximately(vertical, 1f))
            {
                if (!isHeldDown)
                {
                    Debug.Log("Ringcon is now held down.");
                    isHeldDown = true;
                    ResetBendFlags(); // Reset side bend flags
                }

                // Detect left side bend
                if (horizontal >= sideBendThreshold && !hasBendedLeft)
                {
                    Debug.Log("Pendulum Bend to the Left detected.");
                    hasBendedLeft = true;
                    hasBendedRight = false; // Reset opposite bend
                    counter++;
                }

                // Detect right side bend
                if (horizontal <= -sideBendThreshold && !hasBendedRight)
                {
                    Debug.Log("Pendulum Bend to the Right detected.");
                    hasBendedRight = true;
                    hasBendedLeft = false; // Reset opposite bend
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
                // Reset the downward state if the Ringcon is no longer held down
                if (isHeldDown)
                {
                    Debug.Log("Ringcon is no longer held down.");
                    isHeldDown = false;
                    ResetBendFlags(); // Reset side bend flags
                }
            }
        }
        
        // Helper method to reset bend flags
        private void ResetBendFlags()
        {
            hasBendedLeft = false;
            hasBendedRight = false;
        }
    }

}