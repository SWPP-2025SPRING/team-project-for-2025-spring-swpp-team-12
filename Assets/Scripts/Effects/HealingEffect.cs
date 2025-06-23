using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Healing")]
public class HealingEffect : ScriptableObject, IEffect
{
    [SerializeField] private int totalHealAmount = 10;
    [SerializeField] private float duration = 10f;
    [SerializeField] private float tickInterval = 0.5f;

    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<MonoBehaviour>(out var runner) &&
            player.TryGetComponent<Health>(out var health))
        {
            runner.StartCoroutine(HealOverTime(health));
        }
        else
        {
            Debug.LogWarning("[GradualRestoreHealthEffect] 실행 불가: Health 또는 MonoBehaviour 누락");
        }
    }

    private IEnumerator HealOverTime(Health health)
    {
        int ticks = Mathf.FloorToInt(duration / tickInterval);
        int healPerTick = Mathf.CeilToInt((float)totalHealAmount / ticks);

        for (int i = 0; i < ticks; i++)
        {
            health.ChangeHealth(-healPerTick);
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
