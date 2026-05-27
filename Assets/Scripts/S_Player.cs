using UnityEngine;
using UnityEngine.InputSystem;

public class S_Player : MonoBehaviour
{
    // VARAIBLES

    // Objects
    public CapsuleCollider2D capsuleCol;
    public CapsuleCollider2D damageCol;
    public Rigidbody2D rb2d;
    public Animator animator;
    public SpriteRenderer spriteRen;

    // Player
    private Vector2 moveDir;
    public Vector2 moveSpeed = new Vector2(.5f, 1);
    public Vector2 maxMoveSpeed = new Vector2(3, 1);
    public float jumpVel = 3.4f;
    public float airControll = .3f;
    public bool isFacingLeft { get; protected set; } = true;

    // Tools
    public bool hasShovel { get; protected set; } = false;
    public float shovelDamage = 3;
    public bool hasAxe { get; protected set; } = false;
    public float axeDamage = 5;


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
        // Check if can jump
        if (isGrounded || coyoteTimeRemaining > 0)
            rb2d.linearVelocityY = jumpVel;
    }

    public void OnShovel(InputValue value)
    {
        if (!hasShovel) return;

        Damage(S_Enums.Etools.Shovel);
    }

    public void OnAxe(InputValue value)
    {
        if (!hasAxe) return;

        Damage(S_Enums.Etools.Axe);
    }

    public void OnInteract(InputValue value)
    {
        Collider2D[] hits = new Collider2D[10];

        // check for overlaping objects

        capsuleCol.Overlap(new ContactFilter2D(), hits);

        // If no hit then leave
        if (hits.Length <= 0) return;

        // go throught hits array and find object with tag
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i] || !hits[i].enabled) continue;

            if (hits[i].CompareTag("Interactables"))
            {
                hits[i].gameObject.SendMessage("Interact", gameObject);
                return;
            }

        }
    }

    public void pickupTool(S_Enums.Etools tool)
    {
        //index = Mathf.Clamp(index, 0, 1);

        switch (tool)
        {
            case S_Enums.Etools.Shovel:
                hasShovel = true; break;
            case S_Enums.Etools.Axe:
                hasAxe = true; break;
        }
    }

    public void Damage(S_Enums.Etools damage)
    {
        Collider2D[] hits = new Collider2D[10];

        // check for overlaping objects
        damageCol.Overlap(new ContactFilter2D(), hits);

        // If no hit then leave
        if (hits.Length <= 0) return;

        // go throught hits array and find object with tag
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i] || !hits[i].enabled) continue;

            if (hits[i].CompareTag("Objects"))
            {
                hits[i].gameObject.SendMessage("Damage", damage);
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

            bool hitWall = Physics2D.Raycast(edgeClipTopOrigin, edgeClipDirection, edgeClipRayDistance, groundLayer);

            if (!hitWall)
            {
                float VelX = rb2d.linearVelocityX;

                // Change moveSpeed X if in the air
                float newSpeed = isGrounded ? moveSpeed.x : moveSpeed.x * airControll;


                if (Mathf.Abs(VelX) < maxMoveSpeed.x || (VelX > 0 && moveDir.x < 0) || (VelX < 0 && moveDir.x > 0))
                {
                    float force = 1;
                    // if player is moving and want to go in opasite direction
                    if ((VelX > 0 && moveDir.x < 0) || (VelX < 0 && moveDir.x > 0))
                    {
                        force = .2f;
                    }
                    else
                    {
                        force = Mathf.Clamp(Mathf.Abs(rb2d.linearVelocityX) / maxMoveSpeed.x, 0, 1);
                        force = 1 - force;
                    }

                    newSpeed *= force;

                    // Move by apliding force
                    rb2d.AddForceX(newSpeed * moveDir.x, ForceMode2D.Force);
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

        if (damageCol == null)
            damageCol = GetComponentInChildren<CapsuleCollider2D>();
    }
}
