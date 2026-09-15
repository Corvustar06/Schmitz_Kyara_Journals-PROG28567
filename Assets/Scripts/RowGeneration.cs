using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RowGeneration : MonoBehaviour
{
    public TMP_InputField squareNumberInput;

	public bool squaresGenerated = false;
    public Vector2 firstCorner;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(squaresGenerated){
            if (int.Parse(squareNumberInput.text) != null)
            {

                for (int i = 0; i < int.Parse(squareNumberInput.text); i++)
                {
                    Debug.DrawLine(new Vector2(firstCorner.x+i, firstCorner.y), new Vector2(firstCorner.x+i+1, firstCorner.y), Color.red);
                    Debug.DrawLine(new Vector2(firstCorner.x+i+1, firstCorner.y), new Vector2(firstCorner.x+i+1, firstCorner.y+1), Color.red);
                    Debug.DrawLine(new Vector2(firstCorner.x+i+1, firstCorner.y+1), new Vector2(firstCorner.x+i, firstCorner.y+1), Color.red);
                    Debug.DrawLine(new Vector2(firstCorner.x+i, firstCorner.y+1), new Vector2(firstCorner.x+i, firstCorner.y), Color.red);
                }
            }
        }
    }

    public void triggerSquares(){
        squaresGenerated = !squaresGenerated;
    }
}
