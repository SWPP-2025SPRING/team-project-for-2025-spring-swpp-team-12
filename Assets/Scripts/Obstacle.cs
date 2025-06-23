using UnityEngine;

public abstract class Obstacle : MonoBehaviour
{
    [SerializeField] protected int damageToPlayer = 10;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 태그가 "Player"가 아니면 무시
        if (!other.CompareTag("Player")) return;

        // Player 태그를 가진 오브젝트라면 Health 컴포넌트 가져오기
        if (other.TryGetComponent<Health>(out var playerHealth))
        {
            playerHealth.ChangeHealth(damageToPlayer);
            OnHitPlayer();
        }
    }

    //hit시 효과 구현
    protected abstract void OnHitPlayer();
}
