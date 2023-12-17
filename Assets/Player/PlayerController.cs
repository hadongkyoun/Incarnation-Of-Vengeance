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
    }

    private void Movement()
    {
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
