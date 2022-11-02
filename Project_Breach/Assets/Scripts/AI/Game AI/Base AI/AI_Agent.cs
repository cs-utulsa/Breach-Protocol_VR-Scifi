using behaviorNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    public AI_Sensor sensor;
    public AI_Health aiHealth;
    public AI_Data aiData;
    public WeaponIK weaponIK;
    public SemiautomaticWeapon weapon;
    public Animator animator;
    public AI_Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sensor = GetComponent<AI_Sensor>();
        aiHealth = GetComponent<AI_Health>();
        weaponIK = GetComponent<WeaponIK>();

        if (weapon == null)
        {
            weapon = GetComponentInChildren<AutomaticWeapon>();
            if (weapon == null)
            {
                weapon = GetComponentInChildren<SemiautomaticWeapon>();
            }
        }
    }

}
