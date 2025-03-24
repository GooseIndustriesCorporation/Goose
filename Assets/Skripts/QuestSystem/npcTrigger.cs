using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcTrigger : MonoBehaviour
{
    public  dialogue dialogueScript1; 
    public GameObject dialogueUI1;
    public bool trigger = false;
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && trigger == false)
        {

            dialogueUI1.SetActive(true); 
            dialogueScript1.StartDialogue();
            trigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueUI1.activeInHierarchy) dialogueUI1.SetActive(false);
        }
    }


}