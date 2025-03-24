using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class lineswap : MonoBehaviour
{
    public GameObject text1;
    public GameObject text2;
    private bool istext1 = true;
    public npcQuest npc;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (istext1 == true)
            {
                istext1 = false;
            }
            else istext1 = true;
            npc.endDial = true;
        }
        if (istext1 == true){ 
          text1.SetActive(true);
          text2.SetActive(false);
        }
        else
        {
            text1.SetActive(false);
            text2.SetActive(true);
        }
    }
    
}
