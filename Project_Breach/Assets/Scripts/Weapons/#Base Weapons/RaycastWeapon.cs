using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class RaycastWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData = null;
    public Transform raycastOrigin = null;
    protected int currentAmmo;

    [Header("Audio")]
    public AudioSource source = null;

    [Header("Animations")]
    public Animator animator = null;
    //public ParticleSystem[] muzzleFlash;
    //public ParticleSystem impactEffect;

    [Header("Hit Detection")]
    Ray ray;
    RaycastHit hitInfo;
    LayerMask layerMask;

    protected bool isCharging = false;
    protected bool triggerHeld = false;
    private WaitForSeconds regenTick;

    protected virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        currentAmmo = weaponData.maxAmmo;
        layerMask = LayerMask.GetMask(weaponData.layerMask);
        regenTick = new WaitForSeconds(weaponData.rechargeTime / weaponData.maxAmmo);
        isCharging = false;
        ray.origin = raycastOrigin.position;
    }

    public void TriggerPulled()
    {
        animator.SetTrigger(weaponData.shootParam);
        triggerHeld = true;
        if (currentAmmo > 0 && !isCharging)
        {
            Shoot();
        } 
        else
        {
            source.PlayOneShot(weaponData.emptyClip);
        }
    }
    public void TriggerReleased()
    {
        triggerHeld = false;
    }
    public virtual void Shoot()
    {
        source.PlayOneShot(weaponData.shootClip);
        currentAmmo--;
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;
        BulletRegistration();
    }
    public virtual void AI_Shoot(float xInacc, float yInacc)
    {
        if (currentAmmo > 0 && !isCharging)
        {
            source.PlayOneShot(weaponData.shootClip);
            currentAmmo--;
            ray.origin = raycastOrigin.position;
            ray.direction = raycastOrigin.forward + new Vector3(Random.Range(-xInacc, xInacc), Random.Range(-yInacc, yInacc));
            BulletRegistration();
        }
        else
        {
            source.PlayOneShot(weaponData.emptyClip);
        }

    }
    private void BulletRegistration()
    {
        //foreach (var particle in muzzleFlash) particle.Emit(1);
        var tracer = Instantiate(weaponData.tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);
        // Hit Detection
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            //impactEffect.transform.position = hitInfo.point;
            //impactEffect.transform.forward = hitInfo.normal;
            //impactEffect.Emit(1);
            tracer.transform.position = hitInfo.point;


            if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Teammate"))
            {
                if (hitInfo.collider.TryGetComponent<Shield>(out Shield player))
                {
                    player.TakeDamage(weaponData.damage);
                }
            }
            else if (hitInfo.collider.CompareTag("Enemy"))
            {
                if (hitInfo.collider.TryGetComponent<Hitbox>(out Hitbox aiHitbox))
                {
                    aiHitbox.OnRaycastHit(weaponData.damage, ray.direction);
                }
            }

        }
        else
        {
            tracer.transform.position = transform.position + transform.forward * 10;
        }
    }

    public virtual void Recharge()
    {
        if (!isCharging)
        {
            isCharging = true;
            StartCoroutine(StartRecharge());
        }
    }

    private IEnumerator StartRecharge()
    {
        isCharging = true;
        currentAmmo = 0;
        source.PlayOneShot(weaponData.rechargeClip);
        while (currentAmmo < weaponData.maxAmmo)
        {
            currentAmmo++;
            yield return regenTick;
        }
        currentAmmo = weaponData.maxAmmo;
        isCharging = false;
    }

    public int GetAmmo()
    {
        return currentAmmo;
    }

    public bool GetIsCharging()
    {
        return isCharging;
    }
}
