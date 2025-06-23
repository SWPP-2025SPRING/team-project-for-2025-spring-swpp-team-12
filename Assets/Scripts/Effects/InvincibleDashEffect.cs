using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/InvincibleDash")]
public class InvincibleDashEffect : ScriptableObject, IEffect
{
    [SerializeField] protected float invincibleDuration = 5f;
    private int damageReductionFactor = 0; // 0 = 무적, 0.5 = 1/2, etc.

    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<Health>(out var health))
        {
            health.ShieldMode(invincibleDuration, damageReductionFactor);
            Debug.Log($"[ShieldEffect] {player.name} Shield applied ({invincibleDuration} sec, reduction: {damageReductionFactor})");
        }
        else
        {
            Debug.LogWarning($"[ShieldEffect] {player.name}에 Health 컴포넌트가 없습니다!");
        }
    }
}