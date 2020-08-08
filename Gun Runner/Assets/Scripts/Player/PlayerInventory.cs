using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("My Gun")]
    public WeaponInstance currnetWeapon;
    [Header("Guns I am holding")]
    public WeaponInstance slot1;
    public WeaponInstance slot2;
    [Header("Cargo Weapon")]
    public WeaponInstance cargo;

    private PlayerWeapom _playerWeapon;

    private void Awake()
    {
        _playerWeapon = FindObjectOfType<PlayerWeapom>();
    }

    private void Start()
    {
        currnetWeapon = slot1;
    }
    public void swapGun()
    {
        if(currnetWeapon == slot1)
        {
            //updates ammo left in mag on slot1
            slot1.ammoInMag = _playerWeapon.ammoInMag;
            currnetWeapon = slot2;
        }
        else if (currnetWeapon == slot2)
        {
            //updates ammo left in mag on slot2
            slot2.ammoInMag = _playerWeapon.ammoInMag;
            currnetWeapon = slot1;
        }
    }

    public void SetGun1(WeaponInstance gunData)
    {
        slot1 = gunData;
        slot1.ammoInMag = slot1.magSize;
    }
    public void SetGun2(WeaponInstance gunData)
    {
        slot2 = gunData;
        slot2.ammoInMag = slot2.magSize;
    }
    public void SetCargo(WeaponInstance cargoData)
    {
        cargo = cargoData;
        cargo.ammoInMag = cargo.magSize;
    }
}
