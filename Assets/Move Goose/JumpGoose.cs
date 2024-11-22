using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpGoose : MonoBehaviour
{
    private float jumpCooldown = 0.1f;
    private float lastJumpTime = 0f;
    public float speed = 5f; // �������� ��������
    public float jumpForce = 5f; // ���� ������
    public float flyForce = 100f; // ��������� �������
    private bool onFloor = false; // ��������, ��������� �� ������ �� �����
    private Rigidbody rb;
    public Transform playerCamera; // ������ ������ (Cinemachine FreeLook)

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
        if (Input.GetButtonDown("Jump") && onFloor) // ������
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (Input.GetButtonDown("Jump") && onFloor && Time.time > lastJumpTime + jumpCooldown)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onFloor = false;
            lastJumpTime = Time.time;
        }
        if (!onFloor && Input.GetButton("Jump") && rb.velocity.y < 0)
        {
            rb.AddForce(Vector3.down / flyForce, ForceMode.Acceleration);

            // ����������� ������������ �������� �������
            if (rb.velocity.y < -2f)
            {
                rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            onFloor = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Floor"))
        {
            onFloor = false;
        }
    }
}
