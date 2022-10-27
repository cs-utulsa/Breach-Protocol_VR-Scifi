using System.Collections;
using System.Collections.Generic;
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


    private void Start()
    {
        playerHealth = GetComponentInParent<Health>();
        playerShield = GetComponentInParent<Shield>();
        healthSlider.maxValue = playerHealth.playerData.maxHealth;
        shieldSlider.maxValue = playerShield.playerData.maxShield;
    }

    private void Update()
    {
        shieldSlider.value = playerShield.getShieldCharge();
        healthSlider.value = playerHealth.getCurrentHealth();
    }
}
