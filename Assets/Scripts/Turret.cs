using UnityEngine;

public class Turret : MonoBehaviour
{
	[Tooltip("Measured in Degrees per second.")]
	public float angularSpeed;
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float deltaAngle = ComputeShortestAngle(transform.up, directionToTarget);

        float rotateDirection = Mathf.Sign(deltaAngle);
        float angleStep = angularSpeed * Time.deltaTime;

        if (angleStep < Mathf.Abs(deltaAngle)) {
            transform.Rotate(0, 0, rotateDirection * angularSpeed * Time.deltaTime);
        }
        else {
            transform.Rotate(0, 0, deltaAngle);
        }

		

        Debug.DrawLine(transform.position, transform.position + transform.up, Color.green);

        
        float dotProduct = Vector3.Dot(transform.up, directionToTarget);

        if(dotProduct>=0){
            Debug.Log("In Front");
        }
        else{
            Debug.Log("Behind");
        }
    }

    private float ComputeShortestAngle(Vector3 a, Vector3 b){
        float angleA = Mathf.Atan2(a.y, a.x)*Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(b.y, b.x)*Mathf.Rad2Deg;

        return Mathf.DeltaAngle(angleA, angleB);
    }
}
