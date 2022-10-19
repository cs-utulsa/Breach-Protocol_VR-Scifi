using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpGrenade : AOE_Base
{
    protected override void CheckForEffected()
    {
        collidersInRange = Physics.OverlapSphere(transform.position, aoeData.range, mask);
        foreach (Collider c in collidersInRange)
        {
            Debug.Log(c + " is in the EMP range.");
            
        }
    }
}
