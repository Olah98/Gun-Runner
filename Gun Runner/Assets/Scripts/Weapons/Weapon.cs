using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum weaponType
{
    pistol,
    shotgun,
    assaultRifle,
    DMR
}

public class Weapon : MonoBehaviour
{
    [Header("Weapon Bullet Type")]
    public GameObject projectile;
    [Header("Weapon Damage")]
    public int damage;
    [Header("Reload Speed")]
    public int reloadSpeed;
    //if weapon is currently reloading, cant fire (maybe play sound effect instead if its true for better indication)
    protected bool _currentlyReloading;
    protected bool _fireCoolDown;
    [Header("Total Ammo")]
    public int totalAmmo;
    [Header("Mag")]
    public int magSize;
    public int ammoInMag;
    [Header("Fire Rate")]
    public float fireRate;
    

    [Header("Who shooting this bitch?")]
    public shootType type;
    
    //shotgun will inherit weapon
    //will have spread and number of pellets (range)

    public virtual void Shooting()
    {
        //shoots
        if(!_currentlyReloading && !_fireCoolDown)
        {
            Debug.Log("PEW");
            ammoInMag--;
            GameObject bullet = Instantiate(projectile, this.transform.position, this.transform.rotation);
            //set variables to bullet
            bullet.GetComponent<Bullet>().type = type;
            bullet.GetComponent<Bullet>().damage = damage;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(this.transform.forward * 10, ForceMode.Impulse);
            StartCoroutine(FireCoolDown());
            if (ammoInMag <= 0)
            {
                StartCoroutine(Reloading());
            }
        }

        
    }

    public IEnumerator FireCoolDown()
    {
        _fireCoolDown = true;
        yield return new WaitForSeconds(fireRate);
        _fireCoolDown = false;
    }

    public IEnumerator Reloading()
    {
        Debug.Log("Reloading");
        _currentlyReloading = true;
        yield return new WaitForSeconds(reloadSpeed);
        if(totalAmmo != -1)
        {
            totalAmmo -= magSize;
            ammoInMag = magSize;
        }
        _currentlyReloading = false;
    }
}
