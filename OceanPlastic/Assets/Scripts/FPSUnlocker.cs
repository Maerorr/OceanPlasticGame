using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSUnlocker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        SceneManager.LoadScene("Scenes/Levels/Tutorial");
    }
}