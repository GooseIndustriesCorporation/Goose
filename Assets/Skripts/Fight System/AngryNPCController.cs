using UnityEngine;
using UnityEngine.AI;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;          // Ссылка на игрока
    public float visionRadius = 10f; // Радиус видимости
    public float visionAngle = 45f;  // Угол зрения
    public NavMeshAgent agent;       // Навигационный агент

    void Update()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer < visionRadius)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < visionAngle / 2)
            {
                // Проверка на препятствия
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer))
                {
                    // Игрок виден, двигаться к нему
                    agent.SetDestination(player.position);
                }
            }
        }
    }
}