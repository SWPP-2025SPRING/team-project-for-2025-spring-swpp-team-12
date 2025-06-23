using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.Instance.ResumeGame();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
