using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;
    public float orbitalSpeed;
    public float moonRadius;
    float currentAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(moonRadius, orbitalSpeed, planetTransform);
    }

    public void OrbitalMotion(float radius, float speed, Transform target){
        currentAngle += Time.deltaTime * speed;

        transform.position = new Vector3(target.position.x + (radius * Mathf.Sin(currentAngle * Mathf.Deg2Rad)), target.position.y + (radius * Mathf.Cos(currentAngle * Mathf.Deg2Rad)), 0);

	}
}
