using UnityEngine;
using UnityEngine.UI; // For UI elements

public class KneeLift : MonoBehaviour
{
    public Text feedbackText; // UI feedback for player

    private bool isLiftingLeft = false;
    private bool isLiftingRight = false;
    private int kneeLiftCount = 0; // Count of completed knee lifts

    void Update()
    {
        // Left Knee Lift (Strapcon Down - Button 12)
        if (Input.GetKeyDown(KeyCode.JoystickButton12) && !isLiftingLeft)
        {
            isLiftingLeft = true;
            kneeLiftCount++;
            feedbackText.text = $"Left Knee Up! Total: {kneeLiftCount}";
            Debug.Log($"Left Knee Lifted. Total: {kneeLiftCount}");
            ResetKneeLift();
        }

        // Right Knee Lift (Strapcon Up - Button 13)
        if (Input.GetKeyDown(KeyCode.JoystickButton13) && !isLiftingRight)
        {
            isLiftingRight = true;
            kneeLiftCount++;
            feedbackText.text = $"Right Knee Up! Total: {kneeLiftCount}";
            Debug.Log($"Right Knee Lifted. Total: {kneeLiftCount}");
            ResetKneeLift();
        }
    }

    private void ResetKneeLift()
    {
        isLiftingLeft = false;
        isLiftingRight = false;
    }
}
