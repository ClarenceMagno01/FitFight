using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Legs
{
    public class ThighPress : ExerciseBase
    {
        private bool isInRunPosition = false;
        
        public override void Restart()
        {
            base.Restart();
            isInRunPosition = false;
        }

        public override void Run(Action onRep)
        {
            // Detect Run Position (Left Thumb Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
            {
                if (!isInRunPosition)
                {
                    Debug.Log("Run position detected. Ready for Thigh Press.");
                    isInRunPosition = true; // Enter run position state
                }
            }

            // Detect Press Action (Left Shoulder Button) after Run position is detected
            if (Input.GetKeyDown(KeyCode.JoystickButton4) && isInRunPosition) // Left Shoulder Button
            {
                Debug.Log("Thigh Press detected.");
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