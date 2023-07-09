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
    public int playerHealth = 3;
    public GameObject healthImage1;
    public GameObject healthImage2;
    public GameObject healthImage3;
    public GameObject damagedImage1;
    public GameObject damagedImage2;
    public GameObject damagedImage3;
    public GameObject gameOverText;
    public GameObject winText;
    public GameObject restartButton;
    public GameObject mainMenuButton;
    private bool gameWon;
    [SerializeField] private ParticleSystem tomatoHitVFX;

    void Awake()
    {
        StopAllCoroutines();
        playerRb = GetComponent<Rigidbody>();
        playerControl = new PlayerControl();
        playerControl.PlayerInputs.Enable();
        playerControl.PlayerInputs.Dash.performed += Dash_Performed;
    }

    // Player dashes on key press
    public void Dash_Performed(InputAction.CallbackContext context)
    {
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        if (playerRb)
        {
            playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * dashForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Allow player movement
        Vector2 inputVector = playerControl.PlayerInputs.Movement.ReadValue<Vector2>();
        playerRb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }

    // Player loses health when colliding with an obstacle
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !gameWon)
        {
            PlayHitVFX();
            playerHealth -= 1;

            // Update UI to show current health
            if (playerHealth == 2)
            {
                healthImage3.SetActive(false);
                damagedImage3.SetActive(true);
            }
            else if (playerHealth == 1)
            {
                healthImage2.SetActive(false);
                damagedImage2.SetActive(true);
            }
            else if (playerHealth == 0)
            {
                healthImage1.SetActive(false);
                damagedImage1.SetActive(true);
                GameOver();
            }
        }

        // Deal damage to boss if speed is high enough
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float sqrMagnitude = collision.relativeVelocity.sqrMagnitude;

            if (sqrMagnitude > 700f)
            {
                collision.gameObject.GetComponent<Health>().DealDamage(1);
            }
        }
    }

    // Display a particle effect on collision
    public void PlayHitVFX()
    {
        if (tomatoHitVFX != null)
        {
            ParticleSystem instance = Instantiate(tomatoHitVFX, transform.position, tomatoHitVFX.transform.rotation);
            Destroy(instance.gameObject, instance.main.duration);
        }
    }

    // Display Game Over screen
    public void GameOver()
    {
        playerControl.PlayerInputs.Disable();
        gameOverText.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }

    // Display Game Won screen 
    public void GameWon()
    {
        gameWon = true;
        playerControl.PlayerInputs.Disable();
        winText.SetActive(true);
        restartButton.SetActive(true);
        mainMenuButton.SetActive(true);
    }
}
