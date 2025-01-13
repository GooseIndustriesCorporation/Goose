using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // ������ �� ������
    public float visionRadius = 10f;   // ������ ���������
    public float visionAngle = 90f;    // ���� ������
    public float attackDistance = 1f;  // ���������� ��� �����
    public float randomMoveRadius = 5f; // ������ ��� ���������� ��������
    public float attackDelay = 1f;     // �������� ����� ������ (���)
    public float pauseDuration = 2f;   // ������������ ����� ��� ��������� ��������
    public float minPauseTime = 1f;    // ����������� ����� ����� �����������
    public float maxPauseTime = 3f;    // ������������ ����� ����� �����������

    private NavMeshAgent agent;        // ������������� �����
    private bool isAttacking = false;  // ���� �����
    private Vector3 randomDestination; // ���� ��� ���������� ��������
    private bool isPausing = false;    // ���� ���������


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Debug.DrawRay(transform.position, directionToPlayer.normalized * visionRadius, Color.red);
        if (distanceToPlayer < visionRadius)
        {
            Debug.Log("����� � ������� ���������.");
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle / 2)
            {
                Debug.Log("����� � ���� ������.");
                agent.destination = player.position;
                //anim = agent.GetComponent<Animation>();
                //anim.Play();

            }
            if (distanceToPlayer < attackDistance && !isAttacking)
            {
                StartCoroutine(AttackPlayer());
            }
        }
        else
        {
            MoveRandomly();
        }
    }
    private void MoveRandomly()
    {
        if (!agent.hasPath)
        {
            randomDestination = new Vector3(
                transform.position.x + Random.Range(-randomMoveRadius, randomMoveRadius),
                transform.position.y,
                transform.position.z + Random.Range(-randomMoveRadius, randomMoveRadius)
            );
            agent.SetDestination(randomDestination);
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        agent.isStopped = true; // ������������� NPC ����� ������
        Debug.Log("NPC ��������������� � ��������� ���������...");
        
        yield return new WaitForSeconds(attackDelay); // ������� 1 �������

        Debug.Log("NPC ������� ���� �� ������!");       

        // ���������� NPC � ��������
        agent.isStopped = false;
        isAttacking = false;
    }

    private IEnumerator RandomPauseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));

            if (!isAttacking && !agent.isStopped) // ��������������� ������ ���� NPC �� �������
            {
                isPausing = true;
                agent.isStopped = true;
                Debug.Log("NPC ����������� �� �����...");

                yield return new WaitForSeconds(pauseDuration); // �����

                isPausing = false;
                agent.isStopped = false;
                Debug.Log("NPC ��������� ��������...");
            }
        }
    }
}