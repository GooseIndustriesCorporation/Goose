using UnityEngine;

public class collectibleItem : npcQuest
{
    private float rotateSpeed = 100f;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            ItemCollected();
            Destroy(gameObject);
        }
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,0);
    }
}
