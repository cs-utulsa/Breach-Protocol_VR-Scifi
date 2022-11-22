using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using Photon.Pun;

public class AimAtTarget : Wait
{
    protected override void OnStart() {
        // Start a coroutine to ease in the bone weights of the AI.
        // Without this, the AI snaps like aimbot, which doesn't look good.
        base.OnStart();
        context.aiAgent.weaponIK.weight = 0.0f;
        //context.aiAgent.weaponIK.enabled = true;
        context.aiAgent.weaponIK.RPC_EnableWeaponIK();
        context.aiAgent.weaponIK.StartCoroutine(context.aiAgent.weaponIK.EaseInBoneWeight()); // Starts with weight 0 and ends at weight 1.
        
        
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }
        return base.OnUpdate();
    }
}
