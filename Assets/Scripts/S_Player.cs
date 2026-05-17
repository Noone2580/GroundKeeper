using UnityEngine;
using UnityEngine.InputSystem;

public class S_Player : MonoBehaviour
{
    // VARAIBLES

    // Objects
    public CapsuleCollider2D capsuleCol;
    public Rigidbody2D rb2d;
    public Animator animator;
    public SpriteRenderer spriteRen;

    // Player
    private Vector2 moveDir;
    public Vector2 moveSpeed = new Vector2(5, 1);
    public float jumpVel = 2f;
    public bool isFacingLeft { get; protected set; } = true;


    // Physyic / raycast
    public LayerMask groundLayer;
    public bool isGrounded { get; protected set; } = true;

    Vector2 edgeClipTopOrigin = Vector2.zero;
    Vector2 edgeClipBotOrigin = Vector2.zero;
    Vector2 edgeClipDirection = Vector2.zero;
    float edgeClipRayDistance = .4f;


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
        if (isGrounded)
            rb2d.linearVelocityY = jumpVel;
    }


    void Update()
    {

        ////////////////////////////////////////////////////////////
        /// MOVEMENT

        if (Mathf.Abs(moveDir.x) > 0.1f)
        {
            // moveDir X is positive then is moving right
            isFacingLeft = moveDir.x < 0;
            spriteRen.flipX = isFacingLeft;

            // Check is player is hitting a wall
            Vector2 centre = transform.position;
            Vector2 extents = capsuleCol.bounds.extents;
            float extentsX = isFacingLeft ? -extents.x : +extents.x;

            edgeClipTopOrigin = centre + new Vector2(extentsX, extents.y);
            edgeClipBotOrigin = centre + new Vector2(extentsX, -extents.y);

            edgeClipDirection = new Vector2(extents.x, 0).normalized;
            edgeClipRayDistance *= edgeClipDirection.x;

            bool hitTop = Physics2D.Raycast(edgeClipTopOrigin, edgeClipDirection, edgeClipRayDistance, groundLayer);
            bool hitBot = Physics2D.Raycast(edgeClipBotOrigin, edgeClipDirection, edgeClipRayDistance, groundLayer);

            if(!hitBot && !hitBot) 
            {
                // Set move speed (horizontal) directly
                rb2d.linearVelocityX = moveDir.x * moveSpeed.x;
            }
            
            //fhuwihfwi

            Debug.DrawLine(edgeClipTopOrigin, edgeClipTopOrigin + new Vector2(edgeClipRayDistance,0), hitTop ? Color.red: Color.green);
            Debug.DrawLine(edgeClipBotOrigin, edgeClipBotOrigin + new Vector2(edgeClipRayDistance,0), hitBot ? Color.red: Color.green);
            
        }
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveDir.x));

        ////////////////////////////////////////////////////////////
        /// JUMP

        Vector2 rayOrigin = this.transform.position;
        Vector2 rayDir = Vector2.down;
        float rayRange = .3f;
        isGrounded = Physics2D.Raycast(rayOrigin, rayDir, rayRange, groundLayer);

        animator.SetBool("isGrounded", isGrounded);
    }


    /// <summary>
    /// Runs eveytime unity changes defalt vars
    /// </summary>
    private void OnValidate()
    {
        if (capsuleCol == null)
            capsuleCol = GetComponent<CapsuleCollider2D>();

        if (rb2d == null)
            rb2d = GetComponent<Rigidbody2D>();

        if (animator == null)
            animator = GetComponent<Animator>();

        if (spriteRen == null)
            spriteRen = GetComponent<SpriteRenderer>();
    }
}
