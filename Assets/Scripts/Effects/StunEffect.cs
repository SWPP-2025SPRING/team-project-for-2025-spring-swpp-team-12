using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Effect/Stun")]
public class StunEffect : ScriptableObject, IEffect
{
    [SerializeField] protected float stunDuration = 5f;
    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<Health>(out var health))
        {
            health.ShieldMode(stunDuration, 0);
            Debug.Log($"[StunEffect] {player.name} Shield applied ({stunDuration} sec)");
        }

        IStunnable[] targets = GameObject.FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IStunnable>()
            .ToArray();

        foreach (IStunnable target in targets)
        {
            target.Stun(stunDuration);
        }

        Debug.Log("모든 대상에게 스턴 적용!");
    }
}