using UnityEngine;
using TMPro;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float elapsedTime = 0f;

    void Start()
    {
        GameManager.Instance.OnGameClear += OnGameClearReceived;
        elapsedTime = GameManager.Instance.GetPlayTime();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetTime()
    {
        return elapsedTime;
    }

    void OnGameClearReceived()
    {
        float finalTime = elapsedTime;

        GameManager.Instance.SetPlayTime(finalTime);
    }
}
