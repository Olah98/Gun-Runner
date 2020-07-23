using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("My Gun")]
    public weaponType currnetWeapon;
    [Header("Guns I am holding")]
    public weaponType slot1;
    public weaponType slot2;

    private void Awake()
    {
        currnetWeapon = slot1;
    }
    public void swapGun()
    {
        if(currnetWeapon == slot1)
        {
            currnetWeapon = slot2;
        }
        else if (currnetWeapon == slot2)
        {
            currnetWeapon = slot1;
        }
    }
}
