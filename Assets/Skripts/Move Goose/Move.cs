using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckRadius = 0.5f;
    [SerializeField] private LayerMask groundMask; // Назначайте в инспекторе
    [SerializeField] private Transform groundCheck; // Создайте пустой GameObject в ногах персонажа

    private CharacterController controller;
    private Vector3 velocity;
    private float rotationVelocity;
    private bool isGrounded;

    [SerializeField] private float rotationSmoothTime = 0.5f;
    private Vector3 currentMoveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!groundCheck)
            Debug.LogError("Ground Check transform not assigned!");
    }

    void Update()
    {
        // Улучшенная проверка земли с визуализацией
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        Debug.DrawRay(groundCheck.position, Vector3.down * groundCheckRadius, isGrounded ? Color.green : Color.red);

        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 targetDirection = cameraForward * vertical + cameraRight * horizontal;

        if (targetDirection.magnitude > 0.1f)
        {
            // Плавное изменение текущего направления
            currentMoveDirection = Vector3.Slerp(currentMoveDirection, targetDirection, rotationSmoothTime * Time.deltaTime * 100f);
            currentMoveDirection.Normalize();

            // Плавный поворот персонажа
            float targetAngle = Mathf.Atan2(currentMoveDirection.x, currentMoveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // Движение
            controller.Move(currentMoveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // "Прижимаем" к земле
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            Debug.Log("Jump velocity: " + velocity.y);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Визуализация сферы проверки земли
    private void OnDrawGizmos()
    {
        if (groundCheck)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}