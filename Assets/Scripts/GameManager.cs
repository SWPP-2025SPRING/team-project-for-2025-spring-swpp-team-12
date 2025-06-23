using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGamePause;
    public event Action OnGameOver;
    public event Action OnGameClear;
    

    private bool isPaused = false;
    public bool isBoosted { get; set; } = false;
    public bool isSlowed { get; set; } = false;
    public bool isBurning { get; set; } = false;

    public bool isSlowImmune = false;

    private float playTime = 0.0f;
    private string playerName = "";
    private int life = 3;




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

    void Update()
    {
        if (SceneManager.GetActiveScene().name.StartsWith("Stage"))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Inventory.Instance.UseActiveItem(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Inventory.Instance.UseActiveItem(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Inventory.Instance.UseActiveItem(3);
            }
        }

    }

    public void DiscountLife()
    {
        life -= 1;
        if (life <= 0)
        {
            GameSceneUIManager uIManager = FindObjectOfType<GameSceneUIManager>();
            uIManager.DisableLastActiveLife();
            GameOver();
        }
        else
        {
            RestartFromCurrentStage();
        }
    }

    public void InitializeInfo()
    {
        playTime = 0.0f;
        playerName = "";
        life = 3;
    }

    public void ChangeHP(int curr, int max)
    {
        OnHPChanged?.Invoke(curr, max);
    }

    void OnEnable()
    {
    	// 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.StartsWith("Stage"))
        {
            isPaused = true;
            Time.timeScale = 0;
            GameSceneUIManager uIManager = FindObjectOfType<GameSceneUIManager>();
            uIManager.ShowReadyAndStart();
        }
    }
    public void StartGame()
    {
        Debug.Log("StartGame");
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

    public void GameClear()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnGameClear?.Invoke();
    }

    public void RestartGame()
    {
        InitializeInfo();
        SceneManager.LoadScene("Stage1Scene");
    }

    public void RestartFromCurrentStage()
    {
        TimeController timeController = FindObjectOfType<TimeController>();
        this.playTime = timeController.GetTime();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetPlayTime(float t)
    {
        playTime = t;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public float GetPlayTime() => playTime;
    public string GetPlayerName() => playerName;
    public int GetLife() => life;


    public float GetSpeedMultiplier()
    {
        if (isSlowImmune)
            return isBoosted ? 1.5f : 1f; // 슬로우 무시

        if (isSlowed)
            return 0.7f;

        if (isBoosted)
            return 1.5f;

        return 1f;
    }

}
