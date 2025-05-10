using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class OverheadSideBend : ExerciseBase
    {
        private bool isOverhead = false;         // Flag for overhead position detection
        private bool hasBendedLeft = false;     // Flag to detect a left bend
        private bool hasBendedRight = false;    // Flag to detect a right bend
        private float sideBendThreshold = 0.3f; 
        
        public override void Restart()
        {
            base.Restart();
            isOverhead = false;         // Flag for overhead position detection
            hasBendedLeft = false;     // Flag to detect a left bend
            hasBendedRight = false;    // Flag to detect a right bend
            sideBendThreshold = 0.3f; 
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the Ringcon is in the overhead position (vertical == -1)
            if (Mathf.Approximately(vertical, -1f))
            {
                if (!isOverhead)
                {
                    Debug.Log("Ringcon is now overhead.");
                    isOverhead = true;
                    ResetBendFlags(); // Reset side bend flags
                }

                // Detect left side bend
                if (horizontal <= -sideBendThreshold && !hasBendedLeft)
                {
                    Debug.Log("Overhead Side Bend to the Left detected.");
                    hasBendedLeft = true;
                    hasBendedRight = false; // Reset opposite bend
                    counter++;
                }

                // Detect right side bend
                if (horizontal >= sideBendThreshold && !hasBendedRight)
                {
                    Debug.Log("Overhead Side Bend to the Right detected.");
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
                // Reset the overhead state if the Ringcon is no longer overhead
                if (isOverhead)
                {
                    Debug.Log("Ringcon is no longer overhead.");
                    isOverhead = false;
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