using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    public GameObject getNamePanel;
    private PlayerMovement playerMovement;
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        GameManager.Instance.OnGameClear += OnGameClearReceived;
    }

    void OnGameClearReceived()
    {
        getNamePanel.SetActive(true);
        playerMovement.SetCanMove(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
