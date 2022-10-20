using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGrenade : AOE_Base
{
    protected override void CheckForEffected()
    {
        collidersInRange = Physics.OverlapSphere(transform.position, aoeData.range, mask);
        List<Shield> teamShields = new List<Shield>();
        foreach (Collider c in collidersInRange)
        {
            Debug.Log(c + " is in the explosive range.");
            if (c.TryGetComponent<Shield>(out Shield teammate)){
                Debug.Log("Teammate Detected");
                if (!teamShields.Contains(teammate))
                {
                    teamShields.Add(teammate);
                    float proximity = (this.transform.position - c.transform.position).magnitude;
                    float actualDamage = aoeData.damage * (1 - (proximity / aoeData.range));
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
            }

            // Detect enemy AI in the future.
        }
    }
}
