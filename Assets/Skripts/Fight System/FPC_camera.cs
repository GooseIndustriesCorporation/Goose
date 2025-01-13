using UnityEngine;

public class FPC_camera : MonoBehaviour
{
    public Transform playerCamera;
    public Transform cameraFCP;
    public Transform beak;
    // Start is called before the first frame update
    void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
        if (cameraFCP == null)
        {
            Debug.LogError("Player Camera �� ���������! ������� � � ����������.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        cameraFCP.rotation = playerCamera.rotation;
        cameraFCP.position = beak.position;
    }
}
