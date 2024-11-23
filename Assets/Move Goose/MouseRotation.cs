using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private Rigidbody rb;
    public Transform playerCamera;
    public float rotationSpeed = 10f;
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
    void Update()
    {
        if (playerCamera == null) return;

        Quaternion newRotation = playerCamera.rotation;
        newRotation.x = 0;
        newRotation.z = 0; // ������� ������������ ������������
        newRotation.Normalize();

        // ������� �������� ������� � ����������� ������
        Quaternion smoothedRotation = Quaternion.Lerp(
            rb.rotation, // ������� ���������� �������
            newRotation, // ������� ���������� ������
            rotationSpeed * Time.deltaTime // �������� �����������
        );

        // ��������� ���������� ��������
        rb.MoveRotation(smoothedRotation);
    }
}
