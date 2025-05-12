using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class OverheadArmTwist : ExerciseBase
    {
        private bool isOverhead = false;            // Flag for overhead position detection
        private bool hasDetectedArmTwist = false; 
        
        public override void Restart()
        {
            base.Restart();
            isOverhead = false;            // Flag for overhead position detection
            hasDetectedArmTwist = false; 
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Check if the Ringcon is in the overhead position (vertical == 1)
            if (Mathf.Approximately(vertical, 1f))
            {
                if (!isOverhead) // Log only on transition to overhead
                {
                    Debug.Log("Half Twist.");
                    isOverhead = true;
                    hasDetectedArmTwist = false; // Reset the twist detection flag
                }
            }


            // Check if the Ringcon has twisted downward (vertical == -1)
            if (isOverhead && Mathf.Approximately(vertical, -1f))
            {
                if (!hasDetectedArmTwist)
                {
                    Debug.Log($"Overhead Arm Twist detected.");
                    hasDetectedArmTwist = true; // Prevent repeated detection until reset
                    isOverhead = false;
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