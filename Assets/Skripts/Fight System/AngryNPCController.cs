using UnityEngine;
using UnityEngine.AI;

public class AngryNPCController : MonoBehaviour
{
    public Transform player;          // ������ �� ������
    public float visionRadius = 10f; // ������ ���������
    public float visionAngle = 90f;  // ���� ������
    NavMeshAgent agent;       // ������������� �����
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
                // �������� �� �����������
                if (!Physics.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer))
                {
                    //����� �����, ��������� � ����
                    agent.SetDestination(player.position);
                    //anim = agent.GetComponent<Animation>();
                    //anim.Play();
                }
            }
        }
    }
}