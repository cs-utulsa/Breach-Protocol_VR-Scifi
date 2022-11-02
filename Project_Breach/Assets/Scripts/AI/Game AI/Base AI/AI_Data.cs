using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AI_Data", menuName = "AI Data")]
public class AI_Data : ScriptableObject
{
    [Header("AI Attributes")]
    public float maxHealth = 100.0f;
    public float acceleration = 40.0f;
    public float chaseSpeed = 2.25f;
    public float walkspeed = 1.50f;
    public float moveTolerance = 1.0f;
    public float standardStoppingDistance = 0.25f;

    [Header("AI Combat")]
    public float xInaccuracy = .20f;
    public float yInaccuracy = .10f;
    public float minShootTimer = 0.1f;
    public float maxShootTimer = 1.0f;
    public float minAttackStoppingDistance = 2.5f;
    public float maxAttackStoppingDistance = 15.0f;
    public float attackRange = 20.0f;

    [Header("AI Sensor")]
    public float scanDistance = 30.0f;
    public float scanAngle = 180f;
    public float scanHeight = 5.0f;


    [Header("Audio")]
    public AudioClip beepSound;


    [Header("Animator Attributes")]
    public string speedParam = "Speed";
    public string attackParam = "Attack";

    [Header("Layer Mask")]
    public string[] targetLayers = { "Player", "Teammate" };
    public string[] occlusionLayers = { "Default" };

}
