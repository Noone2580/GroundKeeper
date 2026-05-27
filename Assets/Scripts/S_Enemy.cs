using UnityEngine;

public class S_Enemy : MonoBehaviour
{
    public float distanceCheckWallOffsetY = 0;

    void Patrol()
    {
        //We will shoot ray to detect walls from entre of enemy
        Vector2 wallDetectedOrigin = transform.position;
        //Offset Y up or down for this check
        wallDetectedOrigin.y += distanceCheckWallOffsetY;

        Vector2 wallDetectedDir = moveRight ? Vector2.right : Vector2.left;

        //Shoot ray from origin in direction to a max of distance against layers in layer mask only
        bool willHitWall = Physics2D.Raycast(wallDetectedOrigin, wallDetectedDir, distanceCheckWall, layerMask);

        //debug draw the raycast
        Debug.DrawLine(wallDetectedOrigin, wallDetectedOrigin + wallDetectedDir * distanceCheckWall);




        Vector2 ledgeDetetOffsetDir = moveRight ? Vector2.right : Vector2.left;
        Vector2 ledgeDetectOrigin = (Vector2)transform.position + ledgeDetetOffsetDir;

        Vector2 ledgeDetectDir = Vector2.down;

        bool willWalkOffLedge = !Physics2D.Raycast(ledgeDetectOrigin, ledgeDetectDir, distanceCheckLedge, layerMask);



        Debug.DrawLine(ledgeDetectOrigin, ledgeDetectOrigin + ledgeDetectDir * distanceCheckLedge);

        if (willHitWall || willWalkOffLedge)
        {

            //Flip bool
            moveRight = !moveRight;
            SpriteRenderer.flipX = !moveRight;

        }


        //Calculate which direction we need to move in

        float linearVelocityX = moveRight ? moveSpeedX : -moveSpeedX;
        rigidbody2D.linearVelocityX = linearVelocityX;



    }
}
