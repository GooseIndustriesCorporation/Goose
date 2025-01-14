using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab; // Префаб вашего персонажа
    public Transform spawnPoint; // Точка возрождения
    public float respawnTime = 3f; // Время до возрождения

    private bool isDead = false; // Состояние персонажа

    private void Update()
    {
        // Пример уничтожения персонажа (можно заменить на вашу логику)
        if (Input.GetKeyDown(KeyCode.Space) && !isDead) // Уничтожаем персонажа при нажатии пробела
        {
            Died();
        }
    }

    public void Died()
    {
        isDead = true; // Устанавливаем состояние "мертв"
        gameObject.SetActive(false); // Деактивируем объект

        // Запускаем корутину для возрождения
        Respawn();
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position; // Перемещаем на точку возрождения;
        gameObject.SetActive(true); // Активируем объект

        isDead = false; // Сбрасываем состояние "мертв"
    }
}