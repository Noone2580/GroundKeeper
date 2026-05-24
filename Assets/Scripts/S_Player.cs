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
    public Vector2 moveSpeed = new Vector2(2, 1);
    public Vector2 maxMoveSpeed = new Vector2(1,1);
    public float jumpVel = 2f;
    public bool isFacingLeft { get; protected set; } = true;
    //private bool isJumping = false;
    //private float jumpTimeRemaining;


    // Physyic / raycast
    public LayerMask groundLayer;
    public bool isGrounded { get; protected set; } = true;

    Vector2 edgeClipTopOrigin = Vector2.zero;
    Vector2 edgeClipBotOrigin = Vector2.zero;
    Vector2 edgeClipDirection = Vector2.zero;
    public float edgeClipRayDistance = .03f;
    public float edgeClipOffsetY = .02f;

    public float maxCoyoteTime = 0.100f;
    private float coyoteTimeRemaining;

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
        print("Jump");
        // Check if can jump
        if (isGrounded || coyoteTimeRemaining > 0)
            rb2d.linearVelocityY = jumpVel;
    }

    public void OnInteract(InputValue value)
    {
        Collider2D[] hits = new Collider2D[10];

        // check for overlaping objects

        capsuleCol.Overlap(new ContactFilter2D(), hits);

        // If no hit then leave
        if (hits.Length <= 0) return;

        print(hits.Length);

        // go throught hits array and find object with tag
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i] || !hits[i].enabled) continue;

            print(hits[i].name);
            if (hits[i].CompareTag("Interactables"))
            {
                hits[i].gameObject.SetActive(false);
                return;
            }

        }
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
            extents.x += .03f;
            extents.x = isFacingLeft ? -extents.x : +extents.x;
            extents.y -= edgeClipOffsetY;

            edgeClipTopOrigin = centre + new Vector2(extents.x, extents.y);
            edgeClipBotOrigin = centre + new Vector2(extents.x, -extents.y);

            edgeClipDirection = new Vector2(0, -1).normalized;
            edgeClipRayDistance = Vector2.Distance(edgeClipTopOrigin, edgeClipBotOrigin);

            float RayDis = edgeClipRayDistance * edgeClipDirection.x;

            bool hitTop = Physics2D.Raycast(edgeClipTopOrigin, edgeClipDirection, edgeClipRayDistance, groundLayer);
            //bool hitBot = Physics2D.Raycast(edgeClipBotOrigin, edgeClipDirection, edgeClipRayDistance, groundLayer);

            if (!hitTop)
            {
                if (Mathf.Abs(rb2d.linearVelocityX) < maxMoveSpeed.x)
                {
                    rb2d.AddForceX(moveSpeed.x * moveDir.x, ForceMode2D.Force);

                    // Set move speed (horizontal) directly
                    //rb2d.linearVelocityX = moveDir.x * moveSpeed.x;
                }
            }

        }
        animator.SetFloat("moveSpeedX", Mathf.Abs(moveDir.x));

        ////////////////////////////////////////////////////////////
        /// JUMP

        coyoteTimeRemaining -= Time.deltaTime;

        Vector2 rayOrigin = this.transform.position;
        Vector2 rayDir = Vector2.down;
        float rayRange = .3f;
        isGrounded = Physics2D.Raycast(rayOrigin, rayDir, rayRange, groundLayer);

        if (isGrounded)
        {
            coyoteTimeRemaining = maxCoyoteTime;
        }


        print(isGrounded);
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
            animator = GetComponentInChildren<Animator>();

        if (spriteRen == null)
            spriteRen = GetComponentInChildren<SpriteRenderer>();
    }
}
