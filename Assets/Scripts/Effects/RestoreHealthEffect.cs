using UnityEngine;

[CreateAssetMenu(menuName = "Effect/Restore Health")]
public class RestoreHealthEffect : ScriptableObject, IEffect
{
    [SerializeField] protected int healAmount = 50;

    public void Apply(GameObject player)
    {
        if (player.TryGetComponent<Health>(out var health))
        {
            // 체력 회복: amount < 0이면 회복
            health.ChangeHealth(-healAmount);
            Debug.Log($"[RestoreHealthEffect] {player.name} 회복: {healAmount}");
        }
        else
        {
            Debug.LogWarning($"[RestoreHealthEffect] {player.name}에 Health 컴포넌트가 없습니다!");
        }
    }
}
