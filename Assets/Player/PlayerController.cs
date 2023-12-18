using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator Anim;

    [Header("Movement Info")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;


    private float xInput;
    private bool isMoving;
    private bool isJumping;

    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dashTime -= Time.deltaTime;


        AnimatorControllers();
        FlipController();


        checkInput();

    }
    void FixedUpdate()
    {
        Movement();
        GroundCheck();
    }




    private void GroundCheck()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void checkInput()
    {
        
        xInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (dashTime > 0) {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else
            rb.velocity = new Vector2(xInput * runSpeed, rb.velocity.y);
        
        if(isJumping)
            Jump();
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = false;
    }

    private void AnimatorControllers()
    {
        if (xInput != 0)
            isMoving = true;
        else
            isMoving = false;

        Anim.SetFloat("yVelocity", rb.velocity.y);
        Anim.SetBool("isMoving", isMoving);
        Anim.SetBool("isGrounded", isGrounded);
        Anim.SetBool("isDashing", dashTime > 0);
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FlipController()
    {
        if(rb.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if(rb.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
