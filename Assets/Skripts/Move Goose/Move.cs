using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 10f; // Скорость движения
    public float jumpForce = 5f; // Сила прыжка
    private bool onFloor = false; // Проверка, находится ли объект на земле
    private Rigidbody rb;
    public Transform playerCamera; // Камера игрока (Cinemachine FreeLook)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Заморозить вращение объекта

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    void FixedUpdate()
    {
        // Проверка на землю через Physics.Raycast
        onFloor = Physics.Raycast(transform.position, Vector3.down, 0.6f);

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (playerCamera == null)
        {
            Debug.LogWarning("Player Camera не указана! Движение невозможно.");
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
