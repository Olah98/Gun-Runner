﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/// ******In order for a weapon to be properly equiped the weapon itself (the object attached to the player or ai), must have the type set correctly. (enum)*****
///  - the gun object must be set up in the scene properly so that it is facing forward from the player as if they are holding it. 
/// 
/// PLAYER SETUP
///  - On the player gameobject the Player_controller script must have gun type (enum) set correctly as well as the weapon to be dragged in or set on the weapon gameobject variable (gameobject)
///  - the weapon currently in use is the only one that should be active in the hierarchy
///  
/// ENEMY AI SETUP
///  - On the enemy ai under the AI pathing base script, under the gun variables the gun type should be set correctly (enum)
///  - the gun prefab attacted to the enemy in the heirarchy must also be dragged into the gun gameobject (aka the equiped gun type)
/// 
/// -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/// </summary>


public enum weaponType
{
    pistol,
    shotgun,
    assaultRifle,
    DMR,
    GrenadeLauncher,
    RPG,
    MiniGun,
    none
}

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Parent weapon Behavior
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - all weapons should inherit this class
    /// 
    /// </summary>

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
    [Header("Stability")]
    public float stability;
    [Header("Bullet Speed")]
    public float bulletVelocity; //default 500

    [Header("Who shooting this bitch?")]
    public shootType type;

    public GameObject _parent;

    [Header("Must have the LaserSight prefab in the scene (Found in Particles Folder)")]
    [Header("Laser sight")]
    public bool sniper;
    public LineRenderer laserSight;
    public float laserSightRange;

    private void Update()
    {
        //if its a sniper it has a laser sight
        if(sniper)
        {
            //set line renderer start and endpoint
            laserSight.SetPosition(0, this.transform.position);
            //end point
            Vector3 forward = transform.TransformDirection(Vector3.forward) * laserSightRange;
            forward += this.transform.position;
            laserSight.SetPosition(1, forward);
        }
    }

    //shotgun will inherit weapon
    //will have spread and number of pellets (range)
    public virtual void Shooting()
    {
        //shoots
        if (!_currentlyReloading && !_fireCoolDown)
        {
            //Debug.Log("PEW");
            ammoInMag--;
            GameObject bullet = Instantiate(projectile, this.transform.position, this.transform.rotation);
            //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.GetComponent<Collider>());
            //set variables to bullet
            if (projectile.gameObject.tag == "Bullet") {

                //stability spread
                float xSpread = Random.Range(-1, 1);
                float ySpread = Random.Range(-1, 1);
                Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * stability;

                bullet.transform.rotation = Quaternion.Euler(spread) * bullet.transform.rotation;
                bullet.GetComponent<Bullet>().type = type;
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Bullet>().shooter = _parent;
             }
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bullet.transform.forward * bulletVelocity);
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
        //Debug.Log("Reloading");
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
