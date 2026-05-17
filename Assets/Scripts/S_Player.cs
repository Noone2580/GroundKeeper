using UnityEngine;
using UnityEngine.InputSystem;

public class S_Player : MonoBehaviour
{
    // VARAIBLES

    // Objects
    public Rigidbody2D rb2d;
    public Animator animator;
    public SpriteRenderer spriteRen;

    // Player
    private Vector2 moveDir;
    public Vector2 moveSpeed = new Vector2(5, 1);
    public float jumpVel = 2f;
    public bool isFacingLeft { get; protected set; } = true;


    /// <summary>
    /// Triggers on Player InputAction Move and set moveDir to it's value.
    /// </summary>
    /// <param name="value"></param>
    public void OnMove(InputValue value)
    {
        moveDir = value.Get<Vector2>();
    }

    public void OnJump(InputValue value) 
    {
        rb2d.linearVelocityY = jumpVel;
    }
   

    void Update()
    {
        if (Mathf.Abs(moveDir.x) > 0.1f)
        {
            // moveDir X is positive then is moving right
            isFacingLeft = moveDir.x < 0;
            spriteRen.flipX = isFacingLeft;

            // Set move speed (horizontal) directly
            rb2d.linearVelocityX = moveDir.x * moveSpeed.x;
        }
        //animator.SetFloat("moveSpeedX", Mathf.Abs(moveDir.x));

    }


    /// <summary>
    /// Runs eveytime unity changes defalt vars
    /// </summary>
    private void OnValidate()
    {
        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRen == null)
            spriteRen = GetComponent<SpriteRenderer>();
    }
}
