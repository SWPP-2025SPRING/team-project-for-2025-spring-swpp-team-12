using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GetPlayerName : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public Button okButton;

    void Start()
    {
        nameInputField.onValueChanged.AddListener(OnInputValueChanged);
        okButton.interactable = false;  // 초기 비활성화
    }

    void OnInputValueChanged(string input)
    {
        // 영문 8자 이내 검사
        bool isValid = IsValidName(input);
        okButton.interactable = isValid;
    }

    bool IsValidName(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        if (input.Length > 8)
            return false;

        // 영문자 또는 숫자만 허용
        foreach (char c in input)
        {
            if (!char.IsLetterOrDigit(c))  // 영문자 또는 숫자만
                return false;
        }

        return true;
    }

    public string GetPlayerNameInput()
    {
        return nameInputField.text.Trim();
    }

    public void OnOkButtonClicked()
    {
        GameManager.Instance.SetPlayerName(nameInputField.text.Trim());
        SceneManager.LoadScene("LeaderBoardScene");
    }

}
