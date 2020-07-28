using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncherAmmo : MonoBehaviour
{
    //if it hits an enemy it blows up
    //otherwise it runs a countdown then blows up

    //will use physics to roll balll around
    //damage player?

    public GameObject hitEffect;
    public float explosionRadius = 5;
    public float timer = 4;
    public int damage = 5;

    Transform[] _proxEnemies;
    public float counter;
    bool colorTick = true;
   

    public Material defaultC;

    public shootType type;

    private void Start()
    {
        counter = 0;
        //starts countdown
        StartCoroutine(CountDown());
    }

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter >= 2)
        {
            if(colorTick)
            {
                this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                colorTick = false;
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material.color = defaultC.color;
                colorTick = true;
            }

            counter = 0;
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(timer);
        Detonation();
    }


    //for debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

    }

    void Detonation()
    {
        Debug.Log("BOOM");
        //will check radius around this object
        //if enemy enemies are in it, they die
        _proxEnemies = collidersToTransforms(Physics.OverlapSphere(transform.position, explosionRadius));
        foreach (Transform potentialTarget in _proxEnemies)
        {
            if (potentialTarget.gameObject.tag == "Enemy")
            {
                potentialTarget.gameObject.GetComponent<AIPathingBase>().health -= damage;
            }
        }

        Destroy(this.gameObject);
        //will maybe have particle effect too!
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
