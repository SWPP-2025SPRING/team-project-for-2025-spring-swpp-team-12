using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGamePause;
    public event Action OnGameOver;

    private bool isPaused = false;

    [Serializable]
    public class HPChangedEvent : UnityEvent<int, int> { }
    public HPChangedEvent OnHPChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void ChangeHP(int curr, int max)
    {
        OnHPChanged?.Invoke(curr, max);
    }

    void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        OnGameStart?.Invoke();
    }



    public void PauseGame()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGamePause?.Invoke();
    }

    public void ResumeGame()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1;
        OnGameStart?.Invoke();
    }

    public void GameOver()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGameOver?.Invoke();
    }
}
