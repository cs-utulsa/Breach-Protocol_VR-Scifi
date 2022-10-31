using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Spawner : MonoBehaviour
{
    [Header("Spawnable AI Data")]
    public Transform spawnPoint;
    public AI_Spawner_Data spawnerData;

    [SerializeField] private float timer;
    [SerializeField] private int aiAlive;

    private void Start()
    {
        timer = 0.0f;
        aiAlive = 0;
    }

    private void FixedUpdate()
    {
        if (this.isActiveAndEnabled && timer < 0 && aiAlive < spawnerData.maxSpawnable)
        {
            timer = spawnerData.spawntime;
            GameObject spawnedAI = Instantiate(spawnerData.spawnableAI[Random.Range(0, spawnerData.spawnableAI.Length - 1)], spawnPoint);
            spawnedAI.GetComponent<AI_Agent>().spawner = this;
            spawnedAI.transform.parent = null;
            aiAlive++;
        }
        else if  (aiAlive < spawnerData.maxSpawnable){
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            this.enabled = false;
        }
    }

    public void AiHasDied()
    {
        aiAlive--;
        this.enabled = true;
    }
}
