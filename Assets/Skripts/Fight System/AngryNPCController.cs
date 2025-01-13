using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // Ссылка на игрока
    public Collider rb;
    public float visionRadius = 10f;   // Радиус видимости
    public float visionAngle = 180f;    // Угол зрения
    public float attackDistance = 1f;  // Расстояние для удара
    public float randomMoveRadius = 5f; // Радиус для случайного движения
    public float attackDelay = 0.5f;     // Задержка перед атакой (сек)
    public float pauseDuration = 2f;   // Длительность паузы при рандомном движении
    public float minPauseTime = 1f;    // Минимальное время между остановками
    public float maxPauseTime = 3f;    // Максимальное время между остановками
    public int damageAmount = 50;
    private float lastDamageTime;

    private NavMeshAgent agent;        // Навигационный агент
    private bool isAttacking = false;  // Флаг атаки
    private Vector3 randomDestination; // Цель для случайного движения
    private bool isPausing = false;    // Флаг остановки


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
        agent.isStopped = true; // Останавливаем NPC перед атакой
        
        yield return new WaitForSeconds(attackDelay); // Ожидаем 1 секунду

        Vector3 distance = player.position - transform.position;
        float distanceToPlayer = distance.magnitude;
        if (distanceToPlayer < attackDistance && isAttacking)
        {
            ApplyDamage(col);
        }
        // Возвращаем NPC в движение
        agent.isStopped = false;
        isAttacking = false;
    }

    private IEnumerator RandomPauseRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime));

            if (!isAttacking && !agent.isStopped) // Останавливаемся только если NPC не атакует
            {
                isPausing = true;
                agent.isStopped = true;

                yield return new WaitForSeconds(pauseDuration); // Пауза

                isPausing = false;
                agent.isStopped = false;
            }
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