using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum gunType
{
    pistol,
    shotgun
}

public class TestGunControlMerge : MonoBehaviour
{
  

    public gunType myType = gunType.pistol;

    public Transform firePosition;

    public GameObject bulletPrefab;

    public float bulletForce = 20f;
    public int bulletDamage;

    public bool isFiring;
    public float fireRate = 15f;
    private float nextTimeToFire = 0f;



    private void Start()
    {
        if (myType == gunType.pistol)
        {

            fireRate = 8f;
            bulletForce = 10f;
            bulletDamage = 5;


        }

        if (myType == gunType.shotgun)
        {

            fireRate = 2f;
            bulletForce = 15f;
            bulletDamage = 10;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFiring)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                //This magical math formula determines how fast we fire in seconds. 1 divided by 4 would be a fire rate of .25 seconds
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();

            }
        }
        
    }

    void Shoot()
    {
        //Creates an internal game object to refernce the bullet prefab, set it to spawn at the custom fire position. Then take the rigidbody of the bullet gameobject and adds a force to it.

        if (myType == gunType.pistol)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, Quaternion.identity);
            bullet.GetComponent<TestBullet>().damageValue = bulletDamage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePosition.forward * bulletForce, ForceMode.Impulse);
            Destroy(bullet, 3f);
        }

        if (myType == gunType.shotgun)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, Quaternion.identity);
            bullet.GetComponent<TestBullet>().damageValue = bulletDamage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(firePosition.forward * bulletForce, ForceMode.Impulse);
            Destroy(bullet, 3f);
        }



        //GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, Quaternion.identity);
        /*bullet.GetComponent<TestBullet>().damageValue = bulletDamage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePosition.forward * bulletForce, ForceMode.Impulse);
        Destroy(bullet, 3f);  */
    }
    
    

    }
