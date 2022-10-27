using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGrenade : AOE_Base
{
    List<Shield> teamShields = new List<Shield>();
    List<AI_Health> aiHealths = new List<AI_Health>();
    protected override void CheckForEffected()
    {
        collidersInRange = Physics.OverlapSphere(transform.position, aoeData.range, mask);
        foreach (Collider c in collidersInRange)
        {
            //Debug.Log(c + " is in the explosive range.");
            if (c.TryGetComponent<Shield>(out Shield teammate)){
                Debug.Log("Teammate Detected");
                if (!teamShields.Contains(teammate))
                {
                    float proximity = (this.transform.position - c.transform.position).magnitude;
                    float actualDamage = aoeData.damage * (1 - (proximity / aoeData.range));
                    teamShields.Add(teammate);
                    
                    if (actualDamage > teammate.getShieldCharge())
                    {
                        float shieldLeft = teammate.getShieldCharge();
                        teammate.TakeDamage(shieldLeft);
                        teammate.GetComponent<Health>().TakeDamage(actualDamage - shieldLeft);
                    }
                    else
                    {
                        teammate.TakeDamage(actualDamage);
                    }
                }
            } else if (c.TryGetComponent<Hitbox>(out Hitbox hitbox))
            {
                if (!aiHealths.Contains(hitbox.aiHealth))
                {
                    aiHealths.Add(hitbox.aiHealth);
                    float proximity = (this.transform.position - c.transform.position).magnitude;
                    float actualDamage = aoeData.damage * (1 - (proximity / aoeData.range));
                    Vector3 direction = (c.transform.position - this.transform.position).normalized;
                    hitbox.aiHealth.TakeDamage(actualDamage, direction);
                    Debug.Log(actualDamage);
                }
            }            
        }
        teamShields.Clear();
        aiHealths.Clear();
    }
}
