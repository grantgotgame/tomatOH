using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    public GameObject target;
    public GameObject head;
    public GameObject forearm;
    public GameObject cleaverHandle;
    public GameObject handHolding;
    public Transform attackPosition;
    public float maxfollowTimer, maxholdTimer, maxchopTimer;
    public float followTimer, holdTimer, chopTimer;
    public float offsetX, offsetY, offsetZ;
    public float denominator;
    public bool up, stop, down;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        head.transform.forward = target.transform.position - head.transform.position;
        cleaverHandle.transform.position = handHolding.transform.position;

        if (up)
        {
            forearm.transform.forward = target.transform.position - forearm.transform.position;
            followTimer -= 1;
        }

        if (followTimer <= 0)
        {
            up = false;
            stop = true;
            followTimer = maxfollowTimer/denominator;
            attackPosition.position = target.transform.position - new Vector3(offsetX,offsetY, offsetZ);
        }

        if (stop)
        {
            holdTimer -= 1;
        }

        if (holdTimer <= 0)
        {
            stop = false;
            down = true;
            holdTimer = maxholdTimer/denominator;
        }

        if (down)
        {
            cleaverHandle.transform.position = attackPosition.position;
            chopTimer -= 1;
        }

        if (chopTimer <= 0)
        {
            down = false;
            up = true;
            denominator += 0.1f;
            chopTimer = maxchopTimer;
        }
    }
}
