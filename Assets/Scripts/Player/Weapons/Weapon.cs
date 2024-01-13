using UnityEngine;

namespace Player.Weapons
{
    public class Weapon : WeaponBase
    {
        [field:SerializeField]
        protected override float ShootingDistance { get; set; } = 100;

        protected override void Shoot()
        {
            var cameraTransform = mainCamera.transform;
            shootingSystem.Emit(1);
            victimShooting.Emit(1);

            var isHitSomethingFromCamera = TryHitWithRaycast(cameraTransform.position, cameraTransform.forward, out var cameraHitInfo); //выстрел из камеры
        
            var hitPoint = isHitSomethingFromCamera
                ? cameraHitInfo.point
                : mainCamera.transform.position + mainCamera.transform.forward * ShootingDistance;
        
            if (isHitSomethingFromCamera)
            {
                Debug.DrawRay(
                    cameraTransform.position,
                    cameraTransform.forward * cameraHitInfo.distance,
                    Color.yellow,
                    10,
                    false);

                if (!TryDoDamage(cameraHitInfo, cameraTransform))
                {
                    var isHitSomethingFromBulletPos = TryHitWithRaycast(bulletPos.position, hitPoint - bulletPos.position, out var bulletHitInfo); //выстрел из дула, если не попали из камеры

                    if (isHitSomethingFromBulletPos)
                    {
                        TryDoDamage(bulletHitInfo, cameraTransform);
                    }
                }
            }

            DrawTrail(hitPoint);
            ShootLaserHit(hitPoint);

            onShot.Invoke();
            lastShotTime = Time.time;
            StartCoroutine(ShowFlash());
        }
    }
}