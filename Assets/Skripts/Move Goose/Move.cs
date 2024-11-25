using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f; // �������� ��������
    public float jumpForce = 5f; // ���� ������
    private bool onFloor = false; // ��������, ��������� �� ������ �� �����
    private Rigidbody rb;
    public Transform playerCamera; // ������ ������ (Cinemachine FreeLook)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ���������� �������� �������

        // ���������, ��������� �� ������
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
    }

    void FixedUpdate()
    {
        // �������� �� ����� ����� Physics.Raycast
        onFloor = Physics.Raycast(transform.position, Vector3.down, 0.6f);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera �� �������! �������� ����������.");
            return;
        }

        Vector3 forward = playerCamera.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = playerCamera.right;
        right.y = 0;
        right.Normalize();

        Vector3 movement = (forward * v + right * h).normalized * speed * (Time.fixedDeltaTime / 4);
        Vector3 newPosition = rb.position + movement;
        rb.MovePosition(newPosition);
    }
}
