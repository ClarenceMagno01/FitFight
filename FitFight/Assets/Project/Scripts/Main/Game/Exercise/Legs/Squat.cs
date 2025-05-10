using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Legs
{
    public class Squat : ExerciseBase
    {
        public override void Run(Action onRep)
        {
            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
            {
                Debug.Log("Squat detected.");
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