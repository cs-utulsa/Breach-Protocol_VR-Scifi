using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Photon.Pun;

public class RaycastWeapon : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponData weaponData = null;
    public Transform raycastOrigin = null;
    protected int currentAmmo;
    public PhotonView photonView;

    [Header("Audio")]
    public AudioSource source = null;

    [Header("Animations")]
    public Animator animator = null;
    public ParticleSystem[] muzzleFlash;
    //public ParticleSystem impactEffect;

    [Header("Hit Detection")]
    Ray ray;
    RaycastHit hitInfo;
    LayerMask layerMask;

    protected bool isCharging = false;
    protected bool triggerHeld = false;
    protected WaitForSeconds regenTick;

    protected virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        currentAmmo = weaponData.maxAmmo;
        layerMask = LayerMask.GetMask(weaponData.layerMask);
        regenTick = new WaitForSeconds(weaponData.rechargeTime / weaponData.maxAmmo);
        isCharging = false;
        ray.origin = raycastOrigin.position;
        photonView = GetComponent<PhotonView>();
    }

    public virtual void TriggerPulled()
    {
        animator.SetTrigger(weaponData.shootParam);
        triggerHeld = true;
        if (currentAmmo > 0 && !isCharging)
        {
            Shoot();
        } 
        else
        {
            source.pitch = 1.0f;
            source.PlayOneShot(weaponData.emptyClip);
        }
    }
    public void TriggerReleased()
    {
        triggerHeld = false;
    }

    protected virtual void Shoot()
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.PlayOneShot(weaponData.shootClip);
        currentAmmo--;
        ray.origin = raycastOrigin.position;
        ray.direction = raycastOrigin.forward;
        BulletRegistration();
    }

    [PunRPC]
    public virtual void AI_Shoot(float xInacc, float yInacc)
    {
        if (currentAmmo > 0 && !isCharging)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
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
        foreach (var particle in muzzleFlash) particle.Emit(1);
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
                    //player.GetComponentInParent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, weaponData.damage);
                }
            }
            else if (hitInfo.collider.CompareTag("Enemy"))
            {
                if (hitInfo.collider.TryGetComponent<Hitbox>(out Hitbox aiHitbox))
                {
                    aiHitbox.OnRaycastHit(weaponData.damage, ray.direction);
                } else if (hitInfo.collider.TryGetComponent<Health>(out Health aiHealth))
                {
                    aiHealth.TakeDamage(weaponData.damage);
                }
            }

        }
        else
        {
            tracer.transform.position = transform.position + transform.forward * 30;
        }
    }

    [PunRPC]
    public virtual void Recharge()
    {
        if (!isCharging)
        {
            isCharging = true;
            source.pitch = 1.0f;
            StartCoroutine(StartRecharge());
        }
    }

    protected virtual IEnumerator StartRecharge()
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

    [PunRPC]
    public virtual void RPC_Shoot()
    {
        photonView.RPC("Shoot", RpcTarget.All);
    }

    [PunRPC]
    public virtual void RPC_Recharge()
    {
        photonView.RPC("Recharge", RpcTarget.All);
    }

    [PunRPC]
    public virtual void RPC_AI_Shoot(float xInacc, float yInacc)
    {
        photonView.RPC("AI_Shoot", RpcTarget.All, xInacc, yInacc);
    }
}
