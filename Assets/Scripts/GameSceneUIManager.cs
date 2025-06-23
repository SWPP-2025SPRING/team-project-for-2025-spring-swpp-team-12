using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameSceneUIManager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject clearPanel;
    public GameObject overPanel;
    public GameObject lifePanel;
    public GameObject hpBackground;
    public GameObject staminaBackground;
    public GameObject hpFill;
    public GameObject staminaFill;

    public PlayerMovement playerMovement;

    public GameObject readyPanel;
    public GameObject startPanel;
    public float readyDuration = 1.5f;
    public float startDuration = 1.5f;

    void Start()
    {
        UpdateLifeUI();
    }

    public void UpdateLifeUI()
    {
        int lifeCount = GameManager.Instance.GetLife();

        for (int i = 0; i < lifePanel.transform.childCount; i++)
        {
            GameObject child = lifePanel.transform.GetChild(i).gameObject;
            child.SetActive(i < lifeCount);
        }
    }

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePause += PauseGame;
            GameManager.Instance.OnGameStart += ResumeGame;
            GameManager.Instance.OnGameClear += GameClear; // 게임 클리어 후 UI 초기화
            GameManager.Instance.OnGameOver += GameOver;  // 게임 오버 후 UI 초기화
        }

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            // 플레이어에서 Health 컴포넌트 찾기
            Health playerHealth = player.GetComponentInChildren<Health>();
            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged.AddListener(UpdateHealthBar);
            }

            // 플레이어에서 Stamina 컴포넌트 찾기
            Stamina playerStamina = player.GetComponentInChildren<Stamina>();
            if (playerStamina != null)
            {
                playerStamina.OnStaminaChanged.AddListener(UpdateStaminaBar);
            }
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGamePause -= PauseGame;
            GameManager.Instance.OnGameStart -= ResumeGame;
            GameManager.Instance.OnGameClear -= GameClear;
            GameManager.Instance.OnGameOver -= GameOver;
        }
    }

    public void PauseGame()
    {
        //pause 되면 패널 켜기
        pausePanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement.SetCanMove(true);
    }

    public void GameClear()
    {
        clearPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }

    public void GameOver()
    {
        overPanel.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        playerMovement.SetCanMove(false);
    }
    public void DisableLastActiveLife()
    {
        Transform lastActiveChild = null;

        // 자식들을 순회하며 활성화된 것 중 마지막을 찾음
        for (int i = 0; i < lifePanel.transform.childCount; i++)
        {
            Transform child = lifePanel.transform.GetChild(i);
            if (child.gameObject.activeSelf)
            {
                lastActiveChild = child;
            }
        }

        // 마지막으로 활성화된 자식이 있으면 비활성화
        if (lastActiveChild != null)
        {
            lastActiveChild.gameObject.SetActive(false);
            Debug.Log($"[UI] {lastActiveChild.name} 비활성화됨");
        }
    }

    public void UpdateHealthBar(int current, int max)
    {
        float ratio = Mathf.Clamp01((float)current / max);
        Vector3 scale = hpFill.transform.localScale;
        scale.x = ratio;
        hpFill.transform.localScale = scale;

        RectTransform rect = hpFill.GetComponent<RectTransform>();
        RectTransform bRect = hpBackground.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(ratio * bRect.rect.width / 2f, rect.anchoredPosition.y);
    }

    public void UpdateStaminaBar(int current, int max)
    {
        float ratio = Mathf.Clamp01((float)current / max);
        Vector3 scale = staminaFill.transform.localScale;
        scale.x = ratio;
        staminaFill.transform.localScale = scale;

        RectTransform rect = staminaFill.GetComponent<RectTransform>();
        RectTransform bRect = staminaBackground.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(ratio * bRect.rect.width / 2f, rect.anchoredPosition.y);
    }
    
    public void ShowReadyAndStart()
    {
        StartCoroutine(ShowSequenceCoroutine());
    }

    private IEnumerator ShowSequenceCoroutine()
    {
        readyPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(readyDuration);
        readyPanel.SetActive(false);

        startPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(startDuration);
        startPanel.SetActive(false);

        GameManager.Instance.StartGame(); 
    }
}

