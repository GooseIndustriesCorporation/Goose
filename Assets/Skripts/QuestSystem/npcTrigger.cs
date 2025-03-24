using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcTrigger : MonoBehaviour
{
    public  dialogue dialogueScript; 
    public GameObject dialogueUI;
    public bool trigger = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && trigger == false)
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
           // dialogueScript.gameObject.SetActive(false);
        }
    }
}