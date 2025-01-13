using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{

    [Header("Audio Sources")]
    public AudioSource currentMusic; // �������� ������� ������
    public AudioClip Music_1; // 1 ���������
    public AudioClip Music_2; // 2 ���������
    int change = 0;

    private void Start()
    {
        ChangeMusic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (change == 0) change = 1;
        else if (change == 1) change = 0;
        // ���������, ��� ������ ����� ��� "Player"
        if (other.CompareTag("Player"))
        {
            ChangeMusic();
        }
    }

    private void ChangeMusic()
    {
        // ���� ������� ������ ������, ��������� �
        if (change == 0)
        {
            if (Music_1 != null)
            {
                currentMusic.clip = Music_1;
                currentMusic.loop = true; // �������� ������������
                currentMusic.Play();
            }
        }


        if (change == 1)
        {
            if (Music_2 != null)
            {
                currentMusic.clip = Music_2;
                currentMusic.loop = true; // �������� ������������
                currentMusic.Play();
            }
        }
    }
}
