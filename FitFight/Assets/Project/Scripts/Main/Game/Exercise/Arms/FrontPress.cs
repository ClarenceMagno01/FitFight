using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Arms
{
    public class FrontPress : ExerciseBase
    {
        private float detectionThreshold = 0.8f; // Threshold for downward orientation
        private bool isHeavyPressing = false;  // Flag for heavy press detection
        private bool isHeldDown = false;  
        
        public override void Restart()
        {
            base.Restart();
            detectionThreshold = 0.8f; // Threshold for downward orientation
            isHeavyPressing = false;  // Flag for heavy press detection
            isHeldDown = false;  
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

            // Check if the Ringcon is held down (Joycon facing downward)
            if (vertical >= detectionThreshold) // Detect if vertical axis indicates "downward"
            {
                if (!isHeldDown) // Prevent repeated logs
                {
                    Debug.Log($"Ringcon is now held down. Horizontal = {horizontal}, Vertical = {vertical}");
                    isHeldDown = true;
                }
            }
            else if (vertical == 0f) // Reset state when the Ringcon is no longer pointing downward
            {
                if (isHeldDown)
                {
                    Debug.Log($"Ringcon is no longer held down. Horizontal = {horizontal}, Vertical = {vertical}");
                    isHeldDown = false;
                }
            }

            // Detect FrontPress when both conditions are met
            if (isHeavyPressing && isHeldDown)
            {
                Debug.Log("FrontPress detected");
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