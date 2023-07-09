using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private int health = 10;
    private TomatoController tomatoControllerScript;
    public Slider healthSlider;

    void Start()
    {
        // Locate tomato controller script
        tomatoControllerScript = GameObject.Find("Tomato Controller").GetComponent<TomatoController>();

        // Initialize max value of health slider to match max boss health
        healthSlider.maxValue = health;
    }

    // Deal damage to boss
    public void DealDamage(int damage)
    {
        health -= damage;
        healthSlider.value = health;

        // Win game when boss health reaches zero and player is still alive
        if (health <= 0 && tomatoControllerScript.playerHealth > 0)
        {
            tomatoControllerScript.GameWon();
        }
    }
}
