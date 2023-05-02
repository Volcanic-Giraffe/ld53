using UnityEngine;

public class Inputs
{
    public static bool DeployAction {
        get
        {
            var tForward = Input.GetAxis("Thrust Forward");

            var joyInput = tForward != 0;

            return Input.GetButtonDown("Jump") || Input.GetButtonDown("Accept") || Input.GetMouseButtonDown(0) || joyInput;
        }
    }
}
