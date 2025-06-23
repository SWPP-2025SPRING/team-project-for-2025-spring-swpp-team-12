using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

public enum StaminaChangeType
{
    Default,
    Regen,
    Item,
    Run,    // amount < 0
}

[DisallowMultipleComponent]
public class Stamina : MonoBehaviour
{
    [SerializeField] int maxStamina = 3000;
    // [SerializeField] float recoveryRate = 10f; // 초당 회복량

    private int currentStamina;
    // private bool isRecovering = true;

    // 스테미나 사용이 불가능한 상태 (달리기 등)
    private bool isStaminaUnavailable = false;
    private bool isStaminaNotRegenerating = false;
    private bool isExhausted = false;

    public UnityEvent<int, int> OnStaminaChanged; // <current, max>

    public int CurrentStamina => currentStamina;
    public int MaxStamina => maxStamina;

    void Awake()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {

        // Debug.Log("Current Stamina : " + currentStamina + "/" + maxStamina);
    }

    public bool IsStaminaAvailable()
    {
        if (currentStamina == 0 || isStaminaUnavailable)
        {
            return false;
        }
        return true;
    }

    public bool isStaminaFull()
    {
        return currentStamina == maxStamina;
    }

    public void ChangeStamina(int amount, StaminaChangeType type = StaminaChangeType.Default)
    {
        if (type == StaminaChangeType.Regen && isStaminaNotRegenerating) return;
        if (isStaminaUnavailable && (type==StaminaChangeType.Run)) return;
        if (isExhausted && type == StaminaChangeType.Regen)
        {
            amount = Mathf.RoundToInt(amount * 0.8f);
        }

        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        if (currentStamina == 0)
        {
            Debug.Log("Exhausted!");
            isExhausted = true;
            StartCoroutine(LockStamina(2f));
        }
        else if (currentStamina == maxStamina)
        {
            Debug.Log("Stamina fully recovered!");
            isExhausted = false;
        }
    }
    private IEnumerator LockStamina(float duration)
    {
        isStaminaUnavailable = true;
        yield return new WaitForSeconds(duration);
        isStaminaUnavailable = false;
    }

    /* Unused functions */
    // private void RecoverStamina()
    // {
    //     ChangeStamina(Mathf.RoundToInt(recoveryRate * Time.deltaTime));
    // }
    // public void StopRecovery(float duration)
    // {
    //     StopAllCoroutines();
    //     StartCoroutine(StopRecoveryCoroutine(duration));
    // }

    // private IEnumerator StopRecoveryCoroutine(float duration)
    // {
    //     isRecovering = false;
    //     yield return new WaitForSeconds(duration);
    //     isRecovering = true;
    // }
}
