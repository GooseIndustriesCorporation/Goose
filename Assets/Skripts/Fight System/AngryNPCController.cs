using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // Ссылка на игрока
    public Collider rb;                // Коллайдер игрока
    public float visionRadius = 10f;   // Радиус видимости NPC
    public float visionAngle = 360f;   // Угол обзора NPC
    public float attackDistance = 1.5f; // Расстояние для атаки
    public float randomMoveRadius = 5f; // Радиус случайного движения
    public float attackDelay = 0.5f;   // Задержка перед атакой (секунды)
    public float pauseDuration = 2f;   // Длительность паузы во время случайного движения
    public float minPauseTime = 5f;    // Минимальное время между случайными движениями
    public float maxPauseTime = 10f;    // Максимальное время между случайными движениями
    public int damageAmount = 50;      // Количество урона, наносимого игроку
    public int health = 100;           // Здоровье NPC

    private NavMeshAgent agent;        // Навигационный агент NPC
    private Animator animator;         // Ссылка на аниматор NPC
    private bool isAttacking = false;  // Флаг атаки
    private bool isPausing = false;    // Флаг паузы
    private Vector3 randomDestination; // Цель для случайного движения
    private float nextAttackTime = 0f; // Время для следующей атаки
    private bool isDead = false;       // Флаг смерти

    public GameObject portal;
    void Start()
    {
        // Инициализация компонентов
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = player.GetComponent<Collider>();

        // Запуск корутины для случайных пауз
        StartCoroutine(RandomPauseRoutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
        if (isDead) portal.SetActive(true); // Если NPC мёртв, ничего не выполняется

        // Определяем направление и расстояние до игрока
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Если игрок в радиусе видимости и NPC его "видит"
        if (distanceToPlayer < visionRadius && IsPlayerVisible(directionToPlayer))
        {
            // Устанавливаем цель навигации на игрока
            agent.destination = player.position;
            animator.SetBool("isWalking", true); // Включаем анимацию движения

            // Если игрок в зоне атаки и пришло время атаковать
            if (distanceToPlayer < attackDistance && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackPlayer(rb, player)); // Выполняем атаку
                nextAttackTime = Time.time + attackDelay; // Устанавливаем задержку перед следующей атакой
            }
        }
        else
        {
            // Случайное движение, если игрок вне зоны видимости
            MoveRandomly();
        }
    }

    private bool IsPlayerVisible(Vector3 directionToPlayer)
    {
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer < visionAngle) // Если игрок в поле зрения
        {
            // Проверяем препятствия через Raycast
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, visionRadius))
            {
                return hit.collider.CompareTag("Player"); // Возвращаем true, если Raycast попал в игрока
            }
        }
        return false; // Игрок не видим
    }

    private void MoveRandomly()
    {
        if (!agent.hasPath && !isPausing) // Если у NPC нет текущего пути и он не на паузе
        {
            // Генерируем случайную точку в пределах радиуса
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * randomMoveRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, randomMoveRadius, NavMesh.AllAreas))
            {
                // Устанавливаем цель навигации в случайную точку
                agent.SetDestination(hit.position);
                animator.SetBool("isWalking", true); // Включаем анимацию движения
            }
        }
    }

    private IEnumerator AttackPlayer(Collider col, Transform player)
    {
        isAttacking = true;             // Устанавливаем флаг атаки
        agent.isStopped = true;         // Останавливаем движение NPC
        animator.SetBool("isAttack", true);  // Устанавливаем флаг атаки в аниматоре
        animator.SetTrigger("Attack");  // Запускаем анимацию атаки

        yield return new WaitForSeconds(attackDelay); // Ждем задержку перед атакой

        // Если игрок все еще в зоне атаки
        Vector3 distance = player.position - transform.position;
        float distanceToPlayer = distance.magnitude;
        if (distanceToPlayer < attackDistance && isAttacking)
        {
            ApplyDamage(col); // Наносим урон игроку
        }

        animator.SetBool("isAttack", false); // Сбрасываем флаг атаки в аниматоре
        agent.isStopped = false;            // Разрешаем движение
        isAttacking = false;                // Сбрасываем флаг атаки
    }

    private IEnumerator RandomPauseRoutine()
    {
        while (true) // Бесконечный цикл
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime)); // Ждем случайное время

            if (!isAttacking && !isDead) // Если NPC не атакует и не мертв
            {
                isPausing = true;             // Устанавливаем флаг паузы
                agent.isStopped = true;       // Останавливаем движение
                animator.SetBool("isWalking", false); // Отключаем анимацию движения

                yield return new WaitForSeconds(pauseDuration); // Ждем паузу

                isPausing = false;            // Сбрасываем флаг паузы
                agent.isStopped = false;     // Разрешаем движение
            }
        }
    }

    private void ApplyDamage(Collider target)
    {
        Health health = target.GetComponent<Health>(); // Получаем компонент здоровья
        if (health != null)
        {
            health.TakeDamage(damageAmount); // Наносим урон
        }
        else
        {
            Debug.LogWarning($"Объект {target.name} не имеет компонента Health!"); // Предупреждение, если компонент отсутствует
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage; // Уменьшаем здоровье NPC
        if (health <= 0 && !isDead)
        {
            Die(); // Если здоровье <= 0, вызываем смерть
        }
    }

    private void Die()
    {
        isDead = true;                   // Устанавливаем флаг смерти
        agent.isStopped = true;          // Останавливаем движение
        animator.SetTrigger("Die");      // Запускаем анимацию смерти
        // Здесь можно добавить логику для удаления объекта или других действий
    }
}
