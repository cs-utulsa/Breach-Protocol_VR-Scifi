using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Health : MonoBehaviour
{
    public AI_Data aiData;

    private float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = aiData.maxHealth;
    }

}
