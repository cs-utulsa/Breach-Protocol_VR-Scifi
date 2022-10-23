using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class AI_Health : MonoBehaviour
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
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        ragdoll.ActivateRagdoll();
    }

    public bool GetIsDead()
    {
        return isDead;
    }

}
