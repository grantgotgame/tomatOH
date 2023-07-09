using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private Transform tomatoPlayerRef;
    [SerializeField] private float attackDelay = 2f;

    private void Start()
    {
        StartCoroutine(AttacksCoroutine());
    }

    IEnumerator AttacksCoroutine()
    {
        Vector3 playerPos = tomatoPlayerRef.position;

        yield return new WaitForSeconds(attackDelay);

        //play attack animation
    }
}
