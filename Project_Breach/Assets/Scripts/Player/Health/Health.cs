using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

public class Health : MonoBehaviour
{
    [Header("Player Data")]
    public PlayerData playerData;

    [Header("Player Locomotion")]
    public ActionBasedContinuousMoveProvider moveProvider;


    [Header("Audio")]
    public AudioSource source;

    [Header("Runtime Variables")]
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isDNBO;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = playerData.maxHealth;
        source = GetComponent<AudioSource>();
    }

    public void TakeDamage(float value)
    {
        if (currentHealth > 0 && !isDNBO)
        {
            currentHealth -= value;
            if (currentHealth <= 0 && !isDNBO)
            {
                currentHealth = 0.0f;
                PlayerDBNO();
            }
            else
            {
                source.PlayOneShot(playerData.healthHit);
            }
        }
    }

    private void PlayerDBNO()
    {
        source.PlayOneShot(playerData.playerDNBO);
        isDNBO = true;
        moveProvider.enabled = false;
        
    }

    public bool GetDBNO()
    {
        return isDNBO;
    }

    public void Revive()
    {
        currentHealth = playerData.maxHealth;
        source.PlayOneShot(playerData.playerRevive);
        moveProvider.enabled = true;
        isDNBO = false;
    }
}
