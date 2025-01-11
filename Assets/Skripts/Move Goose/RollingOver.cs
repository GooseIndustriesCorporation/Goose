using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingOver : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    public Transform playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Заморозить вращение объекта

        // Проверяем, привязана ли камера
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera не привязана! Укажите её в инспекторе.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
