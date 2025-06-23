using UnityEngine;

public class RotatingAxe : MonoBehaviour, IStunnable
{
    public float rotationSpeed = 45f; // 초당 회전 속도 (도 단위)
    public float maxAngle = 45f;      // 최대 회전 각도
    private float currentAngle = 0f;
    private int direction = 1;

    private bool isStunned = false;
    private float stunTimer = 0f;

    void Update()
    {
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
                isStunned = false;
            return;
        }

        float delta = rotationSpeed * Time.deltaTime * direction;
        currentAngle += delta;

        if (Mathf.Abs(currentAngle) > maxAngle)
        {
            direction *= -1;
            currentAngle = Mathf.Clamp(currentAngle, -maxAngle, maxAngle);
        }

        transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }

    public void Stun(float duration)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        isStunned = true;
        stunTimer = duration;
    }
}
