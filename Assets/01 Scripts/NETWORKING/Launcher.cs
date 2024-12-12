using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI text_Log;
    public Button[] buttonInterafce;
    public RoomData roomData;
    public TMP_InputField inputName;
    int confirmed;

    bool inroom;
    bool ready;
    private void Awake()
    {
        if (PhotonNetwork.IsConnected) return;
        PhotonNetwork.AutomaticallySyncScene = true;
        Connect();
    }
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
            LogText("Connected", "ok");
        }
        else
        {
            LogText("CONNECTING TO ONLINE SERVICES...", "load");
        }

        roomData.RestartData();
            
    }
    public void Connect()
    {
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = "Player" + Random.Range(100, 999).ToString();

    }
    public void Exit()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync(0);
    }
    public void Disconnect()
    {

        PhotonNetwork.Disconnect();


    }

    public void FindRoom()
    {
        JoinRoom();
        LogText("WAITING FOR RIVAL...", "load");
    }
    void UpdateName()
    {
        //if(inputName.text.Trim() == ""){
        //    PhotonNetwork.NickName = "Player_" + Random.Range(100, 9999).ToString();
        //}
        //else{
        //    PhotonNetwork.NickName = inputName.text;
        //}
        if (PlayerPrefs.GetString("userName") == string.Empty) { PhotonNetwork.NickName = "Player_" + Random.Range(100, 9999).ToString(); }
        else { PhotonNetwork.NickName = PlayerPrefs.GetString("userName"); }
        //PhotonNetwork.NickName = inputName.text;
    }
    public void CreateRoom()
    {

        UpdateName();
        PhotonNetwork.CreateRoom("");
        LogText("Creating Room...", "ok");

    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LogText("Failed to create room...", "error");
    }
    
    public void JoinRoom()
    {
        UpdateName();

        print("En room.");
        PhotonNetwork.JoinRandomRoom();
       


    }
    private void Update()
    {
        if(PhotonNetwork.PlayerList.Length == 2 && inroom)
        {
            if (!ready)
            {
                LogText("READY TO START!", "ok");
            }
            PhotonNetwork.CurrentRoom.IsOpen = false;
            buttonInterafce[2].gameObject.SetActive(true);
        }
        else if(inroom && PhotonNetwork.PlayerList.Length < 2)
        {
            LogText("WAITING FOR RIVAL...", "load");

        }
    }

    public override void OnConnectedToMaster()
    {
       
        PhotonNetwork.JoinLobby();

        LogText("Connected", "ok");
        

        foreach (Button a in buttonInterafce)
        {
            a.interactable = true;
        }

        base.OnConnectedToMaster();
    }

    public override void OnJoinedLobby()
    {
        foreach (Button a in buttonInterafce)
        {
            a.interactable = true;
        }
        
    }


    public void LogText(string text, string mode)
    {
        if(mode == "load")
        {
            text_Log.text = text;
            text_Log.color = new Color(1, 1, 1, 1);

        }
        if (mode == "ok")
        {
            text_Log.text = text;
            text_Log.color = new Color(0, 1, .2f, .5f);
        }
        if (mode == "warning")
        {
            text_Log.text = text;
            text_Log.color = new Color(1, .8f, 0, 1);

        }
        if (mode == "error")
        {
            text_Log.text = text;
            text_Log.color = Color.red;

        }

    }
    public override void OnJoinedRoom()
    {
        LogText("Room Created", "ok");
        inroom = true;
        base.OnJoinedRoom();
    }
    public void StartRoom()
    {
        StartGame();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();

    }

    public void StartGame()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            PhotonNetwork.LoadLevel("03");
        }
    }
    public void Confirmated()
    {
        photonView.RPC(nameof(RPC_Confirmed), RpcTarget.All);
        buttonInterafce[3].interactable = false;
    }
    [PunRPC]
    public void RPC_Confirmed()
    {
        confirmed++;
        ready = true;
        LogText("Match Found", "ok");

        if(confirmed >= 2)
        {
            StartGame();
        }
    }

}
