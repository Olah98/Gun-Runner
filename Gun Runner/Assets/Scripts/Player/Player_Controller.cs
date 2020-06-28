using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //this is the main camera that fouces onto the player
    Camera playerCam;

    [Header ("Player Vars")]
    //this is the playes move speed
    public float moveSpeed;
    public GameObject gunLoc;//the location that the bullets are fired from
    public GameObject bulletPrefab;

    public GameObject weapon;

    [Header("Shooting Detection Range")]
    public float shootingDetectionRadius = 20f;
    Transform[] _proxEnemies;


    private void Awake()
    {
        playerCam = Camera.main;
        gunLoc = this.transform.GetChild(0).gameObject;
    }
    void Update()
    {
        lookAround();
        moveAround();
        shootCurGun();

    }

    void lookAround()
    {
        // this is a simple way to turn the player around so they are always faceing the thing you want 
        // takes the angle of input and rotates it to that position using basic geomtry functions
        Vector2 dir = Input.mousePosition - playerCam.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }

    void moveAround()
    {
        //for movment we are updating the vector 3 with 2 inputs 
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position = transform.position + movement * moveSpeed * Time.deltaTime;
    }

    void shootCurGun()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            weapon.GetComponent<Weapon>().Shooting();
            //GameObject bullet = Instantiate(bulletPrefab, gunLoc.transform.position, Quaternion.identity);
            //bullet.GetComponent<Bullet>().type = shootType.player;
            ShootingDetection();
            //Rigidbody rb = bullet.GetComponent<Rigidbody>();
            //rb.AddForce(gunLoc.transform.forward * 10, ForceMode.Impulse);
            //Destroy(bullet, 3f);
        }
    }


    //shooting detection
    //show range of shooting detection (mainly for debugging)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingDetectionRadius);
        
    }

    //locate possible enemy transforms from colliders found in sphere
    private Transform[] collidersToTransforms(Collider[] colliders)
    {
        Transform[] transforms = new Transform[colliders.Length];
        for (int i = 0; i < colliders.Length; i++)
        {
            transforms[i] = colliders[i].transform;
        }
        return transforms;
    }

    //any enemies in radius will be alerted to shooting
    void ShootingDetection()
    {
        _proxEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, shootingDetectionRadius));
        foreach (Transform potentialTarget in _proxEnemies)
        {
            if (potentialTarget.gameObject.tag == "Enemy")
            {
                potentialTarget.gameObject.GetComponent<AIPathingBase>().setPOI(this.transform);
            }
        }
    }
}
