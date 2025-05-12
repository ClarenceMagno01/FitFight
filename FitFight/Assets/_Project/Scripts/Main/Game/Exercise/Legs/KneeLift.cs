using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Main.Game.Exercise.Legs
{
    public class KneeLift : ExerciseBase
    {
        //public Text feedbackText;
        
        private bool isLiftingLeft = false;
        private bool isLiftingRight = false;
        private int kneeLiftCount = 0; // Count of completed knee lifts
        
        public override void Restart()
        {
            base.Restart();
            isLiftingLeft = false;
            isLiftingRight = false;
            kneeLiftCount = 0;
        }

        public override void Run(Action onRep)
        {
            // Left Knee Lift (Strapcon Down - Button 12)
            if (Input.GetKeyDown(KeyCode.JoystickButton12) && !isLiftingLeft)
            {
                isLiftingLeft = true;
                kneeLiftCount++;
                //feedbackText.text = $"Left Knee Up! Total: {kneeLiftCount}";
                Debug.Log($"Left Knee Lifted. Total: {kneeLiftCount}");
                ResetKneeLift();
                counter++;
            }

            // Right Knee Lift (Strapcon Up - Button 13)
            if (Input.GetKeyDown(KeyCode.JoystickButton13) && !isLiftingRight)
            {
                isLiftingRight = true;
                kneeLiftCount++;
                //feedbackText.text = $"Right Knee Up! Total: {kneeLiftCount}";
                Debug.Log($"Right Knee Lifted. Total: {kneeLiftCount}");
                ResetKneeLift();
                counter++;
            }
            
            if (counter >= 1)
            {
                onRep.Invoke();
                counter = 0;
            }
        }
        
        private void ResetKneeLift()
        {
            isLiftingLeft = false;
            isLiftingRight = false;
        }
    }
}