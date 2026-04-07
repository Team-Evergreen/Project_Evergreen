using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public WeaponData weaponData; // 사용할 무기 데이터
    private Transform firePoint;  // 총알이 생성될 위치

    private PlayerController playerController;
    private float timer;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        firePoint = transform.Find("FirePoint");
    }

    private void Update()
    {
        if (weaponData == null || weaponData.projectilePrefab == null) 
            return;

        timer += Time.deltaTime;
        if (timer >= weaponData.fireRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void Fire()
    {
        Vector2 lookDir = playerController.LookDirection;

        // 만약 lookDir이 Zero라면 (게임을 막 시작했을 때) 기본 방향 설정
        if (lookDir == Vector2.zero) 
            lookDir = Vector2.right;

        GameObject projectileObj = Instantiate(weaponData.projectilePrefab, firePoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        projectileObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (projectileObj.TryGetComponent(out Projectile projectile))
        {
            projectile.Setup(weaponData.damage, weaponData.speed, weaponData.lifetime);
        }

        Debug.Log($"현재 발사 방향: {lookDir}, 계산된 각도: {angle}");
    }
}
