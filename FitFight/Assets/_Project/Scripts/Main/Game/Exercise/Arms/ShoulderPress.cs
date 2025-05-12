using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class ShoulderPress : ExerciseBase
    {
        private bool isHeavyPressing = false;    // Flag for heavy press detection
        private bool isAtShoulderLevel = false; // Flag for shoulder level detection
        private bool hasDetectedShoulderPress = false;

        public override void Restart()
        {
            base.Restart();
            isHeavyPressing = false;    // Flag for heavy press detection
            isAtShoulderLevel = false; // Flag for shoulder level detection
            hasDetectedShoulderPress = false;
        }

        public override void Run(Action onRep)
        {
            // Get the rotation/tilt inputs from the Ringcon
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Detect heavy press (Left Shoulder Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton4)) // Left Shoulder Button
            {
                isHeavyPressing = true;
            }
            else if (Input.GetKeyUp(KeyCode.JoystickButton4))
            {
                isHeavyPressing = false;
                hasDetectedShoulderPress = false; // Allow ShoulderPress detection again
            }

            // Check if the Ringcon is held horizontally (shoulder level)
            if (horizontal == 1f || horizontal == -1f)
            {
                if (!isAtShoulderLevel)
                {
                    Debug.Log($"Ringcon is now at shoulder level. Horizontal = {horizontal}, Vertical = {vertical}");
                    isAtShoulderLevel = true;
                }
            }
            else
            {
                if (isAtShoulderLevel)
                {
                    Debug.Log($"Ringcon is no longer at shoulder level. Horizontal = {horizontal}, Vertical = {vertical}");
                    isAtShoulderLevel = false;
                }
            }

            // Detect ShoulderPress when both conditions are met
            if (isHeavyPressing && isAtShoulderLevel && !hasDetectedShoulderPress)
            {
                Debug.Log("ShoulderPress detected");
                hasDetectedShoulderPress = true; // Prevent repeated logs
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