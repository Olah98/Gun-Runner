using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyDumDum : MonoBehaviour
{
    /// <summary>
    /// Player Test script
    /// Alex Olah
    /// 
    /// Updated June 15, 2020
    /// 
    /// - testing values, death and basic UI integration
    /// 
    /// </summary>


    public int maxHealth = 100;
    public int currentHealth;

    //HealthBar is the exact name of the script we are looking for and referenceing.
    //Allows us to access the functions within HealthBar
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        void TakeDamage(int damage)
        {
            currentHealth -= damage;

            healthBar.SetHealth(currentHealth);
        }
    }
}
