using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chef : MonoBehaviour
{
    [SerializeField] private TomatoController tomatoPlayerRef;
    [SerializeField] private float attackDelay = 2f;

    private void Start()
    {
        StartCoroutine(AttacksCoroutine());
    }

    IEnumerator AttacksCoroutine()
    {
        Vector3 playerPos = tomatoPlayerRef.transform.position;

        yield return new WaitForSeconds(attackDelay);

        //lerp hand towards playerPos
    }
}
