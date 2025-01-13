using UnityEngine;

public class RespawnOutOfRange : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnValue;
    void Update()
    {
        if (player.transform.position.y < -spawnValue) RespawnPoint();
    }
    public void RespawnPoint()
    {
        transform.position = spawnPoint.position;
    }

}