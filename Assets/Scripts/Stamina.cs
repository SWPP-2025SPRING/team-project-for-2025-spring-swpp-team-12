using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;

[DisallowMultipleComponent]
public class Stamina : MonoBehaviour
{
    [SerializeField] int maxStamina = 100;
    [SerializeField] float recoveryRate = 10f; // 초당 회복량

    private int currentStamina;
    private bool isRecovering = true;

    public UnityEvent<int, int> OnStaminaChanged; // <current, max>

    public int CurrentStamina => currentStamina;
    public int MaxStamina => maxStamina;

    void Awake()
    {
        currentStamina = maxStamina;
    }

    void Update()
    {
        if (isRecovering && currentStamina < maxStamina)
        {
            RecoverStamina();
        }
    }

    public void ChangeStamina(int amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    private void RecoverStamina()
    {
        ChangeStamina(Mathf.RoundToInt(recoveryRate * Time.deltaTime));
    }

    public void StopRecovery(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(StopRecoveryCoroutine(duration));
    }

    private IEnumerator StopRecoveryCoroutine(float duration)
    {
        isRecovering = false;
        yield return new WaitForSeconds(duration);
        isRecovering = true;
    }
}
