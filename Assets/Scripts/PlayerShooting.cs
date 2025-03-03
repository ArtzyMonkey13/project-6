using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Camera playerCamera;          // Camera from which the player shoots
    public GameObject bulletPrefab;      // Bullet prefab to instantiate
    public Transform shootingPoint;      // The point from where bullets will be fired (e.g., player's gun barrel)
    public float shootingRange = 100f;   // Max range of the bullet
    public float damage = 10f;           // The damage dealt when shot
    public float fireRate = 0.5f;       // Time between shots (in seconds)
    private float nextFireTime = 0f;     // Timer to control firing rate

    public LayerMask enemyLayer;         // Layer to specify the enemy objects

    void Update()
    {
        // Detect when the "G" key is pressed (instead of mouse button)
        if (Input.GetKeyDown(KeyCode.G) && Time.time >= nextFireTime) 
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Shoot()
    {
        // Instantiate the bullet prefab at the shooting point
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Apply force to make the bullet move forward
        bulletRb.AddForce(playerCamera.transform.forward * 20f, ForceMode.VelocityChange);

        // Perform a raycast to detect if the bullet hits an enemy
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, shootingRange, enemyLayer))
        {
            // Check if the hit object has an EnemyAI script (enemy)
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.OnShot(); // Call the method to slow down the enemy
                Debug.Log("Enemy hit!");
            }
        }
    }
}
