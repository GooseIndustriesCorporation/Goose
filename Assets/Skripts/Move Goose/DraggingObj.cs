using UnityEngine;

public class DraggingObj : MonoBehaviour
{
    public float forceGoose = 5f;
    private bool moveBackwards = false; // Флаг движения назад
    private bool moveForwards = false; // Флаг движения вперёд
    private CharacterController characterController; // Character Controller игрока
    private Rigidbody obj; // Rigidbody перетаскиваемого объекта
    public float followDistance = 2f; // Расстояние между игроком и объектом
    private bool objDrag = false; // Проверка, можно ли перетащить объект
    private bool isDragging = false; // Проверка, захвачен ли объект
    public Transform playerCamera;
    //MouseRotation scriptToDisable;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //scriptToDisable = GetComponent<MouseRotation>();

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    void Update()
    {
        // Устанавливаем флаги движения
        if (Input.GetKeyDown(KeyCode.F) && objDrag && obj != null) // Назад
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
        if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.R))
        {
            isDragging = false;
            moveBackwards = false;
            moveForwards = false;
        }
    }

    void FixedUpdate()
    {
        // Перемещение объекта с игроком
        if (isDragging && obj != null)
        {
            //scriptToDisable.enabled = false;

            if (moveBackwards) // Перетаскивание объекта назад
            {
                // Позиция за игроком
                Vector3 targetPosition = transform.position - transform.forward * followDistance;
                obj.MovePosition(Vector3.Lerp(obj.position, targetPosition, forceGoose * Time.fixedDeltaTime));
            }
            else if (moveForwards) // Перетаскивание объекта вперёд
            {
                // Позиция перед игроком
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
        // Если объект имеет тег "DraggingObject", разрешаем его перетаскивание
        if (hit.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = true;
            obj = hit.collider.attachedRigidbody; // Сохраняем Rigidbody объекта
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Когда объект выходит из зоны взаимодействия, сбрасываем флаги
        if (other.gameObject.CompareTag("DraggingObject"))
        {
            objDrag = false;
            obj = null;
        }
    }
}