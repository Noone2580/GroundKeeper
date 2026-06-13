/// Made By
/// Name: Anaharishon
/// ID: 000872286
/// DES: For player interactions with objects

using UnityEngine;

public class S_Interactable : MonoBehaviour
{

    public virtual bool Interact(GameObject interactingObject) 
    {
        if (interactingObject == null) return false;

        return true;
    }

    public virtual bool Damage( S_Enums.Etools tool) 
    {
        return true;
    }

}
