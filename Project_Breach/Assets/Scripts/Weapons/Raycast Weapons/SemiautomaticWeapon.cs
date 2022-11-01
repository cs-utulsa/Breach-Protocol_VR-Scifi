using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SemiautomaticWeapon : RaycastWeapon
{
    [Header("Interactable")]
    public XRBaseInteractable interactable;

    [Header("Weapon UI")]
    public TextMeshProUGUI ammoCounter;

    [Header("Photon")]
    public PhotonView photonView;

    protected override void Awake()
    {
        interactable = GetComponent<XRGrabInteractable>();
        ammoCounter = GetComponentInChildren<TextMeshProUGUI>();
        photonView = GetComponent<PhotonView>();
        base.Awake();
    }

    public virtual void LateUpdate()
    {
        if (interactable.isSelected)
        {
            if (interactable.firstInteractorSelecting.transform.gameObject.TryGetComponent<ExtraActionController>(out ExtraActionController actionController))
            {
                float primaryButtonValue = actionController.primaryButtonAction.action.ReadValue<float>();
                if (primaryButtonValue >= 1.0f && !isCharging)
                {
                    Recharge();
                }
            }
        }
    }

    public override void TriggerPulled()
    {
        animator.SetTrigger(weaponData.shootParam);
        triggerHeld = true;
        if (currentAmmo > 0 && !isCharging)
        {
            photonView.RPC("Shoot", RpcTarget.AllBuffered);
            Shoot();
        }
        else
        {
            source.PlayOneShot(weaponData.emptyClip);
        }
    }

    protected override void Shoot()
    {
        base.Shoot();
        ammoCounter.text = currentAmmo.ToString();
    }

    protected override IEnumerator StartRecharge()
    {
        isCharging = true;
        currentAmmo = 0;
        source.PlayOneShot(weaponData.rechargeClip);
        while (currentAmmo < weaponData.maxAmmo)
        {
            currentAmmo++;
            ammoCounter.text = currentAmmo.ToString();
            yield return regenTick;
        }
        currentAmmo = weaponData.maxAmmo;
        isCharging = false;
    }
}
