using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


[System.Serializable]
public class DefaultRoom
{
    public string roomName;
    public int sceneIndex;
    public int maxPlayer;
}


public class Network_Manager : MonoBehaviourPunCallbacks
{
    public GameObject roomUI;
    public List<DefaultRoom> defaultRooms;

    // Start is called before the first frame update
    void Start()
    {
        //ConnectToServer();
    }

    public void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect to Server...");

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server");
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Player has joinned the lobby.");
        roomUI.SetActive(true);
    }

    public void InitializeRoom(int defaultRoomIndex)
    {
        DefaultRoom roomSettings = defaultRooms[defaultRoomIndex];

        //Load Scene
        PhotonNetwork.LoadLevel(roomSettings.sceneIndex);

        //Create the room.
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte) roomSettings.maxPlayer;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom(roomSettings.roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a Room");
        base.OnJoinedRoom();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A New Player Joined the Room");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
