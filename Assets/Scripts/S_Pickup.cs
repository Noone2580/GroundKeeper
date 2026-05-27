using Unity.VisualScripting;
using UnityEngine;

public class S_Pickup : S_Interactable
{
    public S_Enums.EinteractTypes interactType;
    public S_Enums.Etools tool;

    public override bool Interact(GameObject interactingObject)
    {

        switch (tool)
        {
            case S_Enums.Etools.Shovel:
                if (interactingObject.GetComponent<S_Player>() == null) return false;

                if (!interactingObject.GetComponent<S_Player>().hasShovel)
                {
                    interactingObject.GetComponent<S_Player>().pickupTool(S_Enums.Etools.Shovel);
                }
                else return false;
                break;

            case S_Enums.Etools.Axe:
                if (interactingObject.GetComponent<S_Player>() == null) return false;

                if (!interactingObject.GetComponent<S_Player>().hasAxe)
                {
                    interactingObject.GetComponent<S_Player>().pickupTool(S_Enums.Etools.Axe);
                }
                else return false;
                break;

            case S_Enums.Etools.LawnMower:
                return false;
        }


        Destroy(gameObject);
        return true;
    }

}
