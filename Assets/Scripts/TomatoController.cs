using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class TomatoController : MonoBehaviour
{
    public PlayerControl playerControl;
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

    [SerializeField] private ParticleSystem tomatoHitVFX;

    private SoundManager soundManagerScript;
    private GameManager gameManagerScript;

    void Awake()
    {
        //find scripts
        soundManagerScript = GetComponent<SoundManager>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
        //initialize player controls
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
            soundManagerScript.PlaySound(soundManagerScript.swoosh);
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
        if (collision.gameObject.CompareTag("Obstacle") && !gameManagerScript.gameOver)
        {
            PlayHitVFX();
            playerHealth -= 1;
            soundManagerScript.PlaySound(soundManagerScript.squish);

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
                gameManagerScript.GameOver();
            }
        }

        // Deal damage to enemy if speed is high enough
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
}
