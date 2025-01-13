using UnityEngine;
using UnityEngine.AI;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;          // Ссылка на игрока
    public float visionRadius = 10f; // Радиус видимости
    public float visionAngle = 90f;  // Угол зрения
    NavMeshAgent agent;       // Навигационный агент
    //public Animation anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

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
                    //Игрок виден, двигаться к нему
                    agent.SetDestination(player.position);
                    //anim = agent.GetComponent<Animation>();
                    //anim.Play();
                }
            }
        }
    }
}