using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class LegRaise :ExerciseBase
    {
        private bool isResting = true;
        
        public override void Restart()
        {
            base.Restart();
            isResting = true;
        }

        public override void Run(Action onRep)
        {
            // Detect Squat (Back Button) for resting position
            if (Input.GetKeyDown(KeyCode.JoystickButton6) && !isResting) // Back Button
            {
                Debug.Log("Resting Position (Squat) detected.");
                isResting = true; // Switch to resting position
            }

            // Detect Run (Left Thumb Stick Button) for legs raised
            if (Input.GetKeyDown(KeyCode.JoystickButton8) && isResting) // Run Button
            {
                Debug.Log("Legs Raised (Run) detected.");
                isResting = false; // Switch to legs raised position
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