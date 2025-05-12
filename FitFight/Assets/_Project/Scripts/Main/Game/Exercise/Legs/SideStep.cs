using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Legs
{
    public class SideStep : ExerciseBase
    {
        //public Text feedbackText; // UI feedback for player

        private bool isSteppingLeft = false;
        private bool isSteppingRight = false;
        private bool isMovingRingUp = false;
        private bool isMovingRingDown = false;
        private int stepCount = 0; // Count of completed steps
        
        
        public override void Restart()
        {
            base.Restart();
            isSteppingLeft = false;
            isSteppingRight = false;
            isMovingRingUp = false;
            isMovingRingDown = false;
            stepCount = 0; // Count of completed steps
        }

        public override void Run(Action onRep)
        {
            float vertical = Input.GetAxis("Vertical"); // Detect Ring-Con up/down movement

            // Detect Left Step (Strapcon Left - Button 10)
            if (Input.GetKeyDown(KeyCode.JoystickButton10) && !isSteppingLeft)
            {
                isSteppingLeft = true;
                //feedbackText.text = "Stepped Left! Move the Ring-Con!";
                Debug.Log("Stepped Left.");
            }

            // Detect Right Step (Strapcon Right - Button 11)
            if (Input.GetKeyDown(KeyCode.JoystickButton11) && !isSteppingRight)
            {
                isSteppingRight = true;
                //feedbackText.text = "Stepped Right! Move the Ring-Con!";
                Debug.Log("Stepped Right.");
            }

            // Detect Ring-Con Up Movement
            if (vertical > 0.9f && (isSteppingLeft || isSteppingRight) && !isMovingRingUp)
            {
                isMovingRingUp = true;
                //feedbackText.text = "Ring-Con Up!";
                Debug.Log("Ring-Con moved up.");
            }

            // Detect Ring-Con Down Movement
            if (vertical < -0.9f && (isSteppingLeft || isSteppingRight) && !isMovingRingDown)
            {
                isMovingRingDown = true;
              //  feedbackText.text = "Ring-Con Down!";
                Debug.Log("Ring-Con moved down.");
            }

            // Count a complete rep when both step and Ring-Con movement happen
            if (isSteppingLeft || isSteppingRight)
            {
                if (isMovingRingUp && isMovingRingDown)
                {
                    stepCount++;
                   // feedbackText.text = $"Side Step Complete! Total: {stepCount}";
                    Debug.Log($"Side Step Complete. Total: {stepCount}");

                    // Reset all movement states
                    ResetStep();
                    counter++;
                }
                
                if (counter >= 1)
                {
                    onRep.Invoke();
                    counter = 0;
                }
            }
        }
        
        private void ResetStep()
        {
            isSteppingLeft = false;
            isSteppingRight = false;
            isMovingRingUp = false;
            isMovingRingDown = false;
        }
    }
}