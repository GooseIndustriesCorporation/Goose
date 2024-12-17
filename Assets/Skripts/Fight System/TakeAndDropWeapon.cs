using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeAndDropWeapon : MonoBehaviour
{
    public GameObject weaponHolder;
    public Camera playerCamera;
    public float rayDistance = 5.2f;
    public string weaponTag = "Weapon";
    public Material highlightMaterial;
    private Material originalMaterial;

    private GameObject currentWeapon;
    private bool isHoldingWeapon = false;
    private GameObject highlightedWeapon;

    void Update()
    {
        if (isHoldingWeapon)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                DropWeapon();
            }
        }
        else
        {
            CheckForWeaponPickup();
        }
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * rayDistance, Color.red);

    }

    void CheckForWeaponPickup()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Debug.Log($"Hit Object: {hit.collider.name}");
            if (hit.collider.CompareTag(weaponTag))
            {
                // Если наведённый объект отличается от текущего выделенного
                if (highlightedWeapon != hit.collider.gameObject)
                {
                    RestoreOriginalMaterial();
                    HighlightWeapon(hit.collider.gameObject);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PickupWeapon(hit.collider.gameObject);
                }
            }
            else
            {
                // Если объект не имеет тега оружия, сбрасываем выделение
                RestoreOriginalMaterial();
            }
        }
        else
        {
            // Если луч не попал в объект, сбрасываем выделение
            RestoreOriginalMaterial();
        }
    }

    void HighlightWeapon(GameObject weapon)
    {
        // Если уже есть выделенный объект, сбрасываем его материал
        if (highlightedWeapon != null && highlightedWeapon != weapon)
        {
            RestoreOriginalMaterial();
        }

        var renderer = weapon.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = highlightMaterial;
            highlightedWeapon = weapon;
        }
    }

    void RestoreOriginalMaterial()
    {
        if (highlightedWeapon != null)
        {
            var renderer = highlightedWeapon.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            highlightedWeapon = null;
        }
    }

    void PickupWeapon(GameObject weapon)
    {
        RestoreOriginalMaterial();
        currentWeapon = weapon;

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        currentWeapon.transform.SetParent(weaponHolder.transform);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;

        isHoldingWeapon = true;
    }

    void DropWeapon()
    {
        if (currentWeapon == null) return;

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(playerCamera.transform.forward * 5f, ForceMode.Impulse);
        }

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        currentWeapon.transform.SetParent(null);
        currentWeapon = null;
        isHoldingWeapon = false;
    }
}