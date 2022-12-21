using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapom : Weapon
{
    /// <summary>
    /// Current equiped player weapon
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - 
    /// 
    /// </summary>

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
