using UnityEngine;
using System.Collections;

public class InputService : MonoBehaviour {

    public float RightStickX { get; private set; }
    public float RightStickY { get; private set; }
    public float LeftStickX { get; private set; }
    public float LeftStickY { get; private set; }

    public float LeftStickUp { get; private set; }
    public float LeftStickDown { get; private set; }
    public float RightTrigger { get; private set; }
    public float LeftTrigger { get; private set; }
    public float MixedTrigger { get; private set; }
    
    public float MixedTriggerButtons { get; private set; }
    public const string ACTION_BUTTON = "Action Key";

    void Start () {
        CalibrateJoystick();
    }
	
	void FixedUpdate () {

        if (Input.GetButton("Center Joystick")) {
            CalibrateJoystick();
        }

        // Collect joystick input
        RightStickX = Input.GetAxis("Roll") - xCal;
        RightStickY = Input.GetAxis("Pitch") - yCal;
        LeftStickX = Input.GetAxis("Yaw") - zCal;

        //float joyVerticalThrust = Input.GetAxis("Thrust Vertical") - tvCal;
        LeftStickUp = Input.GetAxis("Thrust Fwd") - tFwdCal;
        LeftStickDown = Input.GetAxis("Thrust Rev") - tRevCal;
        RightTrigger = Input.GetAxis("Thrust Up");
        LeftTrigger = Input.GetAxis("Thrust Down");
        MixedTrigger = (RightTrigger - LeftTrigger) / 2;

        LeftStickY = Input.GetAxis("Thrust") - tCal;
        MixedTriggerButtons = 0;

        // Linear thrust keyboard input
        MixedTriggerButtons =     applyButtonToAxis(MixedTriggerButtons, "Thrust Left", -1);
        MixedTriggerButtons =     applyButtonToAxis(MixedTriggerButtons, "Thrust Right", 1);
        MixedTrigger = applyButtonToAxis(MixedTrigger, "Thrust Up", 1);
        MixedTrigger = applyButtonToAxis(MixedTrigger, "Thrust Down", -1);
        LeftStickY =         applyButtonToAxis(LeftStickY, "Thrust Fwd", 1);
        LeftStickY =         applyButtonToAxis(LeftStickY, "Thrust Rev", -1);

        // Attitude keyboard input
        RightStickX =              applyButtonToAxis(RightStickX, "Roll Right", -1);
        RightStickX =              applyButtonToAxis(RightStickX, "Roll Left", 1);
        RightStickY =              applyButtonToAxis(RightStickY, "Pitch Down", 1);
        RightStickY =              applyButtonToAxis(RightStickY, "Pitch Up", -1);
        LeftStickX =              applyButtonToAxis(LeftStickX, "Yaw Left", -1);
        LeftStickX =              applyButtonToAxis(LeftStickX, "Yaw Right", 1);
    }

    protected float xCal = 0;
    protected float yCal = 0;
    protected float zCal = 0;
    protected float tvCal = 0;
    protected float tCal = 0;
    protected float tFwdCal = 0;
    protected float tRevCal = 0;

    void CalibrateJoystick() {
        xCal = Input.GetAxis("Roll");
        yCal = Input.GetAxis("Pitch");
        zCal = Input.GetAxis("Yaw");
        tvCal = Input.GetAxis("Thrust Vertical");
        tCal = Input.GetAxis("Thrust");
    }

    protected float applyButtonToAxis(float axis, string inputName, float value) {
        if (Input.GetButton(inputName)) {
            axis = value;
        }
        return axis;
    }
}

