using UnityEngine;

public class ItemFloatAndRotate : MonoBehaviour
{
    [Header("Floating Settings")]
    [SerializeField] private bool enableFloating = true;
    [SerializeField] private float floatAmplitude = 0.5f; // 상하 움직임 범위 (limit)
    [SerializeField] private float floatSpeed = 1f;        // 상하 속도

    [Header("Rotation Settings")]
    [SerializeField] private bool enableRotation = true;
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0f, 60f, 0f); // 초당 회전 각도

    private Vector3 initialPosition;
    private float floatTimer = 0f;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // 상하 움직임
        if (enableFloating)
        {
            floatTimer += Time.deltaTime * floatSpeed;
            float yOffset = Mathf.Sin(floatTimer) * floatAmplitude;
            transform.position = initialPosition + Vector3.up * yOffset;
        }

        // 회전
        if (enableRotation)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
    }
}
