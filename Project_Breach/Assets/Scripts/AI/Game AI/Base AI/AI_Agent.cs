using behaviorNameSpace;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Agent : MonoBehaviour
{
    public AI_Sensor sensor;
    public AI_Health aiHealth;
    public AI_Data aiData;
    public WeaponIK weaponIK;
    public SemiautomaticWeapon weapon;
    public Animator animator;
    public AI_Spawner spawner;
    public NavMeshAgent navAgent;

    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sensor = GetComponent<AI_Sensor>();
        aiHealth = GetComponent<AI_Health>();
        weaponIK = GetComponent<WeaponIK>();
        photonView = GetComponent<PhotonView>();
        navAgent = GetComponent<NavMeshAgent>();

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
