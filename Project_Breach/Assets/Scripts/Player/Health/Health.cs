using System.Collections;
using System.Collections.Generic;
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

    [Header("Animator")]
    public Animator animator;

    [Header("Runtime Variables")]
    [SerializeField] private float currentHealth;
    [SerializeField] private bool isDNBO;


    // Start is called before the first frame update
    void Start()
    {
        moveProvider = GetComponentInChildren<ActionBasedContinuousMoveProvider>();
        currentHealth = playerData.maxHealth;
        source = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
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
        if (this.tag == "Player")
        {
            moveProvider.enabled = false;
        }
        animator.SetBool("DBNO", isDNBO);
        
    }

    public bool GetDBNO()
    {
        return isDNBO;
    }

    public void Revive()
    {
        if (isDNBO)
        {
            currentHealth = playerData.maxHealth;
            source.PlayOneShot(playerData.playerRevive);
            if (this.tag == "Player")
            {
            moveProvider.enabled = true;
            }
            isDNBO = false;
            animator.SetBool("DBNO", isDNBO);
        }
    }

    public float getCurrentHealth()
    {
        return currentHealth;
    }

    public void Heal(float amount)
    {
        if (currentHealth + amount >= playerData.maxHealth)
        {
            currentHealth = playerData.maxHealth;
        }
        else
        {
            currentHealth += amount;
        }
    }
}
