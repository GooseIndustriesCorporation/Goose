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

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCamera == null) return;

        Quaternion newRotation = playerCamera.rotation;
        newRotation.x = 0;
        newRotation.z = 0; // Убираем вертикальную составляющую
        newRotation.Normalize();

        // Плавное вращение объекта к направлению камеры
        Quaternion smoothedRotation = Quaternion.Lerp(
            rb.rotation, // Текущая ориентация объекта
            newRotation, // Целевая ориентация камеры
            rotationSpeed * Time.deltaTime // Скорость сглаживания
        );

        // Применяем сглаженное вращение
        rb.MoveRotation(smoothedRotation);
    }
}
