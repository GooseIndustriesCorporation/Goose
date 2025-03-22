using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dialogue: MonoBehaviour
{
    public string[] lines;
    public float speedText;
    public Text dialogueText;
    
    public int index;

    void Start()
    {
         dialogueText.text = string.Empty;
         StartDialogue();
    }

   public void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(speedText);
        }
    }
    public void scipTextClick()
    {
        if (dialogueText.text == lines[index])
        {
            NextLines();
        }
        else
        {
            StopAllCoroutines();
            dialogueText.text = lines[index];
        }
    }
    void NextLines()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        }
    }
}