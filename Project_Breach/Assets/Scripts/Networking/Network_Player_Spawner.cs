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
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player IK (Captain)", transform.position, transform.rotation);
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player IK (Demo)", transform.position, transform.rotation);
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player IK (Tech)", transform.position, transform.rotation);
        } else if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player IK (Scout)", transform.position, transform.rotation);
        }
        else
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player IK (Captain)", transform.position, transform.rotation);
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }


}
