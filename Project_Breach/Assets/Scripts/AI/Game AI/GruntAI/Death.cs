using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using UnityEditor.Rendering.LookDev;

public class Death : ActionNode
{
    public float timer = 3.0f;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        context.aiAgent.weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        context.aiAgent.weapon.interactable.enabled = true;
        context.aiAgent.weapon.transform.SetParent(null);
        context.aiAgent.weaponIK.enabled = false;
        context.agent.destination = context.aiAgent.transform.position;
        context.agent.speed = 0.0f;
        timer -= Time.deltaTime;
        Debug.Log(timer);
        if (timer <= 0)
        {
            Destroy(context.aiAgent.gameObject);
            return State.Success;
        }
        Debug.Log("Death State Running");
        return State.Running;
    }
}
