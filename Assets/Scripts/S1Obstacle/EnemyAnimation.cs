using UnityEngine;

public class EnemyAnimation : MonoBehaviour, IStunnable
{
    [SerializeField] private Animator animator;

    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                animator.speed = 1f;
                //공격 모드로 전환
            }
            return;
        }

    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;

        //idle 상태
        if (animator != null)
        {
            animator.speed = 0f; // 애니메이션 정지
        }
    }
}
