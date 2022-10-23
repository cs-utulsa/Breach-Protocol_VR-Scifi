using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;

public class AimAtTarget : Wait
{
    protected override void OnStart() {
        context.aiAgent.weaponIK.StartCoroutine(context.aiAgent.weaponIK.EaseInBoneWeight());
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }
        base.OnUpdate();
        return State.Success;
    }
}
