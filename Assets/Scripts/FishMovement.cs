using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;
    [SerializeField] private float speed = 5f;
    private float currentSpeed;
    private Rigidbody2D rb; 
    private Animator animator;
    private Vector2 movement;
    private bool m_FacingRight = true;
    private bool stopMoving = false;
    [SerializeField] private float defaultStopTime = 1f;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = speed;
        if(spriteObject==null){
            spriteObject = gameObject;
        }
    }

    public void SetMouvement(Vector2 set)
    {
        movement = set;
    }

    void FixedUpdate()
    {
        if(stopMoving){
            movement = Vector2.zero;
        }


        //Movement
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
        //rb.velocity = movement * currentSpeed;// * Time.fixedDeltaTime;
        if(animator!=null){
            animator.SetFloat("Speed",movement.magnitude);
        }
        

        spriteObject.transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, movement));

        /*
        float directionPos = movement.x;        

        // If the input is moving the player right and the player is facing left...
		if (directionPos > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (directionPos < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
        */
    }


    private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    public void SetSpeed(float set){
        currentSpeed = set;
    }

    public float GetSpeed(){
        return currentSpeed;
    }

    public Vector3 GetMovement(){
        return movement;
    }

    public float GetInitialSpeed(){
        return speed;
    }

    public void StopMovement(bool stop = true){
        stopMoving = stop;
    }

    public void StopMovementFor(float time = -1)
    {
        if(time < 0){
            time = defaultStopTime;
        }
        StartCoroutine(StopMovementTemporarily(time));
    }

    IEnumerator StopMovementTemporarily(float time)
    {
        StopMovement(true);
        yield return new WaitForSeconds(time);
        StopMovement(false);
    }

    private void OnDisable() {
        stopMoving = false;
    }
}
