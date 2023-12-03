using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator anim;

    [SerializeField] float walkSpeed;
    private float xInput;
    private bool isMoving;


    private int facingDir = 1;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorControllers();
        FlipController();
        Movement();
        Input();

    }

    private void Input()
    {
        xInput = UnityEngine.Input.GetAxisRaw("Horizontal");
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * walkSpeed, rb.velocity.y);
    }

    private void AnimatorControllers()
    {
        if (xInput != 0)
            isMoving = true;
        else
            isMoving = false;

        anim.SetBool("isMoving", isMoving);
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
}
