using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    [Header("Audio Sources")]
    public AudioSource currentMusic; // Источник текущей музыки
    public AudioClip Music_1; // 1 аудиотрек
    public AudioClip Music_2; // 2 аудиотрек
    int change = 0;

    private void Start()
    {
        ChangeMusic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (change == 0) change = 1;
        else if (change == 1) change = 0;
        // Проверяем, что объект имеет тег "Player"
        if (other.CompareTag("Player"))
        {
            ChangeMusic();
        }
    }

    private void ChangeMusic()
    {
        // Если текущая музыка играет, остановим её
        if (change == 0)
        {
            if (Music_1 != null)
            {
                currentMusic.clip = Music_1;
                currentMusic.loop = true; // Включаем зацикливание
                currentMusic.Play();
            }
        }


        if (change == 1)
        {
            if (Music_2 != null)
            {
                currentMusic.clip = Music_2;
                currentMusic.loop = true; // Включаем зацикливание
                currentMusic.Play();
            }
        }
    }
}
