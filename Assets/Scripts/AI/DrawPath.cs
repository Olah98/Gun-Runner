using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    /// <summary>
    /// Draw Path AI Movement
    /// Dylan Loe
    /// 
    /// Updated June 15, 2020
    /// 
    /// - Patrol Path behavior
    /// </summary>

    [Header("The Color of The Path Drawn in the Editor")]
    [Header("MAKE SURE TO UNPACK THE PATH PREFAB IN SCENE")]
    public Color rayColor = Color.red;
    [Header("Do not Drag Any Objects Here, They Are automatically Input")]
    public List<Transform> pathPoints = new List<Transform>(); //changed to static to test 
    Transform[] theArray;
    [Header("Total Nodes in Path")]
    public int total;

    private void Awake()
    {
        total = pathPoints.Count;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        //Gets the path points from the children of the path object
        theArray = GetComponentsInChildren<Transform>();
        pathPoints.Clear();

        //adds the path points to the pathPoints List
        foreach (Transform pathPoint in theArray)
        {
            if (pathPoint != this.transform)
            {
                pathPoints.Add(pathPoint);
            }
        }

        //draws the lines and wire spheres in editor
        for (int i = 0; i < pathPoints.Count; i++)
        {
            Vector3 position = pathPoints[i].position;
            if (i > 0)
            {
                Vector3 previous = pathPoints[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.5f);
            }
        }
    }
}
