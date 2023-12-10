using UnityEngine;

namespace Player.Weapons
{
    public class Shotgun : WeaponBase
    {
        [SerializeField] private int bulletsCount = 8;
        [SerializeField] private float spread = 0.13f;

        [field:SerializeField]
        protected override float ShootingDistance { get; set; } = 17;

        protected override void Shoot()
        {
            var cameraTransform = mainCamera.transform;
            shootingSystem.Emit(1);
            victimShooting.Emit(1);

            for (var i = 0; i < bulletsCount; i++)
            {
                var direction = cameraTransform.forward + new Vector3(Random.Range(-spread, spread),
                                                                            Random.Range(-spread, spread),
                                                                            Random.Range(-spread, spread));
                var isHitSomethingFromCamera = TryHitWithRaycast(cameraTransform.position, direction, out var cameraHitInfo);
                var hitPoint = isHitSomethingFromCamera
                    ? cameraHitInfo.point
                    : mainCamera.transform.position + direction * ShootingDistance;
                if (isHitSomethingFromCamera)
                {
                    TryDoDamage(cameraHitInfo, cameraTransform);
                }
                DrawTrail(hitPoint);
                ShootLaserHit(hitPoint);
            }
            onShot.Invoke();
            lastShotTime = Time.time;
            StartCoroutine(ShowFlash());
        }
    }
}