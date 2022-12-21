using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCrate : MonoBehaviour
{
    [Header("Select if designer wished to manually choose gun")]
    public bool manuallyAssignGun = false;
    [Header("Choose which gun number is assigned to this crate")]
    public int manualAssignedGunNum;
    [Space]
    [Header("Select if this is a cargo crate")]
    public bool cargoCrate;
    private int gunNumMin;
    private int gunNumMax;

    public WeaponData weaponData;

    private PlayerInventory playerBag;

    [Header("Weapon stored in crate")]
    public WeaponInstance storedWeapon;

    public GameObject weaponDropObj;
    //player must be near box, and press E
    //

    private void Start()
    {
        if(cargoCrate)
        {
            gunNumMin = 14;
            gunNumMax = 17;
        }
        else
        {
            gunNumMin = 1;
            gunNumMax = 0;//14;
        }

        playerBag = FindObjectOfType<PlayerInventory>();
        if (manuallyAssignGun)
            storedWeapon = weaponData.FindWeapon(manualAssignedGunNum);
        else
            AssignGun();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            GameObject weaponDrop = Instantiate(weaponDropObj, this.transform.position, this.transform.rotation);
            weaponDrop.GetComponent<WeaponDrop>().weapon = storedWeapon;
            if(cargoCrate)
            {
                weaponDrop.GetComponent<WeaponDrop>().cargoWeapon = true;
                other.gameObject.GetComponent<Player_Controller>().hasCargo = true;
            }
            
            Debug.Log("Picked Cargo, dropped weapon");
            Destroy(this.gameObject);
        }
    }

    void AssignGun()
    {
        int num = Random.Range(gunNumMin, gunNumMax);
        storedWeapon = weaponData.FindWeapon(num);
    }


    //UNUSED
    public void GiveGunToPlayer()
    {
        if(playerBag.slot2.weapontype == weaponType.none)
        {
            playerBag.slot2 = storedWeapon;
        }
        else
        {
            //player must pick which gun to replace
        }
    }
}
