using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Mathematics.Geometry;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed = 1;
    public float arrivalDistance;
    public float maxFloatDistance;
    public Vector3 destination;
    public Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        chooseRandomDirection();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, destination)<arrivalDistance){
            chooseRandomDirection();
        }

        velocity = destination - transform.position;
        velocity.Normalize();
        velocity *= moveSpeed;

        transform.position += Time.deltaTime * velocity;
    }

    public void chooseRandomDirection(){
        float direction = (int)UnityEngine.Random.Range(0, 360);

        destination.x = transform.position.x-(maxFloatDistance * MathF.Cos(direction));
        destination.y = transform.position.y-(maxFloatDistance * MathF.Sin(direction));
    }
}
