using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void GoHome()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
