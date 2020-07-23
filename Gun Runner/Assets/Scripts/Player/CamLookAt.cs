using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAt : MonoBehaviour
{

    public GameObject target;

    public float heightY, offsetZ;

    // Update is called once per frame

    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, heightY, target.transform.position.z - offsetZ);
    }
}
