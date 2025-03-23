using UnityEngine;

public class HipLift : MonoBehaviour
{
    private bool isLifting = false; // Tracks if the hip is currently being lifted

    void Update()
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
            }
        }
    }
}
