using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DotProductExercise : MonoBehaviour
{

    public float redAngle;
    public float blueAngle;

    // Update is called once per frame
    void Update()
    {
        Vector3 redVector = ComputeVectorFromAngle(redAngle);
        Vector3 blueVector = ComputeVectorFromAngle(blueAngle);

		Debug.DrawLine(Vector3.zero, redVector, Color.red);
		Debug.DrawLine(Vector3.zero, blueVector, Color.blue);

		if(Keyboard.current.spaceKey.wasPressedThisFrame){
			float dotProduct = ComputeDotProduct(redVector, blueVector);

			if(dotProduct<=float.Epsilon){
				dotProduct = 0;
			}

			Debug.Log(dotProduct);
		}
	}

	private Vector3 ComputeVectorFromAngle(float angle)
	{
		float angleInRads = angle * Mathf.Deg2Rad;

		float x = Mathf.Cos(angleInRads);
		float y = Mathf.Sin(angleInRads);

		return new Vector3(x, y, 0);
	}

	private float ComputeDotProduct(Vector3 a, Vector3 b){
		float dotProduct = a.x*b.x + a.y*b.y;
		return dotProduct;
	}


}
