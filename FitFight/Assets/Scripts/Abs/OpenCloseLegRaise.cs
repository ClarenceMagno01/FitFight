using UnityEngine;

public class OpenCloseLegRaise : MonoBehaviour
{
    private bool hasStartedExercise = false; // Tracks if the exercise has started

    void Update()
    {
        // Detect Squat (Back Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
        {
            if (!hasStartedExercise)
            {
                Debug.Log("Open legs to start exercise.");
                hasStartedExercise = true; // Mark the exercise as started
            }
            else
            {
                Debug.Log("Action complete.");
            }
        }
    }
}
