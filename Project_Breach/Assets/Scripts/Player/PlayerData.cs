using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Player_Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public float maxHealth = 100.0f;
    public float maxShield = 100.0f;
    public float regenerationRate = 0.25f;
    public float maxRegenTimer = 7.0f;

    public AudioClip shieldHit;
    public AudioClip shieldBreak;
    public AudioClip shieldRecharge;
    public AudioClip healthHit;
    public AudioClip playerDNBO;
    public AudioClip playerRevive;
    
    public AudioClip[] movementClips;
}
