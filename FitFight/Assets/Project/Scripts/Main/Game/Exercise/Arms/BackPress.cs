using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class BackPress : ExerciseBase
    {
        private float detectionThreshold = -1f; // Threshold for upside-down orientation
        private bool isHeavyPressing = false;   
        
        public override void Restart()
        {
            base.Restart();
            detectionThreshold = -1f; // Threshold for upside-down orientation
            isHeavyPressing = false;   
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");


            // Detect heavy press (right trigger)
            if (Input.GetKeyDown(KeyCode.JoystickButton4)) // Adjust if "TriggerAxis" is different for your setup
            {
                isHeavyPressing = true;
            }
            else
            {
                isHeavyPressing = false;
            }

            // Check if the Ringcon is upside-down
            bool isUpsideDown = vertical <= detectionThreshold;

            // Detect BackPress when both conditions are met
            if (isHeavyPressing && isUpsideDown)
            {
                Debug.Log("BackPress detected");
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