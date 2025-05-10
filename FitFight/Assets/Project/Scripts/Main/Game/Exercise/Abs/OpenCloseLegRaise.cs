using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class OpenCloseLegRaise : ExerciseBase
    {
        private bool hasStartedExercise = false; 
        public override void Restart()
        {
            base.Restart();
            hasStartedExercise = false; 
        }

        public override void Run(Action onRep)
        {
            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
            {
                if (!hasStartedExercise)
                {
                    Debug.Log("Open legs to start exercise.");
                    hasStartedExercise = true; // Mark the exercise as started
                    counter++;
                }
                else
                {
                    Debug.Log("Action complete.");
                    if (counter >= 1)
                    {
                        onRep.Invoke();
                        counter = 0;
                    }
                }
            }
        }
    }
}