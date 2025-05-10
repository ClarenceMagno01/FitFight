using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class SeatedForwardPress : ExerciseBase
    {
        private bool isSeated = false;
        
        public override void Restart()
        {
            base.Restart();
            isSeated = false;
        }

        public override void Run(Action onRep)
        {
            // Detect Squat (Back Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton6) && !isSeated) // Back Button
            {
                Debug.Log("Squat detected. User is now seated.");
                isSeated = true; // Mark the user as seated
            }

            // Detect Press (Left Shoulder Button) only if the user is seated
            if (Input.GetKeyDown(KeyCode.JoystickButton4) && isSeated) // Left Shoulder Button
            {
                Debug.Log("Press detected. Seated Forward Press action performed!");
                counter++;
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }

            // Reset seated state if user exits the seated position (optional)
            if (Input.GetKeyUp(KeyCode.JoystickButton6)) // Reset when the Back Button is released
            {
                Debug.Log("User has exited the seated position.");
                isSeated = false;
            }
        }
    }
}