using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum current
{
    slot1,
    slot2,
    cargo
}

public class PlayerInventory : MonoBehaviour
{

    

    public current currentlyEquiped = current.slot1;

    [Header("My Gun")]
    public WeaponInstance currnetWeapon;
    [Header("Guns I am holding")]
    public WeaponInstance slot1;
    public WeaponInstance slot2;
    [Header("Cargo Weapon")]
    public WeaponInstance cargo;

   

    public PlayerWeapom _playerWeapon;

    public WeaponData gunObj;
    

    private void Start()
    {
        if (FindObjectOfType<PlayerVars>().isActiveAndEnabled)
        {
            print("Found the Player's Bag");

            if (PlayerVars.Instance.slot1.weapontype == weaponType.none)
            {
                //Debug.Log("Setting weapons");
                PlayerVars.Instance.slot1 = gunObj.FindWeapon(1);
               // PlayerVars.Instance.slot2 = gunObj.FindWeapon(9);
                //temp for testing
                //PlayerVars.Instance.slot2 = gunObj.FindWeapon(17);
                print("pistol set");
            }
            SetGun1(PlayerVars.Instance.slot1);
            SetGun2(PlayerVars.Instance.slot2);

        }
        currnetWeapon = slot1;
        _playerWeapon = FindObjectOfType<PlayerWeapom>();
        //_playerWeapon.damage = 200;
        _playerWeapon.checkWeapon();
    }
    public void swapGun()
    {
        if (currnetWeapon == slot1)
        {
            //updates ammo left in mag on slot1
            currentlyEquiped = current.slot2;
            slot1.ammoInMag = _playerWeapon.ammoInMag;
            currnetWeapon = slot2;
        }
        else if (currnetWeapon == slot2)
        {
            currentlyEquiped = current.slot1;
            //updates ammo left in mag on slot2
            slot2.ammoInMag = _playerWeapon.ammoInMag;
            currnetWeapon = slot1;
        }
    }

    public void SetGun1(WeaponInstance gunData)
    {
        slot1 = gunData;
        slot1.ammoInMag = slot1.magSize;
       // _playerWeapon.checkWeapon();
    }
    public void SetGun2(WeaponInstance gunData)
    {
        slot2 = gunData;
        slot2.ammoInMag = slot2.magSize;
       // _playerWeapon.checkWeapon();
    }
    public void SetCargo(WeaponInstance cargoData)
    {
        cargo = cargoData;
        cargo.ammoInMag = cargo.magSize;
       // _playerWeapon.checkWeapon();
    }
    private void OnDestroy()
    {
        PlayerVars.Instance.slot1 = slot1;
        PlayerVars.Instance.slot2 = slot2;
    }
}
