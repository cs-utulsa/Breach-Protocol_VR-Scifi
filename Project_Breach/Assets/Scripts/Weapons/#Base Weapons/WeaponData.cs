using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "WeaponData",  menuName ="Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Attributes")]
    public int maxAmmo;
    public float damage;
    public float rechargeTime;

    [Header("Audio")]
    public AudioClip shootClip;
    public AudioClip emptyClip;
    public AudioClip rechargeClip;

    [Header("Animator Attributes")]
    public string shootParam = "Shoot";
    public TrailRenderer tracerEffect;

    [Header("Layer Mask")]
    public string[] layerMask = { "Player", "Enemy", "Default", "Teammate" };

}
