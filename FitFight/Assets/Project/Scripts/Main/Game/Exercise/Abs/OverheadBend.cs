using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class OverheadBend : ExerciseBase
    {
        private bool isOverhead = false;            // Flag for overhead position detection
        private bool hasDetectedBend = false;  
        
        public override void Restart()
        {
            base.Restart();
            isOverhead = false;            // Flag for overhead position detection
            hasDetectedBend = false;  
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
                    isOverhead = true;
                }
                Debug.Log("Ringcon is now overhead.");
                hasDetectedBend = false; // Reset bend detection state
            }

            // Detect forward bend (vertical > 0)
            if (isOverhead && vertical > 0f)
            {
                if (!hasDetectedBend)
                {
                    Debug.Log("Overhead Bend detected.");
                    hasDetectedBend = true; // Prevent repeated detection until reset
                    counter++;
                }
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }

            // Reset overhead state when returning to neutral
            if (vertical > 0.5f && isOverhead)
            {
                isOverhead = false;
                Debug.Log("Ringcon is no longer overhead.");
            }
        }
    }
}