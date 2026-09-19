using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquareSpawner : MonoBehaviour
{
    Vector2 mousePos;
    public float radius = 3f;
    public List<Vector2> corner1s;
    public List<Vector2> corner2s;
    public List<Vector2> corner3s;
    public List<Vector2> corner4s;

    public Color lineColour;
    public Color fadedLine;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        fadedLine = lineColour;
        fadedLine.a = 0.5f;
	}

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 scrollValue = Mouse.current.scroll.ReadValue();

		if (scrollValue.y!=0){
            radius += scrollValue.y * 0.1f;
        }

        Vector2 cornerOne = new Vector2(mousePos.x-radius, mousePos.y+radius);
        Vector2 cornerTwo = new Vector2(mousePos.x+radius, mousePos.y+radius);
        Vector2 cornerThree = new Vector2(mousePos.x+radius, mousePos.y-radius);
        Vector2 cornerFour = new Vector2(mousePos.x-radius, mousePos.y-radius);

        Debug.DrawLine(cornerOne, cornerTwo, fadedLine);
        Debug.DrawLine(cornerTwo, cornerThree, fadedLine);
        Debug.DrawLine(cornerThree, cornerFour, fadedLine);
        Debug.DrawLine(cornerFour, cornerOne, fadedLine);

		if (Mouse.current.leftButton.wasPressedThisFrame){
            corner1s.Add(cornerOne);
            corner2s.Add(cornerTwo);
            corner3s.Add(cornerThree);
            corner4s.Add(cornerFour);
        }
            for (int i = 0; i < corner1s.Count; i++)
            {
                Debug.DrawLine(corner1s[i], corner2s[i], lineColour);
                Debug.DrawLine(corner2s[i], corner3s[i], lineColour);
                Debug.DrawLine(corner3s[i], corner4s[i], lineColour);
                Debug.DrawLine(corner4s[i], corner1s[i], lineColour);
            }
    }
}
