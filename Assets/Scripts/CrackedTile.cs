using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedTile : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 4f;       // 떨어지는 속도
    [SerializeField] private float fallDelay = 0.3f;     // 충돌 후 지연 시간
    [SerializeField] private float fallDistance = 5f;    // 사라지기까지 떨어질 거리

    private bool isFalling = false;
    private float fallTimer = 0f;
    private float fallenDistance = 0f;

    private Collider tileCollider;

    void Awake()
    {
        tileCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        if (other.CompareTag("Player") && !isFalling)
        {
            isFalling = true;
            fallTimer = fallDelay;
        }
    }

    void Update()
    {
        if (isFalling)
        {
            if (fallTimer > 0f)
            {
                fallTimer -= Time.deltaTime;

                if (fallTimer <= 0f)
                {
                    // 타일 충돌 비활성화 (한 번만 실행됨)
                    if (tileCollider != null)
                    {
                        tileCollider.enabled = false;
                    }
                }
            }
            else
            {
                float moveAmount = fallSpeed * Time.deltaTime;
                transform.position += Vector3.down * moveAmount;
                fallenDistance += moveAmount;

                if (fallenDistance >= fallDistance)
                {
                    Destroy(gameObject); // 일정 거리 떨어졌으면 삭제
                }
            }
        }
    }
}
