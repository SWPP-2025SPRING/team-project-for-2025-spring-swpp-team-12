using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
