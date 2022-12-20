using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapom : Weapon
{
    public weaponType current;
    private WeaponInstance _currentWeaponInstance;

    //public PlayerInventory inv;

    [Header("Shotgun Pellet Count")]
    public int pelletCountMax;
    public int pelletCountMin;

    [Header("Spread of blast")]
    //public int spreadRadius;
    public float spreadAngle;
    List<Quaternion> pellets;
    [Header("Where bullets come out")]
    public GameObject barrelExit;

    private void Start()
    {
        _parent = this.transform.parent.gameObject;
        checkWeapon();
        //current = _currentWeaponInstance.weapontype;
    }

    public void checkWeapon()
    {
        _currentWeaponInstance = FindObjectOfType<PlayerInventory>().currnetWeapon;
        current = _currentWeaponInstance.weapontype;
        damage = _currentWeaponInstance.damage;
        reloadSpeed = _currentWeaponInstance.reloadSpeed;
        totalAmmo = _currentWeaponInstance.totalAmmo;
        magSize = _currentWeaponInstance.magSize;
        stability = _currentWeaponInstance.stability;
        ammoInMag = _currentWeaponInstance.ammoInMag;
        fireRate = _currentWeaponInstance.fireRate;
        bulletVelocity = _currentWeaponInstance.bulletVelocity;
        projectile = _currentWeaponInstance.ammo;

       //// switch (current)
      //  {
            //each value is set in scriptable object
          //  case weaponType.pistol:
           //     damage = 10;
           //     reloadSpeed = 2; 
          //      totalAmmo = 60; 
          //      magSize = 6;
          //      ammoInMag = 6;
          //      fireRate = 1;
          //      bulletVelocity = 500;
           //     break;
         //  // case weaponType.shotgun:
          //      damage = 5;
           //     reloadSpeed = 5;
           //     totalAmmo = 25;
           //     magSize = 5;
          //      ammoInMag = 5;
          //      fireRate = 1;
         //       bulletVelocity = 500;
         //       break;
         //   case weaponType.assaultRifle:
         //       damage = 10;
         //       reloadSpeed = 5;
         //       totalAmmo = 35;
         //       magSize = 20;
         //       ammoInMag = 20;
         //       fireRate = .1f;
         //       bulletVelocity = 500;
         //       break;
         //   case weaponType.DMR:
         //       damage = 10;
        //        reloadSpeed = 6;
         //       totalAmmo = 15;
         //       magSize = 3;
         //       ammoInMag = 3;
         //       fireRate = 3;
         //       bulletVelocity = 700;
         //       break;
         //   default:
         //       break;
        ////}

    } 

    public override void Shooting()
    {
        switch (current)
        {
            case weaponType.pistol:
                base.Shooting();
                
                break;
            case weaponType.shotgun:
                if (!_currentlyReloading && !_fireCoolDown)
                {
                    //establish list
                    int pelletNum = Random.Range(pelletCountMin, pelletCountMax);
                    pellets = new List<Quaternion>(pelletNum);
                    for (int a = 0; a < pelletNum; a++)
                    {
                        pellets.Add(Quaternion.Euler(Vector3.zero));
                    }

                    //fire shot
                    Debug.Log("PEW");
                    ammoInMag--;
                    //int i = 0;
                    for (int c = 0; c < pellets.Count; c++)
                    {
                        pellets[c] = Random.rotation;
                        GameObject bullet = Instantiate(projectile, barrelExit.transform.position, barrelExit.transform.rotation);
                        //set variables to bullet
                        bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, Random.rotation, spreadAngle);
                        bullet.GetComponent<Bullet>().type = type;
                        bullet.GetComponent<Bullet>().damage = damage;
                        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletVelocity);
                        c++;
                    }

                    StartCoroutine(FireCoolDown());
                    if (ammoInMag <= 0)
                    {
                        StartCoroutine(Reloading());
                    }
                }
                break;
            case weaponType.assaultRifle:
                base.Shooting();
                break;
            case weaponType.DMR:
                base.Shooting();
                break;
            case weaponType.GrenadeLauncher:
                base.Shooting();
                break;
            case weaponType.RPG:
                base.Shooting();
                break;
            case weaponType.MiniGun:
                base.Shooting();
                break;
            default:
                break;
        }

    }

}
