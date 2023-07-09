using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 5;
    private TomatoController tomatoControllerScript;

    void Start()
    {
        tomatoControllerScript = GameObject.Find("Tomato Controller").GetComponent<TomatoController>();
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        Debug.Log("health: " + health);
        if (health <= 0 && tomatoControllerScript.playerHealth > 0)
        {
            Destroy(gameObject);
            tomatoControllerScript.GameWon();
        }
    }
}
