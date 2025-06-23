using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/ShieldFire")]
public class ShieldFireEffect : ScriptableObject, IEffect
{
    [SerializeField] protected float shieldDuration = 5f;
    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<Health>(out var health))
        {
            health.SetFireShield(true, shieldDuration);
            Debug.Log($"[ShieldFireEffect] {player.name} Shield applied ({shieldDuration} sec)");
        }
    }
}