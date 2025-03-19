using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damageAmount = 10; // ���������� �����

    void Update()
    {
        // ������� ���� ��� ������� ������� "J"
        if (Input.GetKeyDown(KeyCode.J))
        {
            Health health = FindObjectOfType<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }

        // ��������������� �������� ��� ������� ������� "H"
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
    //    // ���������, �������� �� ������ �������
    //    if (item.CompareTag("Player"))
    //    {
    //        // �������� ��������� ������ 
    //        Health health = item.GetComponent<Health>();
    //        if (health != null)
    //        {
    //            health.TakeDamage(damageAmount);
    //        }
    //    }
    //}
}