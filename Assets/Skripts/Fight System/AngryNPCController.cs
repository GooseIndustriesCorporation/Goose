using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // ������ �� ������
    public Collider rb;                // ��������� ������
    public float visionRadius = 10f;   // ������ ��������� NPC
    public float visionAngle = 360f;   // ���� ������ NPC
    public float attackDistance = 1.5f; // ���������� ��� �����
    public float randomMoveRadius = 5f; // ������ ���������� ��������
    public float attackDelay = 0.5f;   // �������� ����� ������ (�������)
    public float pauseDuration = 2f;   // ������������ ����� �� ����� ���������� ��������
    public float minPauseTime = 5f;    // ����������� ����� ����� ���������� ����������
    public float maxPauseTime = 10f;    // ������������ ����� ����� ���������� ����������
    public int damageAmount = 50;      // ���������� �����, ���������� ������
    public int health = 100;           // �������� NPC

    private NavMeshAgent agent;        // ������������� ����� NPC
    private Animator animator;         // ������ �� �������� NPC
    private bool isAttacking = false;  // ���� �����
    private bool isPausing = false;    // ���� �����
    private Vector3 randomDestination; // ���� ��� ���������� ��������
    private float nextAttackTime = 0f; // ����� ��� ��������� �����
    private bool isDead = false;       // ���� ������

    public GameObject portal;
    void Start()
    {
        // ������������� �����������
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = player.GetComponent<Collider>();

        // ������ �������� ��� ��������� ����
        StartCoroutine(RandomPauseRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
        if (isDead) portal.SetActive(true); // ���� NPC ����, ������ �� �����������

        // ���������� ����������� � ���������� �� ������
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // ���� ����� � ������� ��������� � NPC ��� "�����"
        if (distanceToPlayer < visionRadius && IsPlayerVisible(directionToPlayer))
        {
            // ������������� ���� ��������� �� ������
            agent.destination = player.position;
            animator.SetBool("isWalking", true); // �������� �������� ��������

            // ���� ����� � ���� ����� � ������ ����� ���������
            if (distanceToPlayer < attackDistance && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackPlayer(rb, player)); // ��������� �����
                nextAttackTime = Time.time + attackDelay; // ������������� �������� ����� ��������� ������
            }
        }
        else
        {
            // ��������� ��������, ���� ����� ��� ���� ���������
            MoveRandomly();
        }
    }

    private bool IsPlayerVisible(Vector3 directionToPlayer)
    {
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer < visionAngle) // ���� ����� � ���� ������
        {
            // ��������� ����������� ����� Raycast
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, visionRadius))
            {
                return hit.collider.CompareTag("Player"); // ���������� true, ���� Raycast ����� � ������
            }
        }
        return false; // ����� �� �����
    }

    private void MoveRandomly()
    {
        if (!agent.hasPath && !isPausing) // ���� � NPC ��� �������� ���� � �� �� �� �����
        {
            // ���������� ��������� ����� � �������� �������
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * randomMoveRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, randomMoveRadius, NavMesh.AllAreas))
            {
                // ������������� ���� ��������� � ��������� �����
                agent.SetDestination(hit.position);
                animator.SetBool("isWalking", true); // �������� �������� ��������
            }
        }
    }

    private IEnumerator AttackPlayer(Collider col, Transform player)
    {
        isAttacking = true;             // ������������� ���� �����
        agent.isStopped = true;         // ������������� �������� NPC
        animator.SetBool("isAttack", true);  // ������������� ���� ����� � ���������
        animator.SetTrigger("Attack");  // ��������� �������� �����

        yield return new WaitForSeconds(attackDelay); // ���� �������� ����� ������

        // ���� ����� ��� ��� � ���� �����
        Vector3 distance = player.position - transform.position;
        float distanceToPlayer = distance.magnitude;
        if (distanceToPlayer < attackDistance && isAttacking)
        {
            ApplyDamage(col); // ������� ���� ������
        }

        animator.SetBool("isAttack", false); // ���������� ���� ����� � ���������
        agent.isStopped = false;            // ��������� ��������
        isAttacking = false;                // ���������� ���� �����
    }

    private IEnumerator RandomPauseRoutine()
    {
        while (true) // ����������� ����
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime)); // ���� ��������� �����

            if (!isAttacking && !isDead) // ���� NPC �� ������� � �� �����
            {
                isPausing = true;             // ������������� ���� �����
                agent.isStopped = true;       // ������������� ��������
                animator.SetBool("isWalking", false); // ��������� �������� ��������

                yield return new WaitForSeconds(pauseDuration); // ���� �����

                isPausing = false;            // ���������� ���� �����
                agent.isStopped = false;     // ��������� ��������
            }
        }
    }

    private void ApplyDamage(Collider target)
    {
        Health health = target.GetComponent<Health>(); // �������� ��������� ��������
        if (health != null)
        {
            health.TakeDamage(damageAmount); // ������� ����
        }
        else
        {
            Debug.LogWarning($"������ {target.name} �� ����� ���������� Health!"); // ��������������, ���� ��������� �����������
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // ��������� �������� NPC
        if (health <= 0 && !isDead)
        {
            Die(); // ���� �������� <= 0, �������� ������
        }
    }

    private void Die()
    {
        isDead = true;                   // ������������� ���� ������
        agent.isStopped = true;          // ������������� ��������
        animator.SetTrigger("Die");      // ��������� �������� ������
        // ����� ����� �������� ������ ��� �������� ������� ��� ������ ��������
    }
}
