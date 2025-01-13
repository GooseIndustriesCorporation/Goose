using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;           // Ссылка на игрока
    public Collider rb;                // Коллайдер игрока
    public float visionRadius = 10f;   // Радиус видимости NPC
    public float visionAngle = 180f;   // Угол обзора NPC
    public float attackDistance = 1f;  // Расстояние для атаки
    public float randomMoveRadius = 5f; // Радиус случайного движения
    public float attackDelay = 0.5f;   // Задержка перед атакой (секунды)
    public float pauseDuration = 2f;   // Длительность паузы во время случайного движения
    public float minPauseTime = 1f;    // Минимальное время между случайными движениями
    public float maxPauseTime = 3f;    // Максимальное время между случайными движениями
    public int damageAmount = 50;      // Количество урона, наносимого игроку

    private NavMeshAgent agent;        // Навигационный агент NPC
    private Animator animator;         // Ссылка на аниматор NPC
    private bool isAttacking = false;  // Флаг атаки
    private bool isPausing = false;    // Флаг паузы
    private Vector3 randomDestination; // Цель для случайного движения
    private float nextAttackTime = 0f; // Время для следующей атаки

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
        // Определяем направление и расстояние до игрока
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        // Если игрок в радиусе видимости и NPC его "видит"
        if (distanceToPlayer < visionRadius && IsPlayerVisible(directionToPlayer))
        {
            // Устанавливаем цель навигации на игрока
            agent.destination = player.position;
            animator.SetBool("isWalking?", true); // Включаем анимацию движения

            // Если игрок в зоне атаки и пришло время атаковать
            if (distanceToPlayer < attackDistance && Time.time >= nextAttackTime)
            {
                StartCoroutine(AttackPlayer(rb, player)); // Выполняем атаку
                nextAttackTime = Time.time + attackDelay; // Задаем задержку для следующей атаки
            }
        }
        else
        {
            // Случайное движение, если игрок вне зоны видимости
            MoveRandomly();
        }
    }

    // Проверка видимости игрока (учитывая угол зрения и препятствия)
    private bool IsPlayerVisible(Vector3 directionToPlayer)
    {
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer < visionAngle / 2) // Если игрок в поле зрения
        {
            // Выполняем Raycast для проверки препятствий
            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out RaycastHit hit, visionRadius))
            {
                return hit.collider.CompareTag("Player"); // Возвращаем true, если Raycast попал в игрока
            }
        }
        return false; // Игрок не видим
    }

    // Метод для случайного движения NPC
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
                animator.SetBool("isWalking?", true); // Включаем анимацию движения
            }
        }
    }

    // Метод атаки игрока
    private IEnumerator AttackPlayer(Collider col, Transform player)
    {
        isAttacking = true;             // Устанавливаем флаг атаки
        agent.isStopped = true;         // Останавливаем движение NPC
        animator.SetTrigger("Attack");  // Запускаем анимацию атаки

        yield return new WaitForSeconds(attackDelay); // Ждем задержку перед атакой

        // Если игрок все еще в зоне атаки
        Vector3 distance = player.position - transform.position;
        float distanceToPlayer = distance.magnitude;
        if (distanceToPlayer < attackDistance && isAttacking)
        {
            ApplyDamage(col); // Наносим урон игроку
        }

        agent.isStopped = false; // Снимаем остановку движения
        isAttacking = false;     // Сбрасываем флаг атаки
    }

    // Корутина для случайных пауз
    private IEnumerator RandomPauseRoutine()
    {
        while (true) // Бесконечный цикл
        {
            yield return new WaitForSeconds(Random.Range(minPauseTime, maxPauseTime)); // Ждем случайное время

            if (!isAttacking) // Если NPC не атакует
            {
                isPausing = true;             // Устанавливаем флаг паузы
                agent.isStopped = true;       // Останавливаем движение
                animator.SetBool("isWalking?", false); // Отключаем анимацию движения

                yield return new WaitForSeconds(pauseDuration); // Ждем паузу

                isPausing = false;            // Сбрасываем флаг паузы
                agent.isStopped = false;     // Разрешаем движение
            }
        }
    }

    // Метод для нанесения урона игроку
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
}
