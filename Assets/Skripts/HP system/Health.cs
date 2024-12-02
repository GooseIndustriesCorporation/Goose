using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // ������������ ��������
    private int currentHealth; // ������� ��������

    void Start()
    {
        currentHealth = maxHealth; // ������������� �������� ��������
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
    }

    // ����� ��� �������������� ��������
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ������������ ��������
        Debug.Log($"������������� ��������: {currentHealth}");
    }

    // ����� ��� ��������� ������
    private void Die()
    {
        Debug.Log($"{gameObject.name} �����!");
        // ����� ����� �������� ������ ������ (��������, ��������, �������� ������� � �.�.)
        Destroy(gameObject); // ������� ������
    }
}
