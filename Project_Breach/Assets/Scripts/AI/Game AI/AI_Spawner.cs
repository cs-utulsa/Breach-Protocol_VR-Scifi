using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AI_Spawner : MonoBehaviour //, IPunObservable
{
    [Header("Spawnable AI Data")]
    public Transform spawnPoint;
    public AI_Spawner_Data spawnerData;
    public PhotonView photonView;

    [SerializeField] private float timer;
    [SerializeField] private int aiAlive;

    private void Start()
    {
        timer = 10.0f;
        aiAlive = 0;
        photonView = GetComponent<PhotonView>();
    }

    private void FixedUpdate()
    {
        if (this.isActiveAndEnabled && timer < 0 && aiAlive < spawnerData.maxSpawnable && PhotonNetwork.IsMasterClient)
        {
            timer = spawnerData.spawntime;
            //GameObject spawnedAI = Instantiate(spawnerData.spawnableAI[Random.Range(0, spawnerData.spawnableAI.Length - 1)], spawnPoint);
            GameObject spawnedAI = PhotonNetwork.Instantiate(spawnerData.spawnableAI[Random.Range(0,spawnerData.spawnableAI.Length-1)].name, spawnPoint.position, Quaternion.identity);
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

    /*
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(aiAlive);
        } else if (stream.IsReading)
        {
            aiAlive = (int) stream.ReceiveNext();
        }
    }
    */
}
