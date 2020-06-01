using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePosition;

    public GameObject bulletPrefab;

    public float bulletForce = 20f;





    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Shoot();
            
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePosition.transform.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(firePosition.forward * bulletForce, ForceMode.Impulse);
    }
}
