using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private Rigidbody rb;
    public Transform playerCamera;
    public float rotationSpeed = 20f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // ���������, ��������� �� ������
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCamera == null) return;

        // �������� ���������� ������
        Quaternion targetRotation = playerCamera.rotation;

        // ������� ������� �� ���� X � Z
        targetRotation.x = 0;
        targetRotation.z = 0;

        // ������������� �� 180 �������� �� ��� Y
        targetRotation *= Quaternion.Euler(0, 180, 0);
        // ���������� �������� � ������� Slerp
        Quaternion smoothedRotation = Quaternion.Slerp(
            rb.rotation,        // ������� ���������� �������
            targetRotation,     // ������� ����������
            rotationSpeed * Time.fixedDeltaTime / 4 // �������� �����������
        );

        // ��������� ���������� ��������
        rb.MoveRotation(smoothedRotation);
    }
}
