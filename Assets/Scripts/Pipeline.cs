using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipeline : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Every 0.1 seconds that the mouse is held, a new vector should be drawn between the previous point and the current one
    //when the mouse is held down, store the position as a variable
    //When the mouse is released, output the total magnitude of the line

    float timer,magnitude,xValue,yValue;

    public List<Vector2> mousePos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.isPressed){
            timer += Time.deltaTime;
            if (timer%0.1<0.01){
                mousePos.Add(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            }
        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame){ 
            for(int i = 0; i<mousePos.Count;i++){
                xValue += mousePos[i].x;
                yValue+= mousePos[i].y;
            }
            magnitude = Mathf.Sqrt((xValue*xValue)+(yValue*yValue));
            Debug.Log("The magnitude is :  " + magnitude);
        }

        for(int i=0;i+1<mousePos.Count;i++){ 
			Debug.DrawLine(mousePos[i], mousePos[i+1], Color.white);
		}
	}
}
