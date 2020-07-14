using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMRWeapon : Weapon
{
    //has high velocity, not much different from pistol tbh
    [Header("Bullet Speed")]
    public float bulletVelocity;

    public override void Shooting()
    {
        //shoots
        if (!_currentlyReloading && !_fireCoolDown)
        {
            Debug.Log("PEW");
            ammoInMag--;
            GameObject bullet = Instantiate(projectile, this.transform.position, this.transform.rotation);
            //set variables to bullet
            bullet.GetComponent<Bullet>().type = type;
            bullet.GetComponent<Bullet>().damage = damage;
            bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * bulletVelocity);
            StartCoroutine(FireCoolDown());
            if (ammoInMag <= 0)
            {
                StartCoroutine(Reloading());
            }
        }

    }
}
