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

    //invincibility settings -> 여러번 충돌 방지
    protected bool isInvincible = false;
    [SerializeField] protected float invincibleDuration = 0f;

    protected bool isShieldMode = false;
    [SerializeField] protected float shieldDuration = 0f;
    [SerializeField] protected int damageReductionFactor = 0;
    private Coroutine shieldCoroutine;

    private bool hasFireShield = false;




    void Awake()
    {
        currentHealth = maxHealth;
        // targetFill = 1f;
    }

    void Update()
    {
        // fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFill, Time.deltaTime * fillSpeed);

        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeHealth(10);
        }
    }

    public void ChangeHealth(int amount)
    {
        if (isInvincible) return;
        if (isShieldMode)
        {
            // 무적 쉴드라면 (reductionFactor == 0), 완전 무시
            if (damageReductionFactor == 0)
            {
                //Debug.Log($"[Health] Shield active - damage fully blocked");
                return;
            }

            int reducedAmount = Mathf.RoundToInt(amount / damageReductionFactor);
            //Debug.Log($"[Health] Shield active - damage reduced: {amount} -> {reducedAmount}");
            amount = reducedAmount;


        }

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // targetFill = currentHealth / maxHealth;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // StartCoroutine(ShakeHealthBar());
        if (currentHealth <= 0)
            Die();

        if (invincibleDuration > 0f && amount > 0)
        {
            StartCoroutine(InvincibleCoroutine());
        }
    }

    public void ShieldMode(float duration, int reductionFactor)
    {
        shieldDuration = duration;
        damageReductionFactor = reductionFactor;

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }

        shieldCoroutine = StartCoroutine(ShieldCoroutine());
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

    protected IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    protected IEnumerator ShieldCoroutine()
    {
        isShieldMode = true;
        yield return new WaitForSeconds(shieldDuration);
        isShieldMode = false;
        damageReductionFactor = 0;
        shieldCoroutine = null; // 코루틴 종료 처리
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            Debug.Log("player die");
            GameManager.Instance.DiscountLife();
        }
        Debug.Log($"{gameObject.name} has died!");
        // gameObject.SetActive(false);

    }

    public bool GetFireShield() => hasFireShield;
    
    public void SetFireShield(bool value, float duration)
    {
        hasFireShield = value;
        if (value)
            StartCoroutine(RemoveShieldAfter(duration));
    }

    private IEnumerator RemoveShieldAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        hasFireShield = false;
    }
}
