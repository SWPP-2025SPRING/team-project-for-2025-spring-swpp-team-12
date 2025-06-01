using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

// TODO : Shake Effect or visualization of health bar will be implemented in actual HP UI by listening OnHealthChanged Event.
    
[DisallowMultipleComponent]
public class Health : MonoBehaviour, IDamageable
{
    // public Image fillImage;
    [SerializeField] int maxHealth = 100;
    private int currentHealth;
    public UnityEvent<int, int> OnHealthChanged;    // <curr, max>
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    // public float fillSpeed = 2f;
    // private float targetFill = 1f;

    void Awake()
    {
        currentHealth = maxHealth;
        // targetFill = 1f;
    }

    void Update()
    {
        // fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);

        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     ChangeHealth(10);
        // }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // targetFill = currentHealth / maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // StartCoroutine(ShakeHealthBar());
        if (currentHealth <= 0)
            Die();
    }

    // IEnumerator ShakeHealthBar()
    // {
    //     Vector3 originalPos = transform.localPosition;
    //     for (int i = 0; i < 5; i++)
    //     {
    //         transform.localPosition = originalPos + (Vector3)UnityEngine.Random.insideUnitCircle * 5f;
    //         yield return new WaitForSeconds(0.02f);
    //     }
    //     transform.localPosition = originalPos;
    // }
    void Die()
    {
        // TODO : Death Handler
        // gameObject.SetActive(false);

    }
}
