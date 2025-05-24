using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HpController : MonoBehaviour
{
    public Image fillImage;
    public float maxHP = 100f;
    private float currentHP;

    public float fillSpeed = 2f;
    private float targetFill = 1f;

    void Start()
    {
        currentHP = maxHP;
        targetFill = 1f;
    }

    void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);

        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        targetFill = currentHP / maxHP;

        StartCoroutine(ShakeHPBar());
    }

    IEnumerator ShakeHPBar()
    {
        Vector3 originalPos = transform.localPosition;
        for (int i = 0; i < 5; i++)
        {
            transform.localPosition = originalPos + (Vector3)Random.insideUnitCircle * 5f; 
            yield return new WaitForSeconds(0.02f);
        }
        transform.localPosition = originalPos;
    }
}
