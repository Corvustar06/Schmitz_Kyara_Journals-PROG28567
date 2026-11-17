using UnityEngine;

public class BlueSquareController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public LayerMask redBoxLay;
    public BoxCollider2D bc2d;

    
    void Start()
    {
        bc2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log((bool)Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, .1f, redBoxLay));


        Debug.Log(Physics2D.ClosestPoint(new Vector2(0.6f,-5.5f),bc2d));
	}
}
