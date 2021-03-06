﻿using UnityEngine;
using System;
using System.Collections;

/* This was used initially to develop a theory on how to do air resistance around a spinning axis
 * Kept for historical reasons and in case more experimentation needs to be done
 */

public class RotatingAxisFriction : MonoBehaviour {

    
    public GameObject target;
    public bool clockwise = true;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ApplyForce(target);
    }

    public void ApplyForce(GameObject target) {

        // Setup variables
        float angularVelocity = GetComponent<Rigidbody>().angularVelocity.magnitude;
        Vector3 targetVector = target.transform.position;
        Vector3 targetVelocityVector = target.GetComponent<Rigidbody>().velocity;
        ConstantForce targetForce = target.GetComponent<ConstantForce>();

        // Get air vector
        Vector3 forceVectorNormalized = GetForceVectorNormalized(targetVector);
        float radius = GetDeltaVector(GetClosestPointOnAxis(targetVector), targetVector).magnitude;
        float forceMagnitude = GetLinearVelocity(angularVelocity, radius);
        Vector3 forceVector = forceVectorNormalized * forceMagnitude;

        // Apply air vector to target
        Vector3 frictionVelocityVector = forceVector - targetVelocityVector;
        targetForce.force = frictionVelocityVector.normalized * (float)Math.Pow(frictionVelocityVector.magnitude, 2);

        target.transform.LookAt(target.transform.position - frictionVelocityVector);
    }

    // Convert angular velocity and radians to linear velocity
    protected float GetLinearVelocity(float angularVelocity, float radius) {
        return angularVelocity * radius;
    }


    // Calcululate the closest point to the target that is on the axis line
    protected Vector3 GetClosestPointOnAxis(Vector3 target) {
        return new Vector3(0, target.y, 0);
    }

    // Get the difference between two vectors
    protected Vector3 GetDeltaVector(Vector3 a, Vector3 b) {
        return b - a;
    }

    // Get the vector of the moving air
    protected Vector3 GetForceVectorNormalized(Vector3 target) {
        Vector3 axisHelper = GetClosestPointOnAxis(target);
        Vector3 forceVector = Vector3.Cross(target, axisHelper).normalized;
        forceVector = clockwise ? -forceVector : forceVector;

        return forceVector;
    }
}
