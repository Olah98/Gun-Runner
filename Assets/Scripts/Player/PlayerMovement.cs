using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    Vector3 movement;

    //Takes reference to the main camera in the scene
    public Camera cam;

    //A test Vector, just to show the mouses position on screen
    public Vector3 mousePos;
    //TEST LINE, POSSIBLY DELETE
    public TestGunControlMerge currentGun;

   

    // Update is called once per frame
    void Update()
    {
        //Player's movement, takes the Vector3 movement x and z value and assigns them
        movement.x = Input.GetAxisRaw("Horizontal");

        movement.z = Input.GetAxisRaw("Vertical");

        //These 3 lines of code have it so the player points to wherever the mouse is on screen.
        Vector2 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);


        //TEST LINES, POSSIBLY DELETE
        if (Input.GetButton("Fire1"))
        {
            currentGun.isFiring = true;
            
        }
        else
        {
            currentGun.isFiring = false;
        }

        



    }

    void FixedUpdate()
    {
        //Controls the player movement
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        //Meant to help prevent player drift when colliding with an object
        rb.angularVelocity = Vector3.zero;

      
    }
}
