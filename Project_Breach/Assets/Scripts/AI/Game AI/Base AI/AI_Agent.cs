using behaviorNameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Agent : MonoBehaviour
{
    public AI_Sensor sensor;
    public AI_Health aiHealth;
    public AI_Data aiData;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        sensor = GetComponent<AI_Sensor>();
        aiHealth = GetComponent<AI_Health>();
    }

}
