using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Obstacle
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // PlayerStatus 확인
        if (other.TryGetComponent<Health>(out var status) && status.GetFireShield())
        {
            Debug.Log("Fire Shield active! No damage taken.");
            return; // 피해 무시
        }

        // Health 적용
        if (other.TryGetComponent<Health>(out var playerHealth))
        {
            playerHealth.ChangeHealth(damageToPlayer);
            OnHitPlayer();
        }
    }

    protected override void OnHitPlayer()
    {
        //dotdeal
    }
}
