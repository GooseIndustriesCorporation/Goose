using UnityEngine;

public class TakeAndDropWeapon : MonoBehaviour
{
    public GameObject weaponHolder;
    public Camera FCPplayerCamera;
    public float rayDistance = 5.2f;
    public string weaponTag = "Weapon";
    public Material highlightMaterial;
    private Material originalMaterial;
    public float rayDistanceFight = 4f;
    public int damageAmount = 10;
    public string targetTag = "Enemy";
    public float damageCooldown = 1f;
    private float lastDamageTime;
    public delegate void WeaponStateChanged(bool isHolding);
    public event WeaponStateChanged OnWeaponStateChanged;

    private GameObject currentWeapon;
    protected bool isHoldingWeapon = false;
    private GameObject highlightedWeapon;

    void Update()
    {
        Ray ray = FCPplayerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistanceFight))
        {
            if (hit.collider.CompareTag(targetTag))
            {
                Debug.Log($"isHoldingWeapon - {isHoldingWeapon}");

                if (isHoldingWeapon && Input.GetMouseButtonDown(0) && Time.time >= lastDamageTime + damageCooldown)
                {
                    DamageEnemy(hit.collider);
                }
            }
        }
        Debug.DrawRay(FCPplayerCamera.transform.position, FCPplayerCamera.transform.forward * rayDistanceFight, Color.red);
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
        Debug.Log($"isHoldingWeapon(TAD) - {isHoldingWeapon}");
    }

    void CheckForWeaponPickup()
    {
        Ray ray = FCPplayerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
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
        OnWeaponStateChanged?.Invoke(isHoldingWeapon);
    }

    void DropWeapon()
    {
        if (currentWeapon == null) return;

        Rigidbody rb = currentWeapon.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(FCPplayerCamera.transform.forward * 5f, ForceMode.Impulse);
        }

        Collider col = currentWeapon.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        currentWeapon.transform.SetParent(null);
        currentWeapon = null;
        isHoldingWeapon = false;
        OnWeaponStateChanged?.Invoke(isHoldingWeapon);
    }
    void DamageEnemy(Collider target)
    {
        // Пробуем получить компонент здоровья у объекта
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damageAmount);
            lastDamageTime = Time.time; // Обновляем время нанесения урона
        }
        else
        {
            Debug.LogWarning($"Объект {target.name} не имеет компонента Health!");
        }
    }
}