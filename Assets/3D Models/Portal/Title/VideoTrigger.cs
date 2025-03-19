using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public GameObject videoObject; // ������ �� ������ Raw Image
    private VideoPlayer videoPlayer;
    private bool hasTriggered = false;

    void Start()
    {
        // �������� ��������� Video Player
        videoPlayer = videoObject.GetComponent<VideoPlayer>();
        // �������� ����� ����������
        videoObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // ���������, ��� � ������� ����� �����, � ����� ��� �� ��������
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            // ���������� �����
            videoObject.SetActive(true);
            // ��������� ���������������
            videoPlayer.Play();
        }
    }
}