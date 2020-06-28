using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum shootType
{
    player,
    enemy
}

public class Bullet : MonoBehaviour
{

    public GameObject hitEffect;
    public int damage = 5;
    public shootType type;

    private void OnCollisionEnter(Collision collision)
    {
        //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
       // Destroy(effect, 5f);
      
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //enemy bullets wont destory when it hits who shot it
        if(type == shootType.enemy)
        {
            if(other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "Weapon")
            {
                Destroy(gameObject);
            }
        }

        //player bullets wont hit player who shot it
        if (type == shootType.player)
        {
            if(other.tag != "Player" && other.tag != "Bullet" && other.tag != "Weapon")
            {
                Destroy(gameObject);
            }
        }
    }
}
