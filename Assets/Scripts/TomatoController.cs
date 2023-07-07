using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TomatoController : MonoBehaviour
{
    PlayerControl playerControl;
    private Rigidbody playerRb;
    [SerializeField] float speed;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerControl = new PlayerControl();
        playerControl.PlayerInputs.Enable();
    }

    void FixedUpdate()
    {
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
}
