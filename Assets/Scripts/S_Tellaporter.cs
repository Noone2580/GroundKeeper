/// Made By
/// Name: Anaharishon
/// ID: 000872286
/// DES: Teleports an object to the portPostion

using UnityEngine;

public class S_Tellaporter : MonoBehaviour
{
    public Transform portPostion;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.transform.position = portPostion.position;
    }
}
