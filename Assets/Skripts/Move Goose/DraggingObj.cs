using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DraggingObj : MonoBehaviour
{
    public float forceGoose = 5f;
    private bool moveBackwards = false; // Флаг движения назад
    private bool moveForwards = false; // Флаг движения вперёд
    private Rigidbody rb; // Rigidbody игрока
    private Rigidbody obj; // Rigidbody перетаскиваемого объекта
    public float followDistance = 2f; // Расстояние между игроком и объектом
    private bool objDrag = false; // Проверка, можно ли перетащить объект
    private bool isDragging = false; // Проверка, захвачен ли объект
    public Transform playerCamera;
    MouseRotation scriptToDisable;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Заморозить вращение игрока

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    void Update()
    {
        // Устанавливаем флаги движения
        if (Input.GetKeyDown(KeyCode.E) && objDrag && obj != null) // Назад
        {
            isDragging = true;
            moveBackwards = true;
            moveForwards = false;
        }
        else if (Input.GetKeyDown(KeyCode.R) && objDrag && obj != null) // Вперёд
        {
            isDragging = true;
            moveForwards = true;
            moveBackwards = false;
        }

        // Отпустить объект
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
        // Перемещение объекта с игроком
        if (isDragging && obj != null)
        {            
            scriptToDisable.enabled = false;
            if (moveBackwards) // Перетаскивание объекта назад
            {
                
                obj.MovePosition(Vector3.Lerp(obj.position, rb.position, Time.fixedDeltaTime));
            }
            else if (moveForwards) // Перетаскивание объекта вперёд
            {
                obj.MovePosition(Vector3.MoveTowards(obj.position, transform.position - transform.forward * followDistance, forceGoose * Time.fixedDeltaTime));
            }
        }
        else
            scriptToDisable.enabled = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Если объект имеет тег "DraggingObject", разрешаем его перетаскивание
        if (collision.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = true;
            obj = collision.rigidbody; // Сохраняем Rigidbody объекта
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Когда объект выходит из зоны взаимодействия, сбрасываем флаги
        if (collision.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = false;
            obj = null;
        }
    }
}
