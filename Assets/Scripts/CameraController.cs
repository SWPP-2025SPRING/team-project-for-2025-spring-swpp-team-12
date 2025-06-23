using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 horizontalOffset = new Vector3(0, 20f, -50f);
    public Vector3 verticalOffset = new Vector3(0, 15f, -30f);
    public PlayerMovement playerMovement;
    public float smoothSpeed = 10f;

    float collisionBuffer = 0.7f;

    void LateUpdate()
    {
        if (target == null || playerMovement == null) return;
        Vector3 offset = GetOffset();
        Vector3 targetPos = target.position + Vector3.up * 5f; // 약간 위를 바라보게
        Vector3 desiredPos = targetPos + Quaternion.Euler(0, target.eulerAngles.y, 0) * offset;

        Vector3 rayDir = desiredPos - targetPos;
        float distance = rayDir.magnitude;

        if (Physics.Raycast(targetPos, rayDir.normalized, out RaycastHit hit, distance, LayerMask.GetMask("CameraCollision")))
        {
            // 충돌 위치보다 살짝 앞쪽으로 카메라 위치 보정
            desiredPos = hit.point - rayDir.normalized * collisionBuffer;
        }
        // 부드럽게 따라가기
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
        transform.LookAt(targetPos);

    }
    Vector3 GetOffset()
    {
        switch(playerMovement.runState)
        {
            case PlayerRunningState.Vertical:
                return verticalOffset;
            case PlayerRunningState.Horizontal:
            case PlayerRunningState.Hor2Vert:
            default:
                return horizontalOffset;
        }
    }
}
