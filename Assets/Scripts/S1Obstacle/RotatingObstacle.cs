using UnityEngine;

public class RotatingObstacle : MonoBehaviour, IStunnable
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveDirection = Vector3.forward;
    [SerializeField] private float moveDistance = 10f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 rotationAxis = Vector3.right;
    [SerializeField] private float rotationSpeed = 180f; // degrees per second

    private Vector3 startPos;
    private bool isMoving = true;
    private bool isStunned = false;
    private float stunTimer = 0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
                isStunned = false;
            return;
        }

        if (isMoving)
        {
            // 이동
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

            if (Vector3.Distance(startPos, transform.position) >= moveDistance)
            {
                Destroy(gameObject); // 목표 거리 도달 시 삭제
            }
        }

        // 회전
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }

    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
    }
}
