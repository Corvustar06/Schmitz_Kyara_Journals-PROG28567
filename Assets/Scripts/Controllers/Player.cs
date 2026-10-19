using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Processors;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

	#region Variables
	public Transform enemyTransform;
    public GameObject bombPrefab;
    public GameObject powerupPrefab;
    public List<Transform> asteroidTransforms;


    public Vector2 testVector;
    public int numberOfBombs = 5;

    float bombSpacing = 1f;

    public Transform warpPosition;
    public float warpRatio;

    public float maxSpeed=1f;
    public float accelerationTime = 1f;
    public float decelerationTime = 1f;

    private float acceleration;
    private float deceleration;

    private Vector3 velocity = Vector3.zero;

    public float angularSpeed = 90f;


    public int radPointNum;
    public float radarRadius;

    public float powerupRadius;
    public int powerupNum;

    public float bombRadius;
    public float bombOrbitSpeed;


    public List<Transform> asteroids;
    public List<GameObject> orbitingBombs;
    public List<Vector3> bombPositions;

    public Vector3 playerMovementDirection;

    public bool[] inRange;

    public bool isBoosted = false;
    public bool bombsOrbiting = false;

    public float timer;
    public float currentTime;
    public float previousTime;


    bool shieldSpawned = false;
    public int shieldPointNum;
    float shieldDegreeDistance;
    public float shieldRadius;
    List<float> shieldPointAngles;
    List<Vector2> shieldPoints;
    public float shieldSpeed;
    public int loops;
	#endregion

	void Start(){
        acceleration = maxSpeed / accelerationTime; //a = delta v / delta t
        deceleration = maxSpeed / decelerationTime;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if(Keyboard.current.spaceKey.wasPressedThisFrame){
            Debug.Log("The noramlized vector is :  " + NormalizeVector(testVector));
        }

        
        if(Keyboard.current.wKey.wasPressedThisFrame){
            WarpPlayer(enemyTransform, warpRatio);
        }

        if(Keyboard.current.pKey.wasPressedThisFrame){
            SpawnPowerups(powerupRadius, powerupNum);
        }

        if(Keyboard.current.fKey.wasPressedThisFrame){
            boostPlayer(enemyTransform);
        }
        if(Keyboard.current.qKey.wasPressedThisFrame){
            shieldSpawned = true;
			shieldPoints = new List<Vector2>();
            shieldPointAngles = new List<float>();
			shieldDegreeDistance = (360 / 3) / shieldPointNum;
			for (int i = 0; i < shieldPointNum; i++)
			{
				shieldPoints.Add(new Vector2(transform.position.x + (shieldRadius * Mathf.Sin((shieldDegreeDistance * i) * Mathf.Deg2Rad)), transform.position.y + (shieldRadius * Mathf.Cos((shieldDegreeDistance * i) * Mathf.Deg2Rad))));
                shieldPointAngles.Add(shieldDegreeDistance * i);
			}
		}

        //DetectAsteroids(maxRange, asteroids);

        //PlayerMovement();
        InClassPlayerMovement();
        PlayerRotation();

        SpawningBombs();

        EnemyRadar(radarRadius, radPointNum);

        if (shieldSpawned)
        {
            spawnShield();
        }
        

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

    public void spawnOrbitingBombs(int bombNum){
        if(bombsOrbiting){

           for(int i=0;i<orbitingBombs.Count;i++){
                Bomb currentBomb = orbitingBombs[i].GetComponent<Bomb>();
                currentBomb.velocity = new Vector3((orbitingBombs[i].transform.position.x - bombPositions[i].x) / (currentTime - previousTime), (orbitingBombs[i].transform.position.y - bombPositions[i].y) / (currentTime - previousTime),0);
           }
            bombsOrbiting = false;
        }
        else{
            bombsOrbiting = true;
            timer = 0;
            currentTime = 0;
            orbitingBombs = new List<GameObject>();
            bombPositions = new List<Vector3>();
        }
    }
    public void orbitBombs(float radius, int bombNum){
		float cycleTime = 360 / bombOrbitSpeed;
		float timeBetweenBombs = cycleTime / bombNum;
        Vector3 bombSpawnPosition = new Vector3(0, radius, 0);
        GameObject spawnedBomb;
        if(timer<=0){
            if(orbitingBombs.Count<bombNum){
				spawnedBomb = Instantiate(bombPrefab, bombSpawnPosition, Quaternion.identity);
				orbitingBombs.Add(spawnedBomb);
                bombPositions.Add(bombSpawnPosition);
				timer = timeBetweenBombs;
			}
            else{
                timer = 0;
            }
        }

        timer -= Time.deltaTime;
        previousTime = currentTime;
        currentTime += Time.deltaTime;

        for(int i=0;i<orbitingBombs.Count;i++){
            Bomb currentBomb = orbitingBombs[i].GetComponent<Bomb>();
            //This is where I struggled
            //I was trying to use ATan2 to find the angle of each bomb in each frame of each loop, and I didn't understand the math
            //Instead, I just gave each bomb prefab it's own angle that they track themselves

            currentBomb.currentAngle += bombOrbitSpeed * Time.deltaTime;

			bombPositions[i] = orbitingBombs[i].transform.position;
			orbitingBombs[i].transform.position = new Vector3(transform.position.x + (radius * Mathf.Sin(currentBomb.currentAngle * Mathf.Deg2Rad)), transform.position.y + (radius * Mathf.Cos(currentBomb.currentAngle * Mathf.Deg2Rad)), 0);

		}
        
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
        if(Keyboard.current.oKey.wasPressedThisFrame){
            spawnOrbitingBombs(numberOfBombs);
        }

        if(bombsOrbiting){
            orbitBombs(bombRadius, numberOfBombs);
        }

	}

	#endregion

	#region Movement
	public void PlayerMovement(){
        Vector3 playerInput = Vector3.zero;
        Keyboard current = Keyboard.current;
        if(current.upArrowKey.isPressed&&current.downArrowKey.isPressed){
            playerMovementDirection.y = 0;
        }
        else if(current.upArrowKey.isPressed){
            playerMovementDirection.y = maxSpeed;
        }
        else if(current.downArrowKey.isPressed){
            playerMovementDirection.y = -maxSpeed;
        }
        else{
        playerMovementDirection.y = 0;
        }

        if (current.leftArrowKey.isPressed && current.rightArrowKey.isPressed)
        {
            playerMovementDirection.x = 0;
        }
        else if (current.leftArrowKey.isPressed)
        {
            playerMovementDirection.x = -maxSpeed;
        }
        else if (current.rightArrowKey.isPressed)
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
        Vector3 playerInput = Vector3.zero;
        Keyboard current = Keyboard.current;
        if (current.upArrowKey.isPressed||current.wKey.isPressed)
        {
            //velocity += Time.deltaTime*acceleration* Vector3.up;
            playerInput += Vector3.up;
        }
        if (current.downArrowKey.isPressed||current.sKey.isPressed)
        {
			//velocity += Time.deltaTime * acceleration * Vector3.down;
            playerInput += Vector3.down;
		}
        if (current.leftArrowKey.isPressed||current.aKey.isPressed)
        {
			//velocity += Time.deltaTime * acceleration * Vector3.left;
            playerInput += Vector3.left;
		}
        if (current.rightArrowKey.isPressed||current.dKey.isPressed)
        {
			//velocity += Time.deltaTime * acceleration * Vector3.right;
            playerInput += Vector3.right;
		}


        if(playerInput.sqrMagnitude > 0){ //some input detected
            velocity += acceleration * Time.deltaTime * playerInput.normalized;//accelerate relative to input

            if(velocity.sqrMagnitude> maxSpeed* maxSpeed){ //going too fast
                if(isBoosted){ // deceleration for if it was just boosted
					float deltaVelocity = deceleration * Time.deltaTime; // deceleration this frame
					if (velocity.magnitude > maxSpeed)
					{ // if we still need to slow down
						velocity -= deltaVelocity * velocity.normalized; //decelerate
					}
					else
					{ //close enough to stop
                        isBoosted = false;
					}
				}
                else{
					velocity = Vector3.ClampMagnitude(velocity, maxSpeed); // enforce the speed limit
				}
                
            }
        }
        else{// no input detected
            float deltaVelocity = deceleration * Time.deltaTime; // deceleration this frame
            if(velocity.magnitude>deltaVelocity){ // if we still need to slow down
                velocity -= deltaVelocity * velocity.normalized; //decelerate
            }
            else{ //close enough to stop
                velocity = Vector3.zero;// complete stop
            }
        }

		#region Commented Out
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
        #endregion
		

        transform.position += Time.deltaTime*velocity; // move relative to velocity
    }

    public void PlayerRotation(){
        if (velocity.sqrMagnitude == 0) return; //not moving, do nothing

        float deltaAngle = ComputeShortestAngle(transform.up, velocity);

        if (deltaAngle == 0) return; // locked-on, do nothing

        float rotationDirection = Mathf.Sign(deltaAngle); //sign function returns either +1 or -1
        float rotationStep = angularSpeed * Time.deltaTime;

        if(rotationStep< Mathf.Abs(deltaAngle)){ // do we still need to rotate --------- Abs makes it ignore the - sign
            transform.Rotate(0, 0, rotationStep * rotationDirection); //rotate relative to direction
        }
        else{ // close enough to locked-on
            transform.Rotate(0, 0, deltaAngle); //lock-on
        }
    }

    private float ComputeShortestAngle(Vector3 a, Vector3 b){
        float angleA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;

        return Mathf.DeltaAngle(angleA, angleB);
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

    public void boostPlayer(Transform target){

        isBoosted = true;

        Vector2 enemyDirection = new Vector2((target.position.x - transform.position.x), (target.position.y - transform.position.y));

        velocity = enemyDirection.normalized * (maxSpeed * 2.5f);
    }

	#endregion

    public void EnemyRadar(float radius, int circlePoints){
        Vector3[] radarPoints = new Vector3[circlePoints];
        float pointDistance = 360 / circlePoints;
        float currentAngle=0;
        Color radarColour;


        for(int i=0;i<radarPoints.Length;i++){
            currentAngle += pointDistance;
            radarPoints[i] = new Vector3(transform.position.x+(radius*Mathf.Sin(currentAngle *Mathf.Deg2Rad)), transform.position.y+(radius*Mathf.Cos(currentAngle *Mathf.Deg2Rad)), 0);
        }


        if(Vector2.Distance(enemyTransform.position, transform.position)<=radius){
            radarColour = Color.red;
        }
        else{
            radarColour = Color.green;
        }


        for (int i = 0; i < radarPoints.Length; i++)
        {
            
            if (i == radarPoints.Length - 1)
            {
                Debug.DrawLine(radarPoints[i], radarPoints[0], radarColour);
            }
            else{
				Debug.DrawLine(radarPoints[i], radarPoints[i + 1], radarColour);
			}
        }
    }


    public void SpawnPowerups(float radius, int numberOfPowerups){
        Vector3[] powerups = new Vector3[numberOfPowerups];
        float powerupDistance = 360 / numberOfPowerups;
        float currentAngle = 0;
        GameObject powerUp;

		for (int i = 0; i < powerups.Length; i++)
		{
			currentAngle += powerupDistance;
			powerups[i] = new Vector3(transform.position.x + (radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad)), transform.position.y + (radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad)), 0);
            powerUp = Instantiate(powerupPrefab, powerups[i], transform.rotation);
		}


	}

	public Vector2 NormalizeVector(Vector2 abnormalVector)
	{
		float magnitude = Mathf.Sqrt(Mathf.Pow(abnormalVector.x, 2) + Mathf.Pow(abnormalVector.y, 2));
		return (new Vector2(abnormalVector.x / magnitude, abnormalVector.y / magnitude));
	}

	public void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids)
	{
		float magnitude;
		for (int i = 0; i < inAsteroids.Count; i++)
		{
			magnitude = (transform.position + inAsteroids[i].position).magnitude;
			if (magnitude < inMaxRange)
			{
				inRange[i] = true;
			}
			else
			{
				inRange[i] = false;
			}
		}
		//total mag of 2.5
	}

    public void spawnShield(){

        for(int i=0;i<shieldPoints.Count;i++){
            shieldPointAngles[i] += shieldSpeed * Time.deltaTime;
            shieldPoints[i] = new Vector2(transform.position.x + (shieldRadius * Mathf.Sin((shieldPointAngles[i]) * Mathf.Deg2Rad)), transform.position.y + (shieldRadius * Mathf.Cos((shieldPointAngles[i]) * Mathf.Deg2Rad)));
			if (i != shieldPoints.Count - 1)
			{
				Debug.DrawLine(shieldPoints[i], shieldPoints[i + 1], Color.cyan);
			}
		}

        if(loops==0){
            if(shieldPointAngles[0]>=360){
                loops = 1;
            }
        }
        else if(loops==1){
            if(shieldPointAngles[0]>=720){
                loops = 2;
            }
        }
        else if(loops==2){
            if(shieldPointAngles[0]>=1080){
                loops = 3;
            }
        }
        else if(loops==3){
            shieldSpawned = false;
        }

	}



}
