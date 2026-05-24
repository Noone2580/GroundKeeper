using UnityEngine;

public class S_ComponentDesabler : MonoBehaviour
{

    public Renderer[] renderers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < renderers.Length; i++) 
        {
            renderers[i].enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
