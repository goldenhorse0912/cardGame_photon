using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField createRoomName;
    [SerializeField] InputField joinRoomName;
    [SerializeField] GameObject errorGeneral;
    [SerializeField] GameObject errorRoom;
    [SerializeField] GameObject roomInterface;
    [SerializeField] GameObject createRoomInterface;
    [SerializeField] GameObject joinRoomInterface;
    [SerializeField] GameObject ManagerRoomInterface;
    [SerializeField] GameObject loadingPanel;
    [SerializeField] GameObject disconnectPanel;
    [SerializeField] GameObject room;
    [SerializeField] GameObject roomTitle;
    [SerializeField] Transform roomParent; 
    [SerializeField] TMP_Text textLog;
    [SerializeField] Text nickname;

    bool privateGame;
    public static NetworkManager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        LogText("CONNECTING TO ONLINE SERVICES...", "load");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void UpdateInformation()
    {
        string location = PhotonNetwork.CloudRegion.Substring(0, PhotonNetwork.CloudRegion.Length - 2).ToUpper();
        LogText($"SERVICES READY.\nSERVER REGION: {location}.\nOnline PLayers: {PhotonNetwork.CountOfPlayers}.", "ok");
        UpdateName();
    }

    public void ReconnectMaster()
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
        LogText("CONNECTING TO ONLINE SERVICES...", "load");
    }

    public override void OnJoinedLobby()
    {
        UpdateInformation();
        loadingPanel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomParent)
        {
            Destroy(child.gameObject);
        }

        Instantiate(roomTitle, roomParent);
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList) 
            {
                continue; 
            }
            GameObject temp = Instantiate(room, roomParent);
            temp.GetComponent<Room>().AssignValues(roomList[i].Name, roomList[i].PlayerCount);
        }
    }

    public void CreateRoom()
    {        
        if (string.IsNullOrEmpty(createRoomName.text))
        {
            Error(errorRoom, "Please insert a valid Room Name");
            return;
        }
        loadingPanel.SetActive(true);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.PublishUserId = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.CreateRoom(createRoomName.text, roomOptions, TypedLobby.Default);
    }

    #region New Room Methods
    // Method to join a random room
    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom(); // Attempts to join a random room
        loadingPanel.SetActive(true);
    }

    // If joining a random room fails, create a new room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join random room, creating a new room.");
        CreateNewRoom();
    }

    // Method to create a new room
    public void CreateNewRoom()
    {
        string roomName = "Room_" + UnityEngine.Random.Range(1000, 10000); // Generate a random room name
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Set max number of players in the room

        PhotonNetwork.CreateRoom(roomName, roomOptions); // Creates a new room with the room options
    }


    // Called when another player joins the room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player " + newPlayer.NickName + " has joined the room.");

        roomInterface.GetComponent<RoomInterface>().OnPlayerEnteredRoom(newPlayer);
        // Add custom code to handle the new player joining, such as updating the UI
    }

    // Called when a player leaves the room
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomInterface.GetComponent<RoomInterface>().OnPlayerLeftRoom(otherPlayer);

        Debug.Log("Player " + otherPlayer.NickName + " has left the room.");
        // Add custom code to handle the player leaving, such as updating the UI
    }
    #endregion


    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomName.text)) 
        {
            Error(errorRoom, "Please insert a valid Room Name");
            return;
        }
        loadingPanel.SetActive(true);
        PhotonNetwork.JoinRoom(joinRoomName.text);
    }
    public void JoinRoomPublic(string roomName)
    {
        loadingPanel.SetActive(true);
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined room: " + PhotonNetwork.CurrentRoom.Name);
        ManagerRoomInterface.SetActive(true);
        roomInterface.SetActive(true);
        createRoomInterface.SetActive(false);
        joinRoomInterface.SetActive(false);
        loadingPanel.SetActive(false);
    }

    public override void OnCreatedRoom()
    {
        roomInterface.SetActive(true);
        createRoomInterface.SetActive(false);
        joinRoomInterface.SetActive(false);
        loadingPanel.SetActive(false);
    }


    #region Handling Errors
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
        loadingPanel.SetActive(false);
        if (ManagerRoomInterface.activeSelf)
        {
            Error(errorRoom, message);
        }
        else
        {
            Error(errorGeneral, message);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        loadingPanel.SetActive(false);
        Error(errorRoom, message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        disconnectPanel.SetActive(true);
        print("disconnected");
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        Error(errorGeneral, errorInfo.Info);
    }

    #endregion

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        ManagerRoomInterface.SetActive(false);
        roomInterface.SetActive(false);
        createRoomInterface.SetActive(false);
        joinRoomInterface.SetActive(false);
    }

    public void Exit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
    public void Reload()
    {
        SceneManager.LoadSceneAsync("02");
    }

    public void DisconnectPhoton()
    {
        PhotonNetwork.Disconnect();
    }

    public void CallDisconnectLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    public void LogText(string text, string mode)
    {
        textLog.text = text;
        if(mode == "load")
        {
            textLog.color = new Color(1, 1, 1, 1);
        }
        if (mode == "ok")
        {
            textLog.color = Color.green;
        }
        if (mode == "warning")
        {
            textLog.color = Color.yellow;
        }
        if (mode == "error")
        {
            textLog.color = Color.red;
        }
    }

    void UpdateName()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("userName"))) 
        { 
            PhotonNetwork.NickName = "Player_" + UnityEngine.Random.Range(100, 9999).ToString();
            nickname.text = PhotonNetwork.NickName;
        }
        else 
        { 
            PhotonNetwork.NickName = PlayerPrefs.GetString("userName");
            nickname.text = PhotonNetwork.NickName;
        }
    }

    public void Error(GameObject text, string message)
    {
        StartCoroutine(ShowError(text, message));
    }

    IEnumerator ShowError(GameObject text, string message)
    {
        Text errorMessage = text.GetComponent<Text>();
        errorMessage.text = message;
        yield return new WaitForSeconds(2f);
        errorMessage.text = string.Empty;
    }
}
