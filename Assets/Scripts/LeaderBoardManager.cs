using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerEntry
{
    public string playerName;
    public float playTime;
}

[System.Serializable]
public class Leaderboard
{
    public List<PlayerEntry> entries = new List<PlayerEntry>();
}

public class LeaderBoardManager : MonoBehaviour
{
    private string fileName = "Files/leaderboard.json";
    private Leaderboard leaderboard = new Leaderboard();

    private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

    public void LoadLeaderboard()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            leaderboard = JsonUtility.FromJson<Leaderboard>(json);
            Debug.Log("Leaderboard loaded from " + FilePath);
        }
        else
        {
            Debug.Log("No leaderboard file found, creating a new one.");

            // 새로 빈 leaderboard 생성
            leaderboard = new Leaderboard();

            // 파일 저장
            string json = JsonUtility.ToJson(leaderboard, true);
            File.WriteAllText(FilePath, json);
            Debug.Log("New empty leaderboard saved to " + FilePath);
        }
    }

    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard, true);
        File.WriteAllText(FilePath, json);
    }

    public void AddScore(string playerName, float playTime)
    {
        if (playerName == "") return;
        PlayerEntry newEntry = new PlayerEntry { playerName = playerName, playTime = playTime };
        leaderboard.entries.Add(newEntry);
        leaderboard.entries.Sort((a, b) => a.playTime.CompareTo(b.playTime));
        SaveLeaderboard();
    }

    public List<PlayerEntry> GetLeaderboardEntries()
    {
        return leaderboard.entries;
    }
}
