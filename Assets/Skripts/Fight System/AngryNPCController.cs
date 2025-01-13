using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // ������ �� ������
    public Collider rb;
    public float visionRadius = 10f;   // ������ ���������
    public float visionAngle = 180f;    // ���� ������
    public float attackDistance = 1f;  // ���������� ��� �����
    public float randomMoveRadius = 5f; // ������ ��� ���������� ��������
    public float attackDelay = 0.5f;     // �������� ����� ������ (���)
    public float pauseDuration = 2f;   // ������������ ����� ��� ��������� ��������
    public float minPauseTime = 1f;    // ����������� ����� ����� �����������
    public float maxPauseTime = 3f;    // ������������ ����� ����� �����������
    public int damageAmount = 50;
    private float lastDamageTime;

    private NavMeshAgent agent;        // ������������� �����
    private bool isAttacking = false;  // ���� �����
    private Vector3 randomDestination; // ���� ��� ���������� ��������
    private bool isPausing = false;    // ���� ���������


    void Start()
    {
        rb = player.GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Debug.DrawRay(transform.position, directionToPlayer.normalized * visionRadius, Color.red);
        if (distanceToPlayer < visionRadius)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle / 2)
            {
                agent.destination = player.position;
                //anim = agent.GetComponent<Animation>();
                //anim.Play();

            }
            if (distanceToPlayer < attackDistance && !isAttacking)
            {
                StartCoroutine(AttackPlayer(rb, player));
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

    private IEnumerator AttackPlayer(Collider col, Transform player)
    {
        isAttacking = true;
        agent.isStopped = true; // ������������� NPC ����� ������
        
        yield return new WaitForSeconds(attackDelay); // ������� 1 �������

        Vector3 distance = player.position - transform.position;
        float distanceToPlayer = distance.magnitude;
        if (distanceToPlayer < attackDistance && isAttacking)
        {
            ApplyDamage(col);
        }
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

                yield return new WaitForSeconds(pauseDuration); // �����

                isPausing = false;
                agent.isStopped = false;
            }
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