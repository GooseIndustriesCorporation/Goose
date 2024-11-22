using UnityEngine;

public class Move : MonoBehaviour
{
    private float jumpCooldown = 0.1f;
    private float lastJumpTime = 0f;
    public float speed = 5f; // �������� ��������
    public float jumpForce = 5f; // ���� ������
    public float flyForce = 100f; // ��������� �������
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

    void Update()
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

        Vector3 movement = (forward * v + right * h).normalized * speed * Time.deltaTime;
        Vector3 newPosition = rb.position + movement;
        rb.MovePosition(newPosition);

        //if (Input.GetButtonDown("Jump") && onFloor) // ������
        //{
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //}
        //else if (Input.GetButtonDown("Jump") && onFloor && Time.time > lastJumpTime + jumpCooldown)
        //{
        //    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //    onFloor = false;
        //    lastJumpTime = Time.time;
        //}
        //if (!onFloor && Input.GetButton("Jump") && rb.velocity.y < 0)
        //{
        //    rb.AddForce(Vector3.down / flyForce, ForceMode.Acceleration);

        //    // ����������� ������������ �������� �������
        //    if (rb.velocity.y < -2f)
        //    {
        //        rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
        //    }
        //}
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Floor"))
    //    {
    //        onFloor = true;            
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (!collision.gameObject.CompareTag("Floor"))
    //    {
    //        onFloor = false;
    //    }
    //}
}
