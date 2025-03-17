using UnityEngine;

public class JumpGoose : MonoBehaviour
{
    //private float jumpCooldown = 0.1f;
    //private float lastJumpTime = 0f;
    public float jumpForce; // Сила прыжка
    public float flyForce = 5f; // Ускорение падения
    private float lastRollTime = 0f; // Время последнего уворота
    private float rollCooldown = 1f; // Время восстановления после уворота (переката)
    public float rollingForce = 7f;
    private bool onFloor = false; // Проверка, находится ли объект на земле
    private Rigidbody rb;
    public Transform playerCamera; // Камера игрока (Cinemachine FreeLook)

    // Start is called before the first frame update
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && onFloor) // Прыжок
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (rb.velocity.y > 8)
            rb.velocity = new Vector3(rb.velocity.x, 8f, rb.velocity.z);
        if (!onFloor && Input.GetButton("Jump") && rb.velocity.y < 0) // Медленное падение
        {
            rb.AddForce(Vector3.down / flyForce, ForceMode.Acceleration);

            // Ограничение максимальной скорости падения
            if (rb.velocity.y < -2f)
            {
                rb.velocity = new Vector3(rb.velocity.x, -2f, rb.velocity.z);
            }
        }
        // Механика уворота (перекат)
        if (onFloor && Time.time > lastRollTime + rollCooldown) // Проверка cooldown
        {

            if (Input.GetKey(KeyCode.Q))
            {
                // Перекат вправо
                gameObject.GetComponent<DraggingObj>().enabled = false;
                gameObject.GetComponent<MouseRotation>().enabled = false;
                Vector3 rollDirection = transform.right; // Направление влево относительно персонажа
                rb.AddForce(rollDirection * rollingForce, ForceMode.Impulse);
                lastRollTime = Time.time; // Обновляем время последнего переката
            }
            else if (Input.GetKey(KeyCode.E))
            {
                // Перекат влево
                gameObject.GetComponent<DraggingObj>().enabled = false;
                gameObject.GetComponent<MouseRotation>().enabled = false;
                Vector3 rollDirection = -transform.right; // Направление вправо относительно персонажа
                rb.AddForce(rollDirection * rollingForce, ForceMode.Impulse);
                lastRollTime = Time.time; // Обновляем время последнего переката
            }
        }
        else if (Time.time > lastRollTime + 0.5f) gameObject.GetComponent<DraggingObj>().enabled = true;
    }

    private int floorContacts = 0; // Счетчик контактов с объектами Floor

    private void OnCollisionEnter(Collision collision)
    {
        // Если объект имеет тег "Floor", увеличиваем счетчик контактов
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("DraggingObject"))
        {
            floorContacts++;
            onFloor = true; // Персонаж на земле, если хотя бы один контакт
            Debug.Log("Collision Enter with Floor. Contacts: " + floorContacts);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Если объект имеет тег "Floor", уменьшаем счетчик контактов
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("DraggingObject"))
        {
            floorContacts--;
            Debug.Log("Collision Exit with Floor. Contacts: " + floorContacts);

            // Если больше нет контактов с объектами "Floor", персонаж не на земле
            if (floorContacts <= 0)
            {
                onFloor = false;
                floorContacts = 0; // Убедимся, что счетчик не уйдет в минус
            }
        }
    }


}
