using System.Collections.Generic;


public class InputState
{

    // Left & Right Stick
    public float HorizontalLeft = 0;
    public float VerticalLeft = 0;
    public float HorizontalRight = 0;
    public float VerticalRight = 0;

    // Triggers
    public float TriggerLeft = 0;
    public float TriggerRight = 0;

    // ABXY/ Sony Shape Buttons
    public bool ButtonNorth = false;
    public bool ButtonSouth = false;
    public bool ButtonEast = false;
    public bool ButtonWest = false;

    // Dpad
    public bool dpadUp = false;
    public bool dpadDown = false;
    public bool dpadLeft = false;
    public bool dpadRight = false;

    //Other buttons 
    public bool LeftShoulder = false;
    public bool RightShoulder = false;
    public bool StartButton = false;
    public bool SelectButton = false;
    public bool LeftStickButton = false;
    public bool RightStickButton = false;

    public override string ToString()
    {
        string output = "InputState:";

        // TO DO: 
        // Add what you need here... 

        return output;
    }
}
