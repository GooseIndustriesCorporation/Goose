using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_lovushka : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Количество урона, наносимого ловушкой.")]
    public int damageAmount = 10;

    [Tooltip("Тег объекта, который будет получать урон.")]
    public string targetTag = "Player";

    [Tooltip("Задержка перед повторным нанесением урона (в секундах).")]
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект имеет нужный тег
        if (other.CompareTag(targetTag))
        {
            ApplyDamage(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Если объект остаётся в ловушке, наносим периодический урон
        if (other.CompareTag(targetTag) && Time.time >= lastDamageTime + damageCooldown)
        {
            ApplyDamage(other);
        }
    }

    private void ApplyDamage(Collider target)
    {
        // Пробуем получить компонент здоровья у объекта
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time; // Обновляем время нанесения урона
        }
        else
        {
            Debug.LogWarning($"Объект {target.name} не имеет компонента Health!");
        }
    }
}
