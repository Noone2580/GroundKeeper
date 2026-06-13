using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class S_LevelChanger : MonoBehaviour
{

    public string level;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        SceneManager.LoadScene(level,LoadSceneMode.Single);
    }
}
