using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject hitEffect;
    public int damage = 5;
    public float explosionRadius = 5;
    //public float bulletVelocity = 500;
    public shootType type;

    public Material defaultC;

    Transform[] _proxEnemies;

    public GameObject tempExplosionObj;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        //enemy rockets wont destory when it hits who shot it
        if (type == shootType.enemy)
        {

            if (other.tag != "Enemy" && other.tag != "Bullet" && other.tag != "Weapon" && other.tag != "Ignore")
            {
                Detonation();
            }

        }

        //player rcokets wont hit player who shot it
        if (type == shootType.player)
        {
            
            if (other.tag != "Player" && other.tag != "Bullet" && other.tag != "Weapon" && other.tag != "Ignore")
            {
                
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                //stop object in mid air for explsoion
                Detonation();
            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GrenadeLauncherAmmo")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
        }
    }

    void Detonation()
    {
        //will remove this when particle effect are in
        tempExplosionObj.GetComponent<MeshRenderer>().enabled = true;
        Debug.Log("BOOM");

        ///
        //PARTILE EFFECT GOES HERE
        ///

        //will check radius around this object
        //if enemy enemies are in it, they die
        _proxEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, explosionRadius));
        foreach (Transform potentialTarget in _proxEnemies)
        {
            if (potentialTarget.gameObject.tag == "Enemy")
            {
                Debug.Log(potentialTarget.gameObject.name);
                potentialTarget.gameObject.GetComponent<AIPathingBase>().health -= damage;
            }
        }

        //Destroy(this.gameObject);
        StartCoroutine(showExplosion());
        //will maybe have particle effect too!
    }

    //explosion (TEMP)
    IEnumerator showExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
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
}
