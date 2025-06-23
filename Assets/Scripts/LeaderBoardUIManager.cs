using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LeaderBoardUIManager : MonoBehaviour
{
    public Transform contentParent;    // Entries Container
    public GameObject entryPrefab;     // Entry Prefab
    public Button nextButton;
    public Button prevButton;
    public TextMeshProUGUI pageText;


    private List<PlayerEntry> entries;
    private int currentPage = 0;
    private int itemsPerPage = 8;

    public LeaderBoardManager leaderboardManager;
    private GetPlayerName getPlayerName;

    void Start()
    {
        leaderboardManager.LoadLeaderboard();
        leaderboardManager.AddScore(GameManager.Instance.GetPlayerName(), GameManager.Instance.GetPlayTime());
        entries = leaderboardManager.GetLeaderboardEntries();
        ShowPage(0);
    }

    void ShowPage(int page)
    {
        foreach (Transform child in contentParent)
        {
            if (child.name == "Head")
                continue;

            Destroy(child.gameObject);
        }

        int startIndex = page * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, entries.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject entry = Instantiate(entryPrefab, contentParent);
            TextMeshProUGUI rankText = entry.transform.Find("Rank").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = entry.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI playTimeText = entry.transform.Find("PlayTime").GetComponent<TextMeshProUGUI>();

            rankText.text = $"{i + 1}";
            nameText.text = entries[i].playerName;

            int minutes = (int)(entries[i].playTime / 60);
            float seconds = entries[i].playTime % 60;
            playTimeText.text = $"{minutes}:{seconds:00.0}";
        }

        prevButton.interactable = page > 0;
        nextButton.interactable = endIndex < entries.Count;

        pageText.text = $"{page + 1} / {Mathf.CeilToInt((float)entries.Count / itemsPerPage)}";
    }

    public void OnNextPage()
    {
        currentPage++;
        ShowPage(currentPage);
    }

    public void OnPrevPage()
    {
        currentPage--;
        ShowPage(currentPage);
    }

    public void OnOKButtonClicked()
    {
        string playerName = getPlayerName.GetPlayerNameInput();

        leaderboardManager.AddScore(playerName, playTime: 70.0f);
        leaderboardManager.SaveLeaderboard();
        entries = leaderboardManager.GetLeaderboardEntries();

        currentPage = 0;
        ShowPage(currentPage);
    }
}

