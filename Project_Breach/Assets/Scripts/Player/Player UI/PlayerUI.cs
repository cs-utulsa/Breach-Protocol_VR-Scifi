using Photon.Pun;
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
        secondaryText.color = Color.red;
        primaryText.color = Color.red;
        primaryFull = false;
        secondaryFull = false;
    }

    private void Update()
    {
        if (healthSlider.value != playerHealth.getCurrentHealth())
        {
            healthSlider.value = playerHealth.getCurrentHealth();
        }

        if (shieldSlider.value != playerShield.getShieldCharge())
        {
            shieldSlider.value = playerShield.getShieldCharge();
        }
        
    }

    public void TogglePrimaryUI()
    {
        primaryFull = !primaryFull;
        if (primaryFull)
        {
            primaryText.text = "Primary: Holstered";
            primaryText.color = Color.green;
        }
        else
        {
            primaryText.text = "Primary: N/A";
            primaryText.color = Color.red;
        }
    }

    public void ToggleSecondaryUI()
    {
        secondaryFull = !secondaryFull;
        if (secondaryFull)
        {
            secondaryText.text = "Secondary: Holstered";
            secondaryText.color = Color.green;
        }
        else
        {
            secondaryText.text = "Secondary: N/A";
            secondaryText.color = Color.red;
        }
    }
}
