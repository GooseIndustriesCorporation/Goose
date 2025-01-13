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
        }
    }
}