using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    public int keyAmount;

    // Start is called before the first frame update
    void Start()
    {
        keyAmount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Key")
        {
            Destroy(other.gameObject);
            keyAmount += 1;
            print("Key Get");
        }
        if (other.gameObject.tag == "Door" && keyAmount >= 1)
        {
            Destroy(other.gameObject);
            keyAmount -= 1;
            print("Door Destroyed");
        }
    }
}
