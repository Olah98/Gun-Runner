using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunWeapon : Weapon
{
    public int pelletCountMax;
    public int pelletCountMin;

    //public int spreadRadius;
    public float spreadAngle;
    List<Quaternion> pellets;
    public GameObject barrelExit;

    public float pelletVelocity;

    private void Awake()
    {
       
    }

    public override void Shooting()
    {
        

        //base.Shooting();
        //shoots
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
            for(int c = 0; c < pellets.Count; c++)
            //foreach(Quaternion quat in pellets)
            {
                pellets[c] = Random.rotation;
                GameObject bullet = Instantiate(projectile, barrelExit.transform.position, barrelExit.transform.rotation);
                //set variables to bullet
                bullet.transform.rotation = Quaternion.RotateTowards(bullet.transform.rotation, Random.rotation, spreadAngle);
                bullet.GetComponent<Bullet>().type = type;
                bullet.GetComponent<Bullet>().damage = damage;
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * pelletVelocity);
                c++;
            }

            StartCoroutine(FireCoolDown());
            if (ammoInMag <= 0)
            {
                StartCoroutine(Reloading());
            }
        }
    }
}
