using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public bool grounded;
    public LayerMask whatIsGround;
    public float jumpTime;
    public float speedMultiplier;
    public float speedIncreaseMilestone;
    public Transform groundCheck;
    public float groundCheckRadius;
    public GameManager theGameManager;
    public AudioSource jumpSound;
    public AudioSource deathSound;

    private Rigidbody2D playerRigidBody;
    //private Collider2D playerCollider;
    private Animator playerAnimator;
    private float jumpTimeCounter;
    private float speedMilestoneCount;
    private float moveSpeedStore;
    private float speedMilestoneCountStore;
    private float speedIncreaseMilestoneStore;
    private bool stoppedJumping;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        //playerCollider = GetComponent<Collider2D>();
        playerAnimator = GetComponent<Animator>();

        jumpTimeCounter = jumpTime;
        speedMilestoneCount = speedIncreaseMilestone;

        moveSpeedStore = moveSpeed;
        speedMilestoneCountStore = speedMilestoneCount;
        speedIncreaseMilestoneStore = speedIncreaseMilestone;

        stoppedJumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        //grounded = Physics2D.IsTouchingLayers(playerCollider, whatIsGround);
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(transform.position.x > speedMilestoneCount) 
        {
            speedMilestoneCount += speedIncreaseMilestone;
            speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;

            moveSpeed = moveSpeed * speedMultiplier;
        }

        playerRigidBody.velocity = new Vector2(moveSpeed, playerRigidBody.velocity.y);

        //Checks if the spacebar or left click is pressed down, and whether the player is touching the ground
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
            && grounded)
        {
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
            stoppedJumping = false;
            jumpSound.Play();
        }

        //checks if the player is continuing to hold the jump button
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
            && !stoppedJumping)
        {
            if(jumpTimeCounter > 0)
            {
                playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
        {
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }

        if (grounded && jumpTimeCounter == 0)
        {
            jumpTimeCounter = jumpTime;
            stoppedJumping = true;
        }

        playerAnimator.SetFloat("Speed", playerRigidBody.velocity.x);
        playerAnimator.SetBool("Grounded", grounded);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "killbox")
        { 
            theGameManager.RestartGame();

            moveSpeed = moveSpeedStore;
            speedMilestoneCount = speedMilestoneCountStore;
            speedIncreaseMilestone = speedIncreaseMilestoneStore;
            deathSound.Play();
        }
    }

}
