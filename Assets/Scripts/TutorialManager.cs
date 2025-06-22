using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public Image tutorialImage;              
    public TMP_Text tutorialText;            
    public Sprite[] tutorialSteps;           
    public string[] tutorialTexts;           

    public float typingSpeed = 0.05f;        
    public float waitAfterTyping = 1f;       

    private int currentStep = 0;

    void Start()
    {
        StartCoroutine(ShowStep());
    }

    IEnumerator ShowStep()
    {
        if (currentStep >= tutorialSteps.Length)
        {
            SceneManager.LoadScene("Stage1Scene");
            yield break;
        }

        // 현재 스텝 이미지 및 텍스트 설정
        tutorialImage.sprite = tutorialSteps[currentStep];
        tutorialText.text = "";

        string fullText = tutorialTexts[currentStep];

        foreach (char c in fullText)
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(waitAfterTyping);

        currentStep++;
        StartCoroutine(ShowStep());
    }
}
