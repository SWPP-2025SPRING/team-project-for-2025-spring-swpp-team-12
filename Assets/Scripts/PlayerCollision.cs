using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string obstacleTag = "Obstacle";
    [SerializeField] private string instantKillTag = "InstantKill";

    private const int DAMAGE_NORMAL = 20;
    private const int DAMAGE_INSTANT = 999;

    [SerializeField] private float invulnerabilityDuration = 0.5f;

    private bool isInvulnerable = false;
    private Health healthComponent;

    void Awake()
    {
        //todo
    }

    // Collider.isTrigger = false일 때 물리 충돌이 발생하면 호출
    void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision.collider);
    }

    // Collider.isTrigger = true인 상대와 충돌하면 호출
    void OnTriggerEnter(Collider other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider other)
    {
        if (isInvulnerable || healthComponent == null)
            return;

        // CompareTag를 쓰면 오타 방지 및 성능상 이점이 있습니다.
        int damageToApply = 0;
        if (other.CompareTag(enemyTag) || other.CompareTag(obstacleTag))
        {
            damageToApply = DAMAGE_NORMAL;
        }
        else if (other.CompareTag(instantKillTag))
        {
            damageToApply = DAMAGE_INSTANT;
        }
        else
        {
            return;
        }

        // 실제로 체력 감소 호출
        healthComponent.ChangeHealth(damageToApply);

        // 0.5초 무적 처리
        StartCoroutine(BecomeTemporarilyInvulnerable());
    }

    private IEnumerator BecomeTemporarilyInvulnerable()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}
