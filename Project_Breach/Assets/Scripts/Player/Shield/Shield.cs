using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Shield : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;
    public Health playerHealth;

    [Header("Audio")]
    public AudioSource source;

    [Header("Runtime Variables")]
    [SerializeField] private float currentShield;
    [SerializeField] private float timeFromLastHit;



    // Start is called before the first frame update
    void Start()
    {
        currentShield = playerData.maxShield;
        source = GetComponent<AudioSource>();
        playerHealth = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        if (CanRegenerate())
        {
            RegenerateShield();
        }
    }

    private bool CanRegenerate() {
        if (timeFromLastHit < playerData.maxRegenTimer)
        {
            timeFromLastHit += Time.fixedDeltaTime;
            return false;
        }
        else
        {
            if (currentShield < playerData.maxShield)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void RegenerateShield()
    {
        if (currentShield < playerData.maxShield)
        {
            currentShield += playerData.regenerationRate;
        }
        else
        {
            currentShield = playerData.maxShield;
        }
    }


    public void TakeDamage(float value)
    {
        if (currentShield > 0)
        {
            source.PlayOneShot(playerData.shieldHit);
            if (value >= currentShield)
            {
                currentShield = 0.0f;
            }
            else
            {
                currentShield -= value;
            }
        }
        else
        {
            playerHealth.TakeDamage(value);
        }
        timeFromLastHit = 0.0f;
    }
}