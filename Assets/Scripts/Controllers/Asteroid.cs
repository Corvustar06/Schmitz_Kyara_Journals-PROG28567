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

    public void chooseRandomDirection()
	{ 
		//a2+b2=c2
		//a =  destination
		//b = transform.position
		//c = maxFloatDistance
		//destination2 = maxFloatDistance2-transform.position2


		float direction = (int)UnityEngine.Random.Range(0, 360);

        destination.x = transform.position.x-(maxFloatDistance * MathF.Cos(direction));
        destination.y = transform.position.y-(maxFloatDistance * MathF.Sin(direction));
    }
}
