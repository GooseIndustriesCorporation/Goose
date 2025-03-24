using UnityEngine;
using UnityEngine.UI;

public class npcQuest : MonoBehaviour
{
    public int itemsToCollect = 3;
    private int itemsCollected = 0;
    public bool endDial;
    public GameObject _dialogue;

    void Update()
    {
        if (endDial== true)
        {
            _dialogue.SetActive(false);
            Time.timeScale = 1;

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _dialogue.SetActive(true);
            Time.timeScale = 0;
        }
    }

   public void ItemCollected()
    {
        itemsCollected++;
    }
}