using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class TomatoController : MonoBehaviour
{
    PlayerControl playerControl;
    private Rigidbody playerRb;
    [SerializeField] float speed;
    [SerializeField] float dashForce;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerControl = new PlayerControl();
        playerControl.PlayerInputs.Enable();
        playerControl.PlayerInputs.Dash.performed += Dash_Performed;
    }

    public void Dash_Performed(InputAction.CallbackContext context)
    {
        if(context.interaction is TapInteraction)
        {
            Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
            playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce, ForceMode.Impulse);
            Debug.Log("simple dash!");
        }
        else if(context.interaction is HoldInteraction)
        {
            Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
            playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce * 2, ForceMode.Impulse);
            Debug.Log("Super dash!");
        }


        Debug.Log(context.interaction);
    }

    void FixedUpdate()
    {
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    // Player loses health when colliding with an obstacle
    private void OnCollisionEnter(Collision collision) {
        // Check if player is colliding with an obstacle
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision!");
        }
    }

}
