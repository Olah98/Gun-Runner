using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Enemy Behavior
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - Enemy behavior and navigation
    /// </summary>
    
    public GameObject incomingBullet;
    public int Health;

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Checks to see if the bullet hit, then checks its current damage value
        if (other.tag == "Bullet")
        {
            Health -= other.gameObject.GetComponent<TestBullet>().damageValue;
            Destroy(other.gameObject);
        }
    }
}
