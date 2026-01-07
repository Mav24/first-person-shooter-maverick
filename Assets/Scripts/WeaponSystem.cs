using UnityEngine;

/// <summary>
/// Base class for weapons in the game
/// Handles shooting, reloading, and ammo management
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName = "Pistol";
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.5f; // Time between shots
    public int maxAmmo = 12;
    public int currentAmmo;
    public float reloadTime = 2f;
    
    [Header("Weapon Type")]
    public bool isAutomatic = false;
    
    [Header("Effects")]
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    
    [Header("Audio")]
    public AudioClip fireSound;
    public AudioClip reloadSound;
    
    [Header("References")]
    public Camera fpsCam;
    
    private float nextTimeToFire = 0f;
    public bool isReloading = false;
    
    private void Start()
    {
        currentAmmo = maxAmmo;
        
        if (fpsCam == null)
        {
            fpsCam = Camera.main;
        }
    }
    
    private void Update()
    {
        // Don't allow shooting while reloading
        if (isReloading) return;
        
        // Check for reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
            return;
        }
        
        // Auto reload when empty
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }
        
        // Handle shooting
        if (isAutomatic)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {
                Shoot();
            }
        }
    }
    
    private void Shoot()
    {
        if (currentAmmo <= 0) return;
        
        nextTimeToFire = Time.time + fireRate;
        currentAmmo--;
        
        // Play effects
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        // Play sound
        if (fireSound != null)
        {
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
        
        // Raycast for hit detection
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log($"Hit: {hit.transform.name}");
            
            // Check what we hit
            if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Pirate") || 
                hit.transform.CompareTag("EmpireSoldier") || hit.transform.CompareTag("SeaCreature"))
            {
                EnemyHealth enemyHealth = hit.transform.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }
            }
            
            // Spawn impact effect
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impact, 2f);
            }
        }
    }
    
    private void StartReload()
    {
        if (isReloading) return;
        if (currentAmmo == maxAmmo) return;
        
        isReloading = true;
        
        // Play reload sound
        if (reloadSound != null)
        {
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
        }
        
        Debug.Log("Reloading...");
        Invoke(nameof(FinishReload), reloadTime);
    }
    
    private void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete!");
    }
}
