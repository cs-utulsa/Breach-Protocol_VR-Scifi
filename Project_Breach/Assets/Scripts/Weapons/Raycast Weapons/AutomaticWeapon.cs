using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;

public class AutomaticWeapon : SemiautomaticWeapon
{

    [SerializeField] private float lastFired;
    [SerializeField] private float timeBetweenShots;
    protected override void Awake()
    {
        lastFired = 0.0f;
        timeBetweenShots = (1.0f / (weaponData.RPM / 60.0f));
        base.Awake();
    }
    public void FixedUpdate()
    {
        if (lastFired <= timeBetweenShots)
        {
            lastFired += Time.deltaTime;
        }

        if (triggerHeld && (lastFired >= timeBetweenShots) && currentAmmo > 0 && !isCharging)
        {
            photonView.RPC("Shoot", RpcTarget.AllBuffered);
            //Shoot();
        }
    }
    [PunRPC]
    protected override void Shoot()
    {
        lastFired = 0.0f;
        base.Shoot();
    }
}
