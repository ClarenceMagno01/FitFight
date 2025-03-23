<<<<<<< Updated upstream
using UnityEngine;

public class Squat : MonoBehaviour
{
    void Update()
    {
        // Detect Squat (Back Button)
        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
        {
            Debug.Log("Squat detected.");
=======
//using UnityEngine;

//public class Squat : MonoBehaviour
//{
//    void Update()
//    {
//        // Detect Squat (Back Button)
//        if (Input.GetKeyDown(KeyCode.JoystickButton6)) // Back Button
//        {
//            Debug.Log("Squat detected.");
//        }
//    }
//}

using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Squat : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isSquatting = false;
    private int squatCount = 0; // Count of completed squats

    void Update()
    {
        // Squat Detection (Back Button - Button 6)
        if (Input.GetKeyDown(KeyCode.JoystickButton6) && !isSquatting)
        {
            isSquatting = true;
            feedbackText.text = "Squatting... Hold position!";
            Debug.Log("Player is squatting.");
        }

        // Squat Completion (Strapcon Down - Button 12)
        if (isSquatting && Input.GetKeyDown(KeyCode.JoystickButton12))
        {
            isSquatting = false;
            squatCount++;
            feedbackText.text = $"Squat Complete! Total: {squatCount}";
            Debug.Log($"Squat detected. Total Squats: {squatCount}");
>>>>>>> Stashed changes
        }
    }
}
