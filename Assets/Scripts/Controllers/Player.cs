using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public Vector2 testVector;
    int numberOfBombs = 5;

    float bombSpacing = 1f;
    float inDistance;

    public Transform warpPosition;
    public float warpRatio;
    public float maxRange;

    public float maxSpeed=3f;
    public float accelerationTime = 1f;

    private float acceleration;
    private Vector3 velocity = Vector3.zero;

    bool decelerate;


    public List<Transform> asteroids;

    public Vector3 playerMovementDirection;

    public bool[] inRange;
    //void Start(){
    //    inRange = new bool[asteroids.Count];
    //    for(int i=0; i<asteroids.Count;i++){
    //        inRange[i] = false;
    //    }
    //}

    void Start(){
        acceleration = maxSpeed / accelerationTime; //a = delta v / delta t
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(Keyboard.current.spaceKey.wasPressedThisFrame){
            Debug.Log("The noramlized vector is :  " + NormalizeVector(testVector));
        }

        
        if(Keyboard.current.wKey.wasPressedThisFrame){
            WarpPlayer(warpPosition, warpRatio);
        }

        //DetectAsteroids(maxRange, asteroids);

        //PlayerMovement();
        InClassPlayerMovement();

        SpawningBombs();
        

        for(int i=0;i<asteroids.Count;i++){
            if(inRange[i]){
                float slope = asteroids[i].position.y / asteroids[i].position.x;
                
            }
            
        }
    }

	#region BombSpawning
	public void SpawnBombAtOffSet(Vector3 inOffSet){
        Instantiate(bombPrefab, transform.position + inOffSet, transform.rotation);
    }

	public void SpawnBombTrail(int bNum, float bSpace)
	{
		for (int i = 0; i < bNum; i++)
		{
			Instantiate(bombPrefab, new Vector3(transform.position.x, transform.position.y - (bSpace * (i + 1))), Quaternion.identity);
			//has bombs spawning on the player because bSpace * i=0 when i=0.
			//aside from that one issue, everything went as planned on the first try
		}
	}

	public void SpawnBombOnRandomCorner(int inDistance)
	{
		int corner = UnityEngine.Random.Range(0, 4);
		//changed max value from 3 to 4
		Debug.Log(corner);
		Vector3 bPosition;
		bPosition.x = inDistance;
		bPosition.y = inDistance;
		bPosition.z = 0;
		//++,+-,--,-+
		// 0, 1, 2, 3
		if (corner > 1)
		{
			bPosition.x = -inDistance;
		}
		if (corner == 1 || corner == 2)
		{
			bPosition.y = -inDistance;
		}
		Debug.Log(bPosition);
		/**two and three were spawning in the same place.
         * This meant there was something wrong with y coord of 2
         * I tried making the statement into checking specifically for 1 and 2
         * I changed the math so they were equal to -bDistance
         * 
         * Answer was that I used an else if statement instead of a  reagular if statement
         */

		Instantiate(bombPrefab, transform.position + bPosition, Quaternion.identity);
		//forgot to compare bPosition to the player's transform position
		//fixed so it spawns at transform + bPosition

		//aside from those simple issues, I got through this task without much challenge.
	}

    public void SpawningBombs(){
		if (Keyboard.current.tKey.wasPressedThisFrame)
		{
			SpawnBombTrail(numberOfBombs, bombSpacing);
		}

		if (Keyboard.current.cKey.wasPressedThisFrame)
		{
			SpawnBombOnRandomCorner((int)bombSpacing);
		}
		if (Keyboard.current.bKey.wasPressedThisFrame)
		{
			SpawnBombAtOffSet(new Vector3(0, 1, 0));
		}

	}

	#endregion


	public Vector2 NormalizeVector(Vector2 abnormalVector){
        float magnitude = Mathf.Sqrt(Mathf.Pow(abnormalVector.x, 2) + Mathf.Pow(abnormalVector.y, 2));
        return(new Vector2(abnormalVector.x/magnitude, abnormalVector.y/magnitude));
    }
    
    public void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids){
        float magnitude;
        for(int i=0; i<inAsteroids.Count;i++){
            magnitude = (transform.position + inAsteroids[i].position).magnitude;
            if (magnitude < inMaxRange)
            {
                inRange[i] = true;
            }
            else{
                inRange[i] = false;
            }
        }
        //total mag of 2.5
    }


	#region Movement
	public void PlayerMovement(){
        if(Keyboard.current.upArrowKey.isPressed&&Keyboard.current.downArrowKey.isPressed){
            playerMovementDirection.y = 0;
        }
        else if(Keyboard.current.upArrowKey.isPressed){
            playerMovementDirection.y = maxSpeed;
        }
        else if(Keyboard.current.downArrowKey.isPressed){
            playerMovementDirection.y = -maxSpeed;
        }
        else{
        playerMovementDirection.y = 0;
        }

        if (Keyboard.current.leftArrowKey.isPressed && Keyboard.current.rightArrowKey.isPressed)
        {
            playerMovementDirection.x = 0;
        }
        else if (Keyboard.current.leftArrowKey.isPressed)
        {
            playerMovementDirection.x = -maxSpeed;
        }
        else if (Keyboard.current.rightArrowKey.isPressed)
        {
            playerMovementDirection.x = maxSpeed;
        }
        else{
            playerMovementDirection.x = 0;
        }

		transform.position += playerMovementDirection;
	}

    public void InClassPlayerMovement()
    {
        if (Keyboard.current.upArrowKey.isPressed)
        {
            velocity += Time.deltaTime*acceleration* Vector3.up;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
			velocity += Time.deltaTime * acceleration * Vector3.down;
		}
        if (Keyboard.current.leftArrowKey.isPressed)
        {
			velocity += Time.deltaTime * acceleration * Vector3.left;
		}
        if (Keyboard.current.rightArrowKey.isPressed)
        {
			velocity += Time.deltaTime * acceleration * Vector3.right;
		}

		//if (!Keyboard.current.upArrowKey.isPressed&&velocity.x>0)     -----------------------------------------------------------
		//{
  //          velocity -= Time.deltaTime * acceleration * Vector3.up;
		//}
		//if (!Keyboard.current.downArrowKey.isPressed&&velocity.x<0)
		//{
  //          velocity -= Time.deltaTime * acceleration * Vector3.down;
		//}
		//if (!Keyboard.current.leftArrowKey.isPressed&&velocity.y<0)
		//{
  //          velocity -= Time.deltaTime * acceleration * Vector3.left;
		//}
		//if (!Keyboard.current.rightArrowKey.isPressed&&velocity.y>0)
		//{
  //          velocity -= Time.deltaTime * acceleration * Vector3.down;
		//}

        //The issue with this is that most of these are always applicable and so they're always messing with the velocity
        //I'm pretty sure I need to use the .context function ---------------------------------------------------------------------

  //      if(decelerate){                               ____________________________________________
  //          slowPlayer();
  //      }
  //                                                    Unfished code that ruins everything somehow
		//if (velocity.magnitude>maxSpeed){
  //          velocity = maxSpeed*velocity.normalized;  --------------------------------------------
  //      }
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        transform.position += Time.deltaTime*velocity;
    }

    public void slowPlayer(){
        
    }

	public void WarpPlayer(Transform target, float ratio)
	{
		if (ratio > 1)
		{
			ratio = 1;
		}
		else if (ratio < 0)
		{
			ratio = 0;
		}
		float xDiff = (target.position.x - transform.position.x) * ratio;
		float yDiff = (target.position.y - transform.position.y) * ratio;

		transform.position = new Vector3(transform.position.x + xDiff, transform.position.y + yDiff);

		//forgot again to have the changes affect the current position rather than being outstanding
		//easy fix, no other issues

	}

	#endregion
}
