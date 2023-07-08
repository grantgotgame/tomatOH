using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int health = 5;
    void Start()
    {
        
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        Debug.Log("health: " + health);
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
