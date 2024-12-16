using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage_lovushka : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("���������� �����, ���������� ��������.")]
    public int damageAmount = 10;

    [Tooltip("��� �������, ������� ����� �������� ����.")]
    public string targetTag = "Player";

    [Tooltip("�������� ����� ��������� ���������� ����� (� ��������).")]
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnTriggerEnter(Collider other)
    {
        // ���������, ��� ������ ����� ������ ���
        if (other.CompareTag(targetTag))
        {
            ApplyDamage(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // ���� ������ ������� � �������, ������� ������������� ����
        if (other.CompareTag(targetTag) && Time.time >= lastDamageTime + damageCooldown)
        {
            ApplyDamage(other);
        }
    }

    private void ApplyDamage(Collider target)
    {
        // ������� �������� ��������� �������� � �������
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time; // ��������� ����� ��������� �����
        }
        else
        {
            Debug.LogWarning($"������ {target.name} �� ����� ���������� Health!");
        }
    }
}
