using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AOE_Data", menuName = "AOE Data")]
public class AOE_Data : ScriptableObject
{
    [Header("AOE Attributes")]
    public float range = 5.0f;
    public float detonationTimer = 6.0f;
    public float flashRateSpeedUp = 1.25f;
    public float damage = 0.0f;
    public float stunDuration = 0.0f;

    [Header("Audio")]
    public AudioClip beepSound;
    public AudioClip detonationAudio;
    public AudioClip activationAudio;
    public AudioClip grabAudio;
    public AudioClip holsterAudio;

    [Header("Animator Attributes")]
    public string activateParam = "Activate";
    public ParticleSystem[] particles;

    [Header("Layer Mask")]
    public string[] layerMask = { "Player", "Enemy", "Default", "Teammate" };

}
