using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{

    public int maxHealth = 100; // ������������ ��������
    private int currentHealth; // ������� ��������
    public Slider Bar;
    void Start()
    {
        currentHealth = maxHealth; // ������������� �������� ��������
        Bar.value = currentHealth;
        Bar.maxValue = maxHealth;
    }

    // ����� ��� ��������� �����
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ ��������
        Debug.Log($"������� ��������: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // �������� ����� ������, ���� �������� ����� ����
        }
        Bar.value = currentHealth;
    }

    // ����� ��� �������������� ��������
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ ��������
        Debug.Log($"������� ��������: {currentHealth}");
        Bar.value = currentHealth;
    }
        // ����� ��� ��������� ������
        private void Die()
    {
        Debug.Log($"{gameObject.name} �����!");
        // ����� ����� �������� ������ ������ (��������, ��������, �������� ������� � �.�.)
        Destroy(gameObject); // ������� ������
    }
}
