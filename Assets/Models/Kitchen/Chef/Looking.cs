using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looking : MonoBehaviour
{
    public GameObject target;
    public GameObject head;
    public GameObject forearm;
    public GameObject cleaver;
    public GameObject handHolding;
    public bool up, down;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        head.transform.forward = target.transform.position - head.transform.position;
        cleaver.transform.position = handHolding.transform.position;
        cleaver.transform.right = target.transform.position - cleaver.transform.position;
        if (up)
        {
            forearm.transform.forward = target.transform.position - forearm.transform.position;
        }
    }
}
