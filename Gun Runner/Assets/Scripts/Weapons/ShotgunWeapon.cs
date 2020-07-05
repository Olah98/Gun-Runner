using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    public int pelletCountMax;
    public int pelletCountMin;

    public int spreadRadius;

    public override void Shooting()
    {
        int pelletNum = Random.Range(pelletCountMin, pelletCountMax);
        //base.Shooting();
        //shoots
        if (!_currentlyReloading && !_fireCoolDown)
        {
            Debug.Log("PEW");
            ammoInMag--;
            for (int count = 0; count <= pelletNum; count++)
            {
                GameObject bullet = Instantiate(projectile, this.transform.position, this.transform.rotation);
                //set variables to bullet
                bullet.GetComponent<Bullet>().type = type;
                bullet.GetComponent<Bullet>().damage = damage;
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.AddForce(this.transform.forward * 10, ForceMode.Impulse);
            }
            StartCoroutine(FireCoolDown());
            if (ammoInMag <= 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }
}
