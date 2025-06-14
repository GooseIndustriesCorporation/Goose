using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundMask; // ���������� � ����������

    private CharacterController controller;
    private Vector3 velocity;
    private float rotationVelocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // ���������, ��� groundMask �������� � ����������
        if (groundMask.value == 0)
        {
            Debug.LogError("Ground Mask �� ��������! ��������� ���� ����� � ����������.");
        }
    }

    void Update()
    {
        // �������� �� �����
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, controller.height / 2, 0),
                                      groundCheckDistance,
                                      groundMask);

        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            // ������� ������������ ������
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            // �������� (������ �� X � Z)
            Vector3 moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            // ����� ������������ �������� ��� ���������� �� �����
            if (velocity.y < 0)
            {
                velocity.y = -0.5f; // ��������� ������������� �������� ��� "��������" � �����
            }

            // ������
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }

        // ��������� ������������ �������� (������/����������)
        controller.Move(velocity * Time.deltaTime);
    }
}