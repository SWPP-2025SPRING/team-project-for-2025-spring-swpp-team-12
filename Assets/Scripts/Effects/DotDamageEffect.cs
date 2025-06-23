using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Dot Damage")]
public class DotDamageEffect : ScriptableObject, IEffect
{
    [SerializeField] protected int dotDamage = 3;
    [SerializeField] protected float interval = 1f;
    [SerializeField] protected int ticks = 10;

    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<PlayerMovement>(out var mono))
        {
            if (GameManager.Instance.isBurning) return;
            GameManager.Instance.isBurning = true;
            mono.StartCoroutine(ApplyDotDamage(player, interval, ticks));
        }
        else
        {
            Debug.LogWarning($"[DotDamageEffect] {player.name}에 PlayerMovement 컴포넌트가 없습니다!");
            return;
        }
    }

    IEnumerator ApplyDotDamage(GameObject player, float interval, int ticks)
    {
        if (player.TryGetComponent<Health>(out var health))
        {
            if (!health.GetFireShield())
            {
                for (int i = 0; i < ticks; i++)
                {
                    health.ChangeHealth(dotDamage);
                    Debug.Log($"[DotDamageEffect] {player.name}에게 {dotDamage}의 지속 피해를 입혔습니다. 남은 체력: {health.CurrentHealth}");
                    if (health.CurrentHealth <= 0)
                    {
                        Debug.Log($"[DotDamageEffect] {player.name}이(가) 사망했습니다. 지속 피해를 중지합니다.");
                        break;
                    }
                    yield return new WaitForSeconds(interval);
                }
                GameManager.Instance.isBurning = false;
            }
        }
        else
        {
            Debug.LogWarning($"[DotDamageEffect] {player.name}에 Health 컴포넌트가 없습니다!");
        }
    }
}
