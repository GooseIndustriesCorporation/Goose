using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Transform spawnPoint;
    public Slider bar;

    void Start()
    {
        currentHealth = maxHealth;
        bar.value = currentHealth;
        bar.maxValue = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
       
        Debug.Log($"Текущее здоровье: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        bar.value = currentHealth;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ограничиваем здоровье
        
        Debug.Log($"Текущее здоровье: {currentHealth}");
        bar.value = currentHealth;
    }


    private void Die()
    {
        Debug.Log($"{gameObject.name} погиб!");
        //Destroy(gameObject);
        transform.position = spawnPoint.position; // Перемещаем на точку возрождения;
        currentHealth = maxHealth;
        bar.value = currentHealth;
    }
}
