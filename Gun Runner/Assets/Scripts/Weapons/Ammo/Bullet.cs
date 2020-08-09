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
    //public float bulletVelocity = 500;
    public shootType type;
    public GameObject shooter;

    private void Start()
    {
        
        //this.GetComponent<Rigidbody>().AddForce(this.transform.forward * bulletVelocity);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //enemy bullets wont destory when it hits who shot it
        if(type == shootType.enemy)
        {

            if (other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "Weapon" && other.tag != "Ignore")
            {
                Destroy(gameObject);
            }

        }

        //player bullets wont hit player who shot it
        if (type == shootType.player)
        {
            if(other.tag != "Player" && other.tag != "Bullet" && other.tag != "Weapon" && other.tag != "Ignore")
            {
                Destroy(gameObject);
            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GrenadeLauncherAmmo")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }

}
