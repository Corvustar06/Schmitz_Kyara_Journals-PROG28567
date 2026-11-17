using UnityEngine;

public class RedSquareController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Rigidbody2D rgbd;
    void Start()
    {
        rgbd = GetComponent<Rigidbody2D>();
        rgbd.angularDamping = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
