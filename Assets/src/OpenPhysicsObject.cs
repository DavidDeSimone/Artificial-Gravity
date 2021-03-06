﻿using UnityEngine;
using System;
using System.Collections;

/* 
 * This is the main class for physics acting on an untethered object
 */

public class OpenPhysicsObject : MonoBehaviour {

    public SpinningSpaceStationPhysics SpinningSpaceStationPhysics;
    public AirResistance AirResistance;
    public Jetpack Jetpack;
    public ArbitraryFPSController FPSController;

    public void FixedUpdate() {
        var forceComponent = GetComponent<ConstantForce>();
        forceComponent.force = Vector3.zero;
        forceComponent.relativeForce = Vector3.zero;

        AirResistance.Apply(gameObject);
        SpinningSpaceStationPhysics.Apply(gameObject);

        if (jetpackEnabled) {
            Jetpack.Apply();
        }
        if (fpsControllerEnabled) {
            FPSController.Apply();
        }
    }

    protected bool jetpackEnabled;
    protected bool fpsControllerEnabled;

    public void SetJetpackEnabled(bool enabled = true) {
        jetpackEnabled = enabled;
    }

    public void SetFPSControllerEnabled(bool enabled = true) {
        fpsControllerEnabled = enabled;
    }
}
