using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public GameObject videoObject; // Ссылка на объект Raw Image
    private VideoPlayer videoPlayer;
    private bool hasTriggered = false;

    void Start()
    {
        // Получаем компонент Video Player
        videoPlayer = videoObject.GetComponent<VideoPlayer>();
        // Скрываем видео изначально
        videoObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что в триггер вошёл игрок, и видео ещё не запущено
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            // Показываем видео
            videoObject.SetActive(true);
            // Запускаем воспроизведение
            videoPlayer.Play();
        }
    }
}