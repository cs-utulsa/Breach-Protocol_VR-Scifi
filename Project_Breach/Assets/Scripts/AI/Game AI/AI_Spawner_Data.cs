using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[CreateAssetMenu(fileName = "AI_Spawner_Data", menuName = "AI Spawner Data")]
public class AI_Spawner_Data : ScriptableObject
{
    [Header("Spawner Attributes")]
    public GameObject[] spawnableAI;
    public float spawntime;
    public float maxSpawnable;
}
