using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform exitPortal;
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!isTeleporting && other.CompareTag("Player"))
        {
            isTeleporting = true;
            other.transform.position = exitPortal.position + exitPortal.forward * 1.0f;
            other.transform.rotation = (other.transform.rotation * Quaternion.Inverse(transform.rotation)) * exitPortal.rotation;
            Invoke("ResetTeleport", 0.5f);
        }
    }

    private void ResetTeleport()
    {
        isTeleporting = false;
    }
}