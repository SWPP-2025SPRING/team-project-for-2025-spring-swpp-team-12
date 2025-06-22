using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerRunningState
{
    Horizontal, // 수평 이동
    Hor2Vert,   // 수평에서 수직으로 전환
    Vertical    // 수직 이동
}

public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    // float crouchSpeed = 3f;
    float walkSpeed = 20f;
    bool isRunning = false;
    float runSpeed = 50f;
    float jumpPower = 20f;
    float gravity = 40f;
    float lookSpeed = 2f;
    float lookXLimit = 70f;
    // float defaultHeight = 4f;
    // float crouchHeight = 2f;
    // float shrinkSpeed = 5f;
    // bool isCrouching = false;
    // float wiggleSize = 0f;

    public PlayerRunningState runState { get; private set; } = PlayerRunningState.Horizontal;
    float vX, vZ;
    float vertSpeed = 30f;
    float rotateSpeed = 30f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private Stamina stamina;
    private Health health;
    private Rigidbody rb;

    private bool canMove = true;

    // Player Fall handler
    private Vector3 lastSafePosition = Vector3.zero; // 마지막 안전한 위치
    private float fallTimeout = 1f;
    private float fallTimer = 0f; // 낙사 타이머

    HashSet<Vector3Int> validChunks;

    // Animation
    Animator animator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        stamina = gameObject.GetComponent<Stamina>();
        health = gameObject.GetComponent<Health>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        validChunks = MapLoader.ChunksHashSet;
        animator = GetComponentInChildren<Animator>();

        // runState = PlayerRunningState.Horizontal;
        animator.SetInteger("i_runState", (int)runState);
    }

    void Update()
    {
        if (runState == PlayerRunningState.Horizontal)
        {

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float vInput = Input.GetAxis("Vertical");
            float hInput = Input.GetAxis("Horizontal");

            bool isRunningKeyPushed = Input.GetKey(KeyCode.LeftShift);

            
            if (isRunningKeyPushed)
            {
                if (stamina.IsStaminaAvailable() && vInput > 0)
                    isRunning = true;
                else
                    isRunning = false;
            }
            else
                isRunning = false;


            if (isRunning)
                stamina.ChangeStamina(Mathf.RoundToInt(Time.deltaTime * -1000), StaminaChangeType.Run);
            else if (!stamina.isStaminaFull())
                stamina.ChangeStamina(Mathf.RoundToInt(Time.deltaTime * 500), StaminaChangeType.Regen);


            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * GameManager.Instance.GetSpeedMultiplier() * vInput : 0;
            float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * GameManager.Instance.GetSpeedMultiplier()* hInput : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedZ);

            if (characterController.isGrounded)
            {
                movementDirectionY = 0f;
                moveDirection.y = 0f;
            }
            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                // Jump
                animator.SetTrigger("t_jump");
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
                // playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                // playerCamera.transform.localPosition = new Vector3(0, 7f, -27f);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

                // Vector3 rayDir = playerCamera.transform.position - transform.position;
                // float maxCameraDistance = 30f;
                // if (Physics.Raycast(transform.position, rayDir, out RaycastHit hit, float.MaxValue, LayerMask.GetMask("CameraCollision")))
                // {
                //     // Debug.Log("Hit: " + hit.collider.name);
                //     float distance = hit.distance;
                //     // Debug.Log("Distance: " + distance);
                //     //distance = Mathf.Clamp(distance, 0.5f, maxCameraDistance);
                //     if (distance < maxCameraDistance)
                //     {
                //         playerCamera.transform.position = hit.point - rayDir.normalized * 1.5f;
                //     }
                // }
            }
        }
        else if (runState == PlayerRunningState.Hor2Vert)
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
                runState = PlayerRunningState.Vertical;
                animator.SetInteger("i_runState", (int)runState);
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
            // Debug.Log("VertMove: " + moveDirection);
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else if (runState == PlayerRunningState.Vertical)
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
                moveDirection.y = -vertSpeed;
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }
        // Debug.Log("Velocity: " + characterController.velocity.magnitude);
        animator.SetFloat("f_speed", new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).magnitude);

        // 낙사 처리
        RaycastHit hit;
        if (characterController.isGrounded &&
            Physics.Raycast(transform.position, Vector3.down, out hit, 15f))
        {
            GameObject groundObject = hit.collider.gameObject;
            // Debug.Log("Standing on: " + groundObject.name);
            if (groundObject.CompareTag("SafeTile"))
            {
                lastSafePosition = transform.position;
                lastSafePosition.y += 10f; // 약간 위로 올려서 안전한 위치로 설정
            }
        }

        Vector3Int currentChunk = GetPlayerChunk(transform.position);
        if (validChunks.Contains(currentChunk))
        {
            fallTimer = 0f; // 유효한 청크에 있으면 타이머 초기화
        }
        else
        {
            Debug.LogWarning("Player is in an invalid chunk: " + currentChunk);
            fallTimer += Time.deltaTime;

            if (fallTimer >= fallTimeout)
            {
                moveDirection = Vector3.zero; // 낙사 타이머가 초과되면 이동 방향 초기화
                health.ChangeHealth(1); // 낙사로 인해 체력 감소
                canMove = false; // 낙사 타이머가 초과되면 이동 불가
                Respawn(); // 낙사 처리
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("VertStart"))
        {
            runState = PlayerRunningState.Hor2Vert;
            animator.SetInteger("i_runState", (int)runState);
            vX = other.gameObject.transform.position.x;
            vZ = other.gameObject.transform.position.z;
            Debug.Log("VertStart, vX: " + vX + ", vZ: " + vZ);
        }
        else if (other.gameObject.CompareTag("VertEnd"))
        {
            runState = PlayerRunningState.Horizontal;
            animator.SetInteger("i_runState", (int)runState);
            vX = 0;
            vZ = 0;
        }
    }

    public void SetCanMove(bool allowMove)
    {
        canMove = allowMove;
    }
    Vector3Int GetPlayerChunk(Vector3 position, int chunkSize = 70)
    {
        return new Vector3Int(
            Mathf.FloorToInt((position.x + chunkSize / 2f) / chunkSize),
            Mathf.FloorToInt((position.y + chunkSize / 2f) / chunkSize),
            Mathf.FloorToInt((position.z + chunkSize / 2f) / chunkSize)
        );
    }
    void Respawn()
    {
        if (lastSafePosition != Vector3.zero)
        {
            characterController.transform.position = lastSafePosition; // 마지막 안전한 위치로 이동
            fallTimer = 0f; // 낙사 타이머 초기화
            Debug.Log("Player respawned at last safe position: " + lastSafePosition);
            StartCoroutine(RespawnDelay(0.3f)); // 딜레이 후 이동 가능
        }
        else
        {
            Debug.LogWarning("No last safe position found for respawn!");
        }
    }
    private IEnumerator RespawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canMove = true; // 딜레이 후 이동 가능
    }
}