using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using Photon.Pun;

public class AI_Health : MonoBehaviour, IPunObservable
{
    public AI_Data aiData;
    Ragdoll ragdoll;

    private float currentHealth;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        ragdoll = GetComponent<Ragdoll>();
        currentHealth = aiData.maxHealth;
        isDead = false;
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigidBody in rigidBodies)
        {
            if (!rigidBody.CompareTag("Primary Weapon") || !rigidBody.CompareTag("Secondary Weapon"))
            {
                Hitbox hitBox = rigidBody.AddComponent<Hitbox>();
                hitBox.aiHealth = this;
            }
        }
    }

    public void TakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;
        if (currentHealth <= 0.0f)
        {
            ragdoll.ApplyForce(direction.normalized);
            RPC_Die();
        }
    }

    [PunRPC]
    public void Die()
    {
        isDead = true;
        ragdoll.ActivateRagdoll();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float amount)
    {
        currentHealth = amount;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            if (!isDead)
            {
                stream.SendNext(currentHealth);
            }
        } else if (stream.IsReading)
        {
            currentHealth = (float) stream.ReceiveNext();
            if (currentHealth <= 0 && !isDead)
            {
                RPC_Die();
            }
        }
    }

    [PunRPC]
    public void RPC_Die()
    {
        GetComponent<PhotonView>().RPC("Die", RpcTarget.All);
    }
}
