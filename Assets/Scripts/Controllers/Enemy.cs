using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Vector3 velocity;
    public float acceleration = 1;
    float maxSpeed = 3f;

    private void Update()
    {
        enemyMove();
    }

    public void enemyMove(){
        velocity = player.position - transform.position;
        velocity.Normalize();
        if(velocity.x<0){
            velocity.x += Time.deltaTime * acceleration * -1;
            //if(velocity.x<maxSpeed*velocity.x){
            //    velocity.x = maxSpeed*velocity.x;
            //}
        }
        else{
            velocity.x += Time.deltaTime * acceleration;
			//if (velocity.x > maxSpeed * velocity.x)
			//{
			//	velocity.x = maxSpeed * velocity.x;
			//}
		}
        if (velocity.y < 0)
        {
            velocity.y += Time.deltaTime * acceleration * -1;
			//if (velocity.y < maxSpeed * velocity.y)
			//{
			//	velocity.y = maxSpeed * velocity.y;
			//}
		}
        else{
            velocity.y += Time.deltaTime * acceleration;
			//if (velocity.y > maxSpeed * velocity.y)
			//{
			//	velocity.y = maxSpeed * velocity.y;
			//}
		}


        transform.position += Time.deltaTime * velocity;
    }

}
