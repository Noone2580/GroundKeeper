using UnityEngine;

public class Enemy : MonoBehaviour
{
    //RB2D To move
    public Rigidbody2D rb2d;

    //Which layers do raycast look for

    // Changed name to fix violation
    public LayerMask groundLayer;

    public float distanceCheckWall = 1;
    public float distanceCheckLedge = 1;
    public float distanceCheckWallOffsetY = -.5f;
    //Movement & Rendering
    public float moveSpeedX = 5;
    public bool moveRight = true;
    
    // Changed name to fix violation
    public SpriteRenderer sprite;
    
    // Changed Type Player to GameObject to save on memory
    public GameObject player;

    public float playerChaseRadius = 16;
    public float chaseSpeedX = 7;


    void Update()
    {

        float distancetoPlayer = Vector2.Distance(transform.position, player.transform.position);

        /*
        
        if (distancetoPlayer <= playerChaseRadius)
        {

            Chase();

        }
        else
        {

            Patrol();
        }
        */

        Patrol();
    }


    void Chase()
    {
        //Check to see if player x coordinate is greater than ours = move right
        moveRight = player.transform.position.x > transform.position.x;

        float LVX = moveRight ? +chaseSpeedX : -chaseSpeedX;

        rb2d.linearVelocityX = LVX;

    }


    void Patrol()
    {
        //We will shoot ray to detect walls from entre of enemy
        Vector2 wallDetectedOrigin = transform.position;
        //Offset Y up or down for this check
        wallDetectedOrigin.y += distanceCheckWallOffsetY;

        Vector2 wallDetectedDir = moveRight ? Vector2.right : Vector2.left;

        //Shoot ray from origin in direction to a max of distance against layers in layer mask only
        bool willHitWall = Physics2D.Raycast(wallDetectedOrigin, wallDetectedDir, distanceCheckWall, groundLayer);

        //debug draw the raycast
        Debug.DrawLine(wallDetectedOrigin, wallDetectedOrigin + wallDetectedDir * distanceCheckWall);



        Vector2 ledgeDetetOffsetDir = moveRight ? Vector2.right : Vector2.left;
        Vector2 ledgeDetectDir = Vector2.down;

        // We don't need this it't not scalable. and it adds to much off set to ledge detation
        //Vector2 ledgeDetectOrigin = (Vector2)transform.position + ledgeDetetOffsetDir;


        // Instent of making an offset just use the wallcheck instand.
        // but you can make an offset if you realy need to.
        //bool willWalkOffLedge = !Physics2D.Raycast(ledgeDetectOrigin, ledgeDetectDir, distanceCheckLedge, groundLayer);

        Vector2 ledgeDetectOrigin = (Vector2)transform.position + new Vector2(distanceCheckWall * ledgeDetetOffsetDir.x,0);


        bool willWalkOffLedge = !Physics2D.Raycast(ledgeDetectOrigin, ledgeDetectDir, distanceCheckLedge, groundLayer);


        Debug.DrawLine(ledgeDetectOrigin, ledgeDetectOrigin + ledgeDetectDir * distanceCheckLedge, willWalkOffLedge ? Color.green: Color.red);

        if (willHitWall || willWalkOffLedge)
        {

            //Flip bool
            moveRight = !moveRight;
            sprite.flipX = !moveRight;

        }

        //Calculate which direction we need to move in
        float linearVelocityX = moveRight ? moveSpeedX : -moveSpeedX;
        rb2d.linearVelocityX = linearVelocityX;

    }
}