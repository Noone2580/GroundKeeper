using UnityEngine;

public class S_TEST_Col : MonoBehaviour
{
    public Renderer[] renderers;
    public Collider2D col;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hits = new Collider2D[10];

        bool inside = false;

        // check for overlaping objects
        col.Overlap(new ContactFilter2D(), hits);

        // If no hit then leave
        if (hits.Length <= 0) return;

        // go throught hits array and find object with tag
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i] || !hits[i].enabled) continue;

            if (hits[i].CompareTag("Player"))
            {
                inside = true;
                for (int j = 0; j < renderers.Length; j++)
                {
                    renderers[j].enabled = false;
                }
                return;
            }
        }

        if (!inside)
        {
            for (int j = 0; j < renderers.Length; j++)
            {
                renderers[j].enabled = true;
            }
        }
    }
}
