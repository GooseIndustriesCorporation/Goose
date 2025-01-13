using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // Ссылка на игрока
    public float visionRadius = 10f;   // Радиус видимости
    public float visionAngle = 90f;    // Угол зрения
    public float attackDistance = 1f;  // Расстояние для удара
    public float randomMoveRadius = 5f; // Радиус для случайного движения
    public float attackDelay = 1f;     // Задержка перед атакой (сек)
    public float pauseDuration = 2f;   // Длительность паузы при рандомном движении
    public float minPauseTime = 1f;    // Минимальное время между остановками
    public float maxPauseTime = 3f;    // Максимальное время между остановками

    private NavMeshAgent agent;        // Навигационный агент
    private bool isAttacking = false;  // Флаг атаки
    private Vector3 randomDestination; // Цель для случайного движения
    private bool isPausing = false;    // Флаг остановки


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
            Debug.Log("Игрок в радиусе видимости.");
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
            if (angleToPlayer < visionAngle / 2)
            {
                Debug.Log("Игрок в угле зрения.");
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
        agent.isStopped = true; // Останавливаем NPC перед атакой
        Debug.Log("NPC останавливается и готовится атаковать...");
        
        yield return new WaitForSeconds(attackDelay); // Ожидаем 1 секунду

        Debug.Log("NPC наносит удар по игроку!");       

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
                Debug.Log("NPC остановился на месте...");

                yield return new WaitForSeconds(pauseDuration); // Пауза

                isPausing = false;
                agent.isStopped = false;
                Debug.Log("NPC продолжил движение...");
            }
        }
    }
}