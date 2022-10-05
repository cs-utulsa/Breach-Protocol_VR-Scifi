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
    private int currentAmmo;

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

    private bool isCharging = false;
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
        if (currentAmmo > 0)
        {
            Shoot();
        }
        else
        {
            source.PlayOneShot(weaponData.emptyClip);
        }
    }

    public virtual void Shoot()
    {
        source.PlayOneShot(weaponData.shootClip);
        currentAmmo--;

        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;

        //foreach (var particle in muzzleFlash) particle.Emit(1);

        var tracer = Instantiate(weaponData.tracerEffect, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);


        // Hit Detection
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask))
        {
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            Debug.Log(hitInfo.transform);
            //impactEffect.transform.position = hitInfo.point;
            //impactEffect.transform.forward = hitInfo.normal;
            //impactEffect.Emit(1);
            tracer.transform.position = hitInfo.point;


            if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Teammate"))
            {
                Debug.Log(hitInfo.transform);
            }
            else if (hitInfo.collider.CompareTag("Enemy"))
            {
                Debug.Log(hitInfo.transform);
            }

        }
    }

    protected virtual void Recharge()
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
}
