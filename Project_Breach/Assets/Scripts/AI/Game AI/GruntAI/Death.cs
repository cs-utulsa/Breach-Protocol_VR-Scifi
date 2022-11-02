using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using behaviorNameSpace;
using UnityEngine.XR.Interaction.Toolkit;
using System.Net.Mime;
using Photon.Pun;

public class Death : ActionNode
{
    public float timer = 3.0f;
    public bool deathTriggered = false;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (!context.aiAgent.aiHealth.GetIsDead())
        {
            return State.Failure;
        }

        // Disable the AI Movement.
        context.aiAgent.weapon.interactable.enabled = true;
        context.aiAgent.weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        context.aiAgent.weapon.ammoCounter.text = context.aiAgent.weapon.GetAmmo().ToString();
        context.aiAgent.weapon.transform.SetParent(null);
        context.aiAgent.weaponIK.enabled = false;
        context.agent.destination = context.aiAgent.transform.position;
        context.agent.speed = 0.0f;
        
        if (!deathTriggered && context.aiAgent.spawner != null)
        {
            deathTriggered = true;
            //context.aiAgent.spawner.AiHasDied();
            context.aiAgent.spawner.photonView.RPC("AiHasDied", RpcTarget.AllBuffered);
        }

        // Drop the AI's Weapon.
        if (context.aiAgent.weapon.interactable.CompareTag("Primary Weapon")){
            foreach (XRSimpleInteractable interactable in context.aiAgent.weapon.GetComponentsInChildren<XRSimpleInteractable>())
            {
                interactable.enabled = false;
            }
        } 
        context.aiAgent.weapon.interactable.enabled = true;
        context.aiAgent.weapon.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        context.aiAgent.weapon.transform.SetParent(null);

        // Delete the AI.
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(context.aiAgent.gameObject);
            return State.Success;
        }
        //Debug.Log("Death State Running");
        return State.Running;
    }
}
