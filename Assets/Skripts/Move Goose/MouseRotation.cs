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

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerCamera == null) return;

        // Получаем ориентацию камеры
        Quaternion targetRotation = playerCamera.rotation;

        // Убираем наклоны по осям X и Z
        targetRotation.x = 0;
        targetRotation.z = 0;

        // Разворачиваем на 180 градусов по оси Y
        targetRotation *= Quaternion.Euler(0, 180, 0);
        // Сглаживаем движение с помощью Slerp
        Quaternion smoothedRotation = Quaternion.Slerp(
            rb.rotation,        // Текущая ориентация объекта
            targetRotation,     // Целевая ориентация
            rotationSpeed * Time.fixedDeltaTime / 4 // Скорость сглаживания
        );

        // Применяем сглаженное вращение
        rb.MoveRotation(smoothedRotation);
    }
}
