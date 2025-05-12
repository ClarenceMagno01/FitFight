using System;
using UnityEngine;

namespace _Project.Scripts.Main.Game.Exercise.Legs
{
    public class HipLift : ExerciseBase
    {
        private bool isLifting = false;
        
        public override void Restart()
        {
            base.Restart();
            isLifting = false;
        }

        public override void Run(Action onRep)
        {
            // Detect Run (Left Thumb Stick Button)
            if (Input.GetKeyDown(KeyCode.JoystickButton8)) // Run Button
            {
                if (isLifting)
                {
                    Debug.Log("Hip is lowering.");
                    isLifting = false; // Switch to lowering state
                }
                else
                {
                    Debug.Log("Hip is lifting.");
                    isLifting = true; // Switch to lifting state
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
}