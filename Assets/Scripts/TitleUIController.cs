using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUIController : MonoBehaviour
{
    [SerializeField] private GameObject howToPanel;  
    void Awake()
    {
        if (howToPanel != null) howToPanel.SetActive(false);  // 처음엔 숨김
    }

    public void ShowHowTo() => howToPanel?.SetActive(true);

    public void HideHowTo() => howToPanel?.SetActive(false);

    // public void StartGame() => SceneManager.LoadScene("Stage1Scene");
}
