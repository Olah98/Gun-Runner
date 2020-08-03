using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponInstance
{
    public WeaponInstance()
    {
        weapontype = weaponType.none;
        weaponName = "";
    }

    public string weaponName;
    public int weaponNumber;

    //variables go here
    public weaponType weapontype;
    public int damage;
    public int reloadSpeed;
    public int totalAmmo;
    public int magSize;
    public float fireRate;
    public float bulletVelocity;
    //shotgun stuff
    public int pelletCountMax;
    public int pelletCountMin;
    public float spreadAngle;
    //public float pelletVelocity;

}

[CreateAssetMenu(fileName = "Data", menuName = "ScritableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    //public weaponInfo[] weaponData;
    public WeaponInstance[] weaponData;
    //public int totalWeapons = weaponData.Length();
    public WeaponInstance FindWeapon(int weaponNum)
   {
        foreach(WeaponInstance weapon in weaponData)
        {
            if (weapon.weaponNumber == weaponNum)
                return weapon;
        }
        return null;
   }

    public WeaponInstance FindWeapon(string weaponName)
    {
        foreach (WeaponInstance weapon in weaponData)
        {
            if (weapon.weaponName == weaponName)
                return weapon;
        }
        return null;
    }
    
    



}
