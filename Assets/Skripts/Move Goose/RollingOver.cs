using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingOver : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    public Transform playerCamera;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {

    }
}
