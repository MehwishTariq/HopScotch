﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcCreator : MonoBehaviour
{
    LineRenderer lr;
    public GameObject player;

    float g; //force of gravity on the y axis
    StoneMovement stoneMove;

    // Number of points on the line
    public int numPoints = 10;

    // distance between those points on the line
    public float timeBetweenPoints = 0.1f;

    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;

    //direction
    public Vector3 Dir;

    private void Awake()
    {
        //player = GameObject.FindWithTag("Player");
        lr = GetComponent<LineRenderer>();
        stoneMove = GetComponent<StoneMovement>();
        g = Mathf.Abs(Physics2D.gravity.y);
    }

    [SerializeField]
    float factor;
    [SerializeField]
    Vector3 offset;
    
    [SerializeField]
    float planeYPos;
    List<Vector3> points = new List<Vector3>();
    Vector3 startingPosition;
    Plane p;
    Ray ray;

    private void Update()
    {
        //if (GetComponentInParent<PlayerMovement>().StopMovement)
        //{
        //    return;
        //}
        if (GameManager.instance.gameStarted)
        {
            points.Clear();
            startingPosition = stoneMove.ShotPoint.position;
            //Vector3 startingVelocity = stoneMove.ShotPoint.forward * stoneMove.BlastPower;
            p = new Plane(Vector3.up, planeYPos);
            
            ray = Camera.main.ScreenPointToRay(Input.mousePosition *factor);
            if (p.Raycast(ray, out float Dist) /*&& Input.GetMouseButton(1)*/ && !PlayerMovement.isHopping)
            {
                lr.positionCount = (int)numPoints;
                Dir = ray.GetPoint(Dist) + offset - player.transform.position;
                Vector3 startingVelocity = Dir * stoneMove.BlastPower;
                for (float t = 0; t < numPoints; t += timeBetweenPoints)
                {
                    Vector3 newPoint = startingPosition + t * startingVelocity;
                    newPoint.y = startingPosition.y + startingVelocity.y * t + Physics2D.gravity.y / 2 * t * t;
                    points.Add(newPoint);

                }
                lr.SetPositions(points.ToArray());
                
            }
            else
            {
                lr.positionCount = 0;
            }
        }
    }

}