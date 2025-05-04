using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Abs
{
    public class FlutterKick : ExerciseBase
    {
        private bool isSquatting = false; // Tracks if the squat position is detected
        private bool isOverhead = false; // Tracks if the Ringcon is held overhead

        public override void Restart()
        {
            base.Restart();
            isSquatting = false; 
            isOverhead = false; 
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float vertical = Input.GetAxis("Vertical");

            // Check if the Ringcon is held overhead (vertical == 1)
            if (Mathf.Approximately(vertical, -1f))
            {
                if (!isOverhead)
                {
                    Debug.Log("Ringcon is now held overhead. Ready for Flutter Kicks.");
                    isOverhead = true; // Enter overhead state
                }

                // Detect Squat (Back Button)
                if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
                {
                    if (!isSquatting)
                    {
                        Debug.Log("Squat detected. Right Leg Up.");
                        isSquatting = true; // Mark as in squat position
                        counter++;
                    }
                }

                // Detect Run (Right Stick Button)
                if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
                {
                    if (isSquatting)
                    {
                        Debug.Log("Run detected. Left Leg Up.");
                        isSquatting = false; // Reset to allow alternating actions
                        counter++;
                    }
                }

                if (counter >= 2)
                {
                    onRep.Invoke();
                    counter = 0;
                }
            }
            else
            {
                // Reset overhead state if the Ringcon is no longer overhead
                if (isOverhead)
                {
                    Debug.Log("Ringcon is no longer held overhead.");
                    isOverhead = false;
                }
            }
        }
    }
}