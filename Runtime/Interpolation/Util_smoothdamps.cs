﻿using UnityEngine;

partial class Util
{
    public static Vector3 SignedEulers(this Vector3 value)
    {
        for (int i = 0; i < 3; i++)
            if (value[i] > 180)
                value[i] -= 360;
        return value;
    }

    public static Vector3 UnsignedEulers(this Vector3 value)
    {
        for (int i = 0; i < 3; i++)
            value[i] %= 360;
        return value;
    }

    public static Quaternion SmoothDamp(in Quaternion current, in Quaternion target, ref Vector3 angularVelocity, in float damp, in float maxSpeed, in float deltaTime)
    {
        Vector3 eulers = (target * Quaternion.Inverse(current)).eulerAngles.SignedEulers();
        Vector3.SmoothDamp(Vector3.zero, eulers, ref angularVelocity, damp, maxSpeed, deltaTime);
        return Quaternion.Euler(deltaTime * angularVelocity) * current;
    }

    public static void SmoothDamp(this Rigidbody rigidbody, in Quaternion target, in float damp, in float maxSpeed, in float control)
    {
        Vector3 angularVelocity1 = rigidbody.angularVelocity;
        Vector3 angularVelocity2 = angularVelocity1 * Mathf.Rad2Deg;
        SmoothDamp(rigidbody.rotation, target, ref angularVelocity2, damp, maxSpeed, Time.fixedDeltaTime);
        rigidbody.AddTorque(control * (angularVelocity2 * Mathf.Deg2Rad - angularVelocity1), ForceMode.VelocityChange);
    }

    public static void SmoothDamp(this Rigidbody rigidbody, in Vector3 target, in float damp, in float maxSpeed, in float control)
    {
        Vector3 velocity = rigidbody.linearVelocity;
        Vector3.SmoothDamp(rigidbody.position, target, ref velocity, damp, maxSpeed, Time.fixedDeltaTime);
        rigidbody.AddForce(control * (velocity - rigidbody.linearVelocity), ForceMode.VelocityChange);
    }
}