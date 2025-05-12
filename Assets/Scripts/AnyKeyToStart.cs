using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToStart : MonoBehaviour
{
    private bool hasStarted = false;

    void Update()
    {
        if (!hasStarted && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            hasStarted = true;
            SceneManager.LoadScene("GameScene");
        }
    }
}
