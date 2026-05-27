using System.Collections;
using UnityEngine;

public class S_Objects : S_Interactable
{

    public S_Enums.Etools toolNeeded;
    public float hp = 2;
    public float damageJiggle = .03f;
    Transform startTransform;
    Vector3 pos;
    public GameObject spriteAncer;


    void Start()
    {
        pos = spriteAncer.transform.position;
    }

    public override bool Damage(S_Enums.Etools tool)
    {
        if (tool != toolNeeded) return false;

        hp--;

        if (hp <= 0)
        {
            Destroy(gameObject);
            return false;
        }

        StartCoroutine(Desplace());

        return true;
    }

    IEnumerator Desplace()
    {
        for (int i = 0; i < 5; i++)
        {
            spriteAncer.transform.position = pos + new Vector3(Random.Range(-damageJiggle, damageJiggle), Random.Range(-damageJiggle, damageJiggle), 0);

            print(i);
            yield return new WaitForSeconds(.02f);

            spriteAncer.transform.position = pos;
        }

    }
}
