using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AI_Data", menuName = "AI Data")]
public class AI_Data : ScriptableObject
{
    [Header("AI Attributes")]
    public float maxHealth = 100.0f;
    public float damage = 10.0f;
    public float minShootTimer = 1.0f;
    public float maxShootTimer = 3.0f;
    public float speed = 1.50f;
    

    [Header("AI Sensor")]
    public float scanDistance = 10.0f;
    public float scanAngle = 180f;
    public float scanHeight = 5.0f;
    public int scanFrequency = 30;


    [Header("Audio")]
    public AudioClip beepSound;


    [Header("Animator Attributes")]
    public string speedParam = "Speed";
    public string attackParam = "Attack";

    [Header("Layer Mask")]
    public string[] targetLayers = { "Player", "Teammate" };
    public string[] occlusionLayers = { "Default" };

}
