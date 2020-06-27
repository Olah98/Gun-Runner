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
            GameObject bullet = Instantiate(bulletPrefab, gunLoc.transform.position, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(gunLoc.transform.forward * 10, ForceMode.Impulse);
            Destroy(bullet, 3f);
        }
    }
}
