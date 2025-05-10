using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class Plank : ExerciseBase
    {
        private bool isResting = true;
        
        public override void Restart()
        {
            base.Restart();
            isResting = true;
        }

        public override void Run(Action onRep)
        {
            // Detect Run (Left Thumb Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
            {
                if (isResting)
                {
                    Debug.Log("Currently Planking.");
                    isResting = false; // Switch to planking
                    counter++;
                }
                else
                {
                    Debug.Log("Resting Position.");
                    isResting = true; // Switch to resting
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