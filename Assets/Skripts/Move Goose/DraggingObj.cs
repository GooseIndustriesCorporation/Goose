using UnityEngine;

public class DraggingObj : MonoBehaviour
{
    public float forceGoose = 5f;
    private bool moveBackwards = false; // ���� �������� �����
    private bool moveForwards = false; // ���� �������� �����
    private CharacterController characterController; // Character Controller ������
    private Rigidbody obj; // Rigidbody ���������������� �������
    public float followDistance = 2f; // ���������� ����� ������� � ��������
    private bool objDrag = false; // ��������, ����� �� ���������� ������
    private bool isDragging = false; // ��������, �������� �� ������
    public Transform playerCamera;
    //MouseRotation scriptToDisable;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //scriptToDisable = GetComponent<MouseRotation>();

        // ���������, ��������� �� ������
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
    }

    void Update()
    {
        // ������������� ����� ��������
        if (Input.GetKeyDown(KeyCode.F) && objDrag && obj != null) // �����
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
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.R))
        {
            isDragging = false;
            moveBackwards = false;
            moveForwards = false;
        }
    }

    void FixedUpdate()
    {
        // ����������� ������� � �������
        if (isDragging && obj != null)
        {
            //scriptToDisable.enabled = false;

            if (moveBackwards) // �������������� ������� �����
            {
                // ������� �� �������
                Vector3 targetPosition = transform.position - transform.forward * followDistance;
                obj.MovePosition(Vector3.Lerp(obj.position, targetPosition, forceGoose * Time.fixedDeltaTime));
            }
            else if (moveForwards) // �������������� ������� �����
            {
                // ������� ����� �������
                Vector3 targetPosition = transform.position + transform.forward * followDistance;
                obj.MovePosition(Vector3.MoveTowards(obj.position, targetPosition, forceGoose * Time.fixedDeltaTime));
            }
        }
        else
        {
            //scriptToDisable.enabled = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // ���� ������ ����� ��� "DraggingObject", ��������� ��� ��������������
        if (hit.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = true;
            obj = hit.collider.attachedRigidbody; // ��������� Rigidbody �������
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ����� ������ ������� �� ���� ��������������, ���������� �����
        if (other.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = false;
            obj = null;
        }
    }
}