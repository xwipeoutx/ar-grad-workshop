using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks
{
    [SerializeField] string roomName = "Default Room";
    [SerializeField] Text topText;
    [SerializeField] GameObject[] onJoinActivate = new GameObject[0];

    void Awake()
    {
        StopGame();
        topText.text = $"Look at the image target to join a room";
        PhotonNetwork.AddCallbackTarget(this);
    }

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.J) && !PhotonNetwork.IsConnected)
        {
            Join();
        }
        #endif
    }
    
    public void Join()
    {
        topText.text = $"Connecting to Photon...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnected()
    {
        topText.text = $"Connecting to Master...";
    }

    public void OnConnectedToMaster()
    {
        topText.text = $"Connected! Joining room {roomName}...";
        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions(), TypedLobby.Default);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        topText.text = $"Disconnected: {cause}";
        StopGame();
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
    }

    public void OnCreatedRoom()
    {
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinedRoom()
    {
        StartCoroutine(StartGame());
    }

    private void StopGame()
    {
        foreach (var go in onJoinActivate)
        {
            go.SetActive(false);
        }
    }

    private IEnumerator StartGame()
    {
        topText.text = $"Joined room: {roomName}";
        foreach (var go in onJoinActivate)
        {
            go.SetActive(true);
        }
        yield return new WaitForSeconds(3);
        
        topText.text = $"You're in! Enjoy :)";
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        topText.text = $"Failed to join room {roomName}: {message}";
        StopGame();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
    }

    public void OnLeftRoom()
    {
        topText.text = $"Left room: {roomName}";
        StopGame();
    }
}