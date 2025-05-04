using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class KneeToChest : ExerciseBase
    {
        private bool hasSquatted = false;      // Flag to detect a squat action
        private bool hasSprinted = false;     // Flag to detect a sprint action
      
        public override void Restart()
        {
            base.Restart();
            hasSquatted = false;      
            hasSprinted = false;     
        }

        public override void Run(Action onRep)
        {
            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6) && !hasSquatted)
            {
                Debug.Log("Squat detected.");
                hasSquatted = true; // Mark squat as performed
            }

            // Detect Sprint (Right Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton9) && hasSquatted && !hasSprinted)
            {
                hasSprinted = true; // Mark sprint as performed
                Debug.Log("Knee-to-Chest action completed!");
                ResetSequence(); // Reset for the next sequence
                counter++;
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }
        }
        
        // Helper method to reset the sequence
        private void ResetSequence()
        {
            hasSquatted = false;
            hasSprinted = false;
        }
    }
}