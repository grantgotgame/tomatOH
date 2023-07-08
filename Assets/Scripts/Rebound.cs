using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rebound : MonoBehaviour
{
    public float reboundForce = 3f;
    [SerializeField] private ParticleSystem reboundVFX;


    private void OnCollisionEnter(Collision collision)
    {
        var otherRigidBody = collision.rigidbody;
        if (otherRigidBody == null)
            return;

        otherRigidBody.AddForce(-collision.relativeVelocity * reboundForce, ForceMode.Impulse);

        PlayReboundVFX(collision);

    }

    public void PlayReboundVFX(Collision collision)
    {
        if (reboundVFX != null)
        {
            ParticleSystem instance = Instantiate(reboundVFX, collision.transform.position, reboundVFX.transform.rotation);
            Destroy(instance.gameObject, instance.main.duration);
        }
    }
}
