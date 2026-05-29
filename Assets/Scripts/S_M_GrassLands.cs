using UnityEngine;
using UnityEngine.Rendering.Universal;

public class S_M_GrassLands : MonoBehaviour
{

    public Light2D sun;
    public Collider2D StartSunBox;
    public Collider2D EndSunBox;
    public Camera playerCamera;
    public Transform player;

    float sunTarget = 1;
    bool sunChanging = false;


    void Update()
    {
        if (sun == null) return;
        if (sunChanging)
        {
            sun.intensity = Mathf.Lerp(sun.intensity, sunTarget, 2 * Time.deltaTime);
            if (sun.intensity == sunTarget) sunChanging = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        print("SUN!");

        if (transform.position.x < player.position.x)
        {
            sunTarget = 1;
            sunChanging = true;
        }

        else if (transform.position.x > player.position.x)
        {
            sunTarget = 0;
            sunChanging = true;
        }
    }
}
