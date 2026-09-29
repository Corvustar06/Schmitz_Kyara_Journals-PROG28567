using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public Vector3 velocity;

    public Vector3 combinedPoints;
    private void Update()
    {
        enemyMove();
    }

    public void enemyMove(){
        velocity = player.position - transform.position;
        velocity.Normalize();

        transform.position += Time.deltaTime * velocity;
    }

}
