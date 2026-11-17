using UnityEngine;

public class BlueSquareController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public LayerMask redBox;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((bool)Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, 1f, redBox));
    }
}
