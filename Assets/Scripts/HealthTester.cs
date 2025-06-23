using UnityEngine;
using System.Collections;

public class HealthTester : MonoBehaviour
{
    public Health health; 

    void Start()
    {
        if (health == null)
        {
            health = FindObjectOfType<Health>(); // 자동으로 찾기
        }

        StartCoroutine(DamageLoop());
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f); // 1초 대기
            if (health != null)
            {
                health.ChangeHealth(10); // HP를 10씩 깎기
                Debug.Log($"[HealthTester] HP decreased by 10. Current: {health.CurrentHealth}");
            }
        }
    }
}
