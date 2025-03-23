using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcTrigger2 : MonoBehaviour
{
    public dialogue dialogueScript;
    public GameObject dialogueUI;
    public bool trigger = false;
    private collectibleItem item;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && (item.Condition()) && trigger == false)
        {

            dialogueUI.SetActive(true);
            dialogueScript.StartDialogue();
             trigger = true;
        }
    } 

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueUI.activeInHierarchy) dialogueUI.SetActive(false);
        }

    }
    private void Start()
    {
        item = FindObjectOfType<collectibleItem>();
    }
}
