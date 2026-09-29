using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;
    public int currentStar=0;

    private Vector3 currentPosition;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        if (currentStar <starTransforms.Count)
        {
            if (Vector2.Distance(currentPosition, endPosition) < 0.5) 
            {
                startPosition = endPosition;
                currentPosition = startPosition;
                endPosition = starTransforms[currentStar].position;
                currentStar += 1;
            }
        }
        else{
            currentStar = 0;
		}


            DrawConstellation();
    }

    private void DrawConstellation()
    {
      //a = v / t
      //v = d / t

        if (currentStar == 1)
        {

            velocity.x = (endPosition.x - startPosition.x) / (drawingTime + 1);
            velocity.y = (endPosition.y - startPosition.y) / (drawingTime + 1);
        }
        else
        {
            velocity.x = (endPosition.x - startPosition.x) / drawingTime;
            velocity.y = (endPosition.y - startPosition.y) / drawingTime;


        }

    currentPosition += Time.deltaTime * velocity;

        

        Debug.DrawLine(startPosition, currentPosition);
    }
}
