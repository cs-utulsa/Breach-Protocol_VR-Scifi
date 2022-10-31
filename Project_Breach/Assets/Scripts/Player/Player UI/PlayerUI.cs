using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Tracked Data")]
    public Health playerHealth;
    public Shield playerShield;

    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider shieldSlider;
    public TextMeshProUGUI primaryText;
    public TextMeshProUGUI secondaryText;

    [Header("Inventory Sockets")]
    public SocketTagCheck primarySocket;
    public SocketTagCheck secondarySocket;

    private bool primaryFull;
    private bool secondaryFull;
    private void Start()
    {
        playerHealth = GetComponentInParent<Health>();
        playerShield = GetComponentInParent<Shield>();
        healthSlider.maxValue = playerHealth.playerData.maxHealth;
        shieldSlider.maxValue = playerShield.playerData.maxShield;
        primaryFull = false;
        secondaryFull = false;
    }

    private void Update()
    {
        shieldSlider.value = playerShield.getShieldCharge();
        healthSlider.value = playerHealth.getCurrentHealth();
    }

    public void TogglePrimaryUI()
    {
        primaryFull = !primaryFull;
        if (primaryFull)
        {
            primaryText.text = "You have a primary.";
        }
        else
        {
            primaryText.text = "No primary.";
        }
    }

    public void ToggleSecondaryUI()
    {
        secondaryFull = !secondaryFull;
        if (secondaryFull)
        {
            secondaryText.text = "You have a secondary.";
        }
        else
        {
            secondaryText.text = "No secondary.";
        }
    }
}
