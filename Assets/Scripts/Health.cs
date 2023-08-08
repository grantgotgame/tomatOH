using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int health = 10;
    private GameManager gameManagerScript;
    public Slider healthSlider;

    void Start()
    {
        // Locate game manager script
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();

        // Initialize max value of health slider to match max boss health
        healthSlider.maxValue = health;
    }

    // Deal damage to boss
    public void DealDamage(int damage)
    {
        if (!gameManagerScript.gameOver)
        health -= damage;
        healthSlider.value = health;

        // Win game when boss health reaches zero and player is still alive
        if (health <= 0)
        {
            gameManagerScript.GameWon();
        }
    }
}
