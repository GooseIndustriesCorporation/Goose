using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab; // ������ ������ ���������
    public Transform spawnPoint; // ����� �����������
    public float respawnTime = 3f; // ����� �� �����������

    private bool isDead = false; // ��������� ���������

    private void Update()
    {
        // ������ ����������� ��������� (����� �������� �� ���� ������)
        if (Input.GetKeyDown(KeyCode.Space) && !isDead) // ���������� ��������� ��� ������� �������
        {
            Died();
        }
    }

    public void Died()
    {
        isDead = true; // ������������� ��������� "�����"
        gameObject.SetActive(false); // ������������ ������

        // ��������� �������� ��� �����������
        Respawn();
    }

    private void Respawn()
    {
        transform.position = spawnPoint.position; // ���������� �� ����� �����������;
        gameObject.SetActive(true); // ���������� ������

        isDead = false; // ���������� ��������� "�����"
    }
}