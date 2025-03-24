using UnityEngine;

public class collectibleItem : MonoBehaviour
{
    public int itemsToCollect = 3;
    public static int itemsCollected = 0;
    private float rotateSpeed = 100f;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ItemCollected();
            Debug.Log($"Предмет собран! Всего собрано: {itemsCollected}");
            Destroy(gameObject);
        }

     }
    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,0);
        if (Condition())
        {
            Debug.Log("Все предметы собраны!");
            // Дополнительные действия при выполнении условия
        }
    }
    public void ItemCollected()
    {
        itemsCollected++;
    }
    public bool Condition()
    {
        return itemsCollected >= itemsToCollect;

    }
}
