using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnim : MonoBehaviour
{
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1")) animator.SetBool("attack", true);
        else if (Input.GetButtonUp("Fire1")) animator.SetBool("attack", false);
    }
}
