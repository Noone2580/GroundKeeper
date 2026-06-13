/// Made By
/// Name: Anaharishon
/// ID: 000872286
/// DES: Script that follows the Player around 

using UnityEngine;

public class S_PlayerFollow : MonoBehaviour
{
    public Rigidbody2D target;
    public SpriteRenderer targetSpriteRen;
    public float heihtOffset;
    public float lookAheadOffset;
    public Vector2 lookAheadSpeed;


    // Update is called once per frame
    void Update()
    {
        bool isFacingLeft = targetSpriteRen.flipX;
        float offsetX = isFacingLeft ?-lookAheadOffset : +lookAheadOffset;

        Vector3 targetPosition = target.position;


        targetPosition.z = this.transform.position.z;

        targetPosition.y += heihtOffset;

        targetPosition.x += offsetX;

        Vector3 newPosition = targetPosition;

        newPosition.x = Mathf.Lerp(this.transform.position.x, targetPosition.x, Time.deltaTime * lookAheadSpeed.x);
        newPosition.y = Mathf.Lerp(this.transform.position.y, targetPosition.y, Time.deltaTime * lookAheadSpeed.y);

        this.transform.position = newPosition;
    }

    private void OnValidate()
    {

    }
}
