using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	
	

    public LayerMask platform;
    private bool isGrounded;
    private Rigidbody2D playerBody;
    public float jumpForce;
    public float maxSpeed, acceleration, deceleration, groundDecel, airDecel;
    private bool isDescending = false;
    public bool isMoving;
    public float lastVelocity;

    public Vector2 velocity;

    public enum FacingDirection
    {
        left, right
    }

    void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //creates and resets the vector each frame
        Vector2 playerInput = new Vector2();

        //if the player has pressed up or space, and is on the ground
        if((Keyboard.current.wKey.wasPressedThisFrame||Keyboard.current.spaceKey.wasPressedThisFrame)
        &&Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, .5f, platform)){
            //add a specified amount of force upwards
            playerBody.AddForce(new Vector2(0, jumpForce));
            
        }

        if(Keyboard.current.sKey.wasPressedThisFrame&&!Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, .5f, platform)&&!isDescending){
            playerBody.AddForce(new Vector2(0, -400));
            isDescending = true;
        }

        //if the player tries to move left
        if(Keyboard.current.aKey.isPressed){
            playerInput += Vector2.left;
        }

        //if the player tries to move right
        if(Keyboard.current.dKey.isPressed){
            playerInput += Vector2.right;
		}

        if(Keyboard.current.aKey.wasReleasedThisFrame|| Keyboard.current.dKey.wasReleasedThisFrame)
		{
			airDecel = velocity.sqrMagnitude *0.3f;
		}

        if(playerInput.sqrMagnitude>0){
            isMoving = true;
        }
        else{
            isMoving = false;
        }

        if(velocity.sqrMagnitude!=0){
            lastVelocity = velocity.x;
        }

        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.

        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        
    if(Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, .5f, platform)){
            deceleration = groundDecel;
            isDescending = false;
    }
    else{
            deceleration = airDecel;
    }

        //if any input is detected
		if(playerInput.sqrMagnitude>0){
            

            if(velocity.sqrMagnitude>maxSpeed*maxSpeed){
                velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
            }
            else{
                velocity += acceleration * Time.deltaTime * playerInput.normalized;
            }
        } 
        else{
            float deltaVelocity = deceleration * Time.deltaTime;
			if (velocity.magnitude > deltaVelocity)
			{ // if we still need to slow down
				velocity -= deltaVelocity * velocity.normalized; //decelerate
			}
			else
			{ //close enough to stop
				velocity = Vector3.zero;// complete stop
			}
		}

		transform.position += Time.deltaTime * (Vector3)velocity;
	}

    public bool IsWalking()
    {
        return isMoving;
    }
    public bool IsGrounded()
    {
	    return (bool)Physics2D.BoxCast(transform.position, transform.localScale, 0f, Vector2.down, .5f, platform);
    }

    public FacingDirection GetFacingDirection()
    {
       if(lastVelocity>0){
			return FacingDirection.right;
		}
        else{
			return FacingDirection.left;
		}
    }
}
