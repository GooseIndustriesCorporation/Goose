using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10; // Количество урона

    void Update()
    {
        // Наносим урон при нажатии клавиши "J"
        if (Input.GetKeyDown(KeyCode.J))
        {
            Health health = FindObjectOfType<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }

        // Восстанавливаем здоровье при нажатии клавиши "H"
        if (Input.GetKeyDown(KeyCode.H))
        {
            Health health = FindObjectOfType<Health>();
            if (health != null)
            {
                health.Heal(damageAmount);
            }
        }

    }
    //private void OnTriggerEnter(Collider item)
    //{
    //    // Проверяем, является ли объект игроком
    //    if (item.CompareTag("Player"))
    //    {
    //        // Получаем компонент игрока 
    //        Health health = item.GetComponent<Health>();
    //        if (health != null)
    //        {
    //            health.TakeDamage(damageAmount);
    //        }
    //    }
    //}
}