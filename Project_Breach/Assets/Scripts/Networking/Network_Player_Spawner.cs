using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Network_Player_Spawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network PlayerLead", transform.position, transform.rotation);
            
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network PlayerDemo", transform.position, transform.rotation);
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network PlayerScout", transform.position, transform.rotation);
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network PlayerTech", transform.position, transform.rotation);
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network PlayerLead", transform.position, transform.rotation);
        }
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }


}
