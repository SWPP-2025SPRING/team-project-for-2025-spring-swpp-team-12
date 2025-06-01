using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    float crouchSpeed = 3f;
    float walkSpeed = 15f;
    float runSpeed = 30f;
    float jumpPower = 20f;
    float gravity = 40f;
    float lookSpeed = 2f;
    float lookXLimit = 70f;
    float defaultHeight = 4f;
    float crouchHeight = 2f;
    float shrinkSpeed = 5f;
    bool isCrouching = false;
    float wiggleSize = 0f;

    int runState = 0;   // 0 = Horizontal, 1 = Hor2Vert 2 = Vert
    float vX, vZ;
    float vertSpeed = 30f;
    float rotateSpeed = 15f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (runState == 0)
        {

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            characterController.Move(moveDirection * Time.deltaTime);

            if (canMove)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                playerCamera.transform.localPosition = new Vector3(0, 7f, -27f);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

                Vector3 rayDir = playerCamera.transform.position - transform.position;
                float maxCameraDistance = 30f;
                if (Physics.Raycast(transform.position, rayDir, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("CameraCollision")))
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    float distance = hit.distance;
                    Debug.Log("Distance: " + distance);
                    //distance = Mathf.Clamp(distance, 0.5f, maxCameraDistance);
                    if (distance < maxCameraDistance)
                    {
                        playerCamera.transform.position = hit.point-rayDir.normalized*1.5f;
                    }
                }
            }
        }
        else if (runState == 1)
        {
            float movementDirectionY = moveDirection.y;
            Vector3 targetPosition = new Vector3(vX, transform.position.y, vZ);
            Vector3 direction = (targetPosition - transform.position);
            direction.y = 0; // Don't affect y
            if (direction.magnitude > 6f)
            {
                direction = direction.normalized * 50f;
            }
            else
            {
                runState = 2;
            }
            moveDirection = direction;
            if (characterController.isGrounded)
            {
                Debug.Log("Grounded");
                moveDirection.y = jumpPower;
            }
            else
            {
                moveDirection.y = movementDirectionY;
                moveDirection.y -= gravity * Time.deltaTime;
            }
            Debug.Log("VertMove: " + moveDirection);
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else if (runState == 2)
        {
            // vX, vZ 기준으로 좌우 입력시 회전
            Vector3 targetDirection = new Vector3(vX, transform.position.y, vZ) - transform.position;
            targetDirection.y = 0;
            if (targetDirection.sqrMagnitude > 0.01f)
            {
                float horizontalInput = Input.GetAxis("Horizontal");
                float rotationAmount = horizontalInput * rotateSpeed;
                Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.up);
                targetDirection = rotation * targetDirection;

                transform.rotation = Quaternion.LookRotation(targetDirection);
                moveDirection = targetDirection.normalized * walkSpeed;
                moveDirection.y = - vertSpeed;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VertStart"))
        {
            runState = 1;
            vX = other.gameObject.transform.position.x;
            vZ = other.gameObject.transform.position.z;
            Debug.Log("VertStart, vX: " + vX + ", vZ: " + vZ);
        }
        else if (other.gameObject.CompareTag("VertEnd"))
        {
            runState = 0;
            vX = 0;
            vZ = 0;
        }
    }
}