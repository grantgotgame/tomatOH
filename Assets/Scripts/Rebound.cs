using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public float reboundForce = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        var otherRigidBody = collision.rigidbody;
        if (otherRigidBody == null)
            return;

        otherRigidBody.AddForce(-collision.relativeVelocity * reboundForce, ForceMode.Impulse);
    }
}
