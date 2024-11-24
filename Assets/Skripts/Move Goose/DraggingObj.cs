using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DraggingObj : MonoBehaviour
{
    public float forceGoose = 5f;
    private bool moveBackwards = false; // ���� �������� �����
    private bool moveForwards = false; // ���� �������� �����
    private Rigidbody rb; // Rigidbody ������
    private Rigidbody obj; // Rigidbody ���������������� �������
    public float followDistance = 2f; // ���������� ����� ������� � ��������
    private bool objDrag = false; // ��������, ����� �� ���������� ������
    private bool isDragging = false; // ��������, �������� �� ������
    public Transform playerCamera;
    MouseRotation scriptToDisable;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // ���������� �������� ������

        // ���������, ��������� �� ������
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
    }

    void Update()
    {
        // ������������� ����� ��������
        if (Input.GetKeyDown(KeyCode.E) && objDrag && obj != null) // �����
        {
            isDragging = true;
            moveBackwards = true;
            moveForwards = false;
        }
        else if (Input.GetKeyDown(KeyCode.R) && objDrag && obj != null) // �����
        {
            isDragging = true;
            moveForwards = true;
            moveBackwards = false;
        }

        // ��������� ������
        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.R))
        {
            isDragging = false;
            moveBackwards = false;
            moveForwards = false;
        }
    }

    void FixedUpdate()
    {
        scriptToDisable = gameObject.GetComponent<MouseRotation>();
        // ����������� ������� � �������
        if (isDragging && obj != null)
        {            
            scriptToDisable.enabled = false;
            if (moveBackwards) // �������������� ������� �����
            {
                
                obj.MovePosition(Vector3.Lerp(obj.position, rb.position, Time.fixedDeltaTime));
            }
            else if (moveForwards) // �������������� ������� �����
            {
                obj.MovePosition(Vector3.MoveTowards(obj.position, transform.position - transform.forward * followDistance, forceGoose * Time.fixedDeltaTime));
            }
        }
        else
            scriptToDisable.enabled = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        // ���� ������ ����� ��� "DraggingObject", ��������� ��� ��������������
        if (collision.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = true;
            obj = collision.rigidbody; // ��������� Rigidbody �������
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // ����� ������ ������� �� ���� ��������������, ���������� �����
        if (collision.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = false;
            obj = null;
        }
    }
}
