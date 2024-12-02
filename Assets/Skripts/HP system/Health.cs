using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Максимальное здоровье
    private int currentHealth; // Текущее здоровье

    void Start()
    {
        currentHealth = maxHealth; // Инициализация текущего здоровья
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье

        Debug.Log($"Текущее здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // Вызываем метод смерти, если здоровье равно нулю
        }
    }

    // Метод для восстановления здоровья
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье
        Debug.Log($"Восстановлено здоровье: {currentHealth}");
    }

    // Метод для обработки смерти
    private void Die()
    {
        Debug.Log($"{gameObject.name} погиб!");
        // Здесь можно добавить логику смерти (например, анимацию, удаление объекта и т.д.)
        Destroy(gameObject); // Удаляем объект
    }
}
