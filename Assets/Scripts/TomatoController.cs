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
    [SerializeField] int playerHealth = 3;
    public GameObject healthImage1;
    public GameObject healthImage2;
    public GameObject healthImage3;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        playerControl = new PlayerControl();
        playerControl.PlayerInputs.Enable();
        playerControl.PlayerInputs.Dash.performed += Dash_Performed;
    }

    public void Dash_Performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce, ForceMode.Impulse);

        /*
        Debug.Log(context.phase);
        if (context.interaction is TapInteraction)
        {
            Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
            playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce, ForceMode.Impulse);
            Debug.Log("simple dash!");
            Debug.Log(context.phase);
        }
        else if(context.interaction is HoldInteraction)
        {
            Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
            playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce * 2, ForceMode.Impulse);
            Debug.Log("Super dash!");
            Debug.Log(context.phase);
        }


        Debug.Log(context.interaction);*/
    }

    void FixedUpdate()
    {
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    // Player loses health when colliding with an obstacle
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerHealth -= 1;

            // Update UI to show current health
            if (playerHealth == 2)
            {
                healthImage3.SetActive(false);
            }
            else if (playerHealth == 1)
            {
                healthImage2.SetActive(false);
            }
            else if (playerHealth == 0)
            {
                healthImage1.SetActive(false);
            }
        }
    }
}
