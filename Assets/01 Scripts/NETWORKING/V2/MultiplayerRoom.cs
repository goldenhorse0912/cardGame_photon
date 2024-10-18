using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class MultiplayerRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] Text roomName;
    [SerializeField] Text playerList;
    [SerializeField] Button startGameButton;
    [SerializeField] GameObject optionsPanel;
    [SerializeField] GameObject otherPlayerTextPanel;
    [SerializeField] Dropdown wagerOptions;
    [SerializeField] GameObject error;
    public List<double> currentUsd = new List<double>();

    public override void OnEnable()
    {
        // PhotonNetwork.NickName = AuthenticationHandler.currentSession.name;
        // wagerOptions.options[0].text = AuthenticationHandler.adminOptions.wagerOptions.amount1.ToString();
        // wagerOptions.options[1].text = AuthenticationHandler.adminOptions.wagerOptions.amount2.ToString();
        // wagerOptions.options[2].text = AuthenticationHandler.adminOptions.wagerOptions.amount3.ToString();
        // wagerOptions.options[3].text = AuthenticationHandler.adminOptions.wagerOptions.amount4.ToString();
        // SettingsManagerPublic.instance.StartPanel();
        // SettingsManager.instance.StartPanel();
    }

    [PunRPC]
    public void SetRoomInformation(string _roomName = null)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            optionsPanel.SetActive(true);
            otherPlayerTextPanel.SetActive(false);
        }
        else
        {
            otherPlayerTextPanel.SetActive(true);
            optionsPanel.SetActive(false);

        }

        if (_roomName != null)
        {
            roomName.text = "Room: " + _roomName + "\n Players:";
        }

        playerList.text = "";

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            playerList.text += player.NickName + "\n";
        }
       
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 4) { startGameButton.interactable = true; }
        else { startGameButton.interactable = false; }
            
    }
    [PunRPC]
    public void SetRoomInformationPublicRoom(string _roomName = null)
    {
        print(roomName.text);
        otherPlayerTextPanel.SetActive(true);
        optionsPanel.SetActive(false);

        if (_roomName != null)
        {
            roomName.text = "Room: " + _roomName + "\n Players:";
        }

        playerList.text = "";

        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
        {
            playerList.text += player.NickName + "\n";
        }

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.PlayerList.Length == 4) { startGameButton.interactable = true; }
        else { startGameButton.interactable = false; }

    }
    public void CallPhotonUi(string roomName)
    {
        photonView.RPC("SetRoomInformation", RpcTarget.All, roomName);
    }
    public void CallPhotonUiPublicRoom(string roomName)
    {
        photonView.RPC("SetRoomInformationPublicRoom", RpcTarget.All, roomName);
        //if (PhotonNetwork.IsMasterClient) { SettingsManagerPublic.instance.LoadGameValues(); }
    }
    public void OnStartGame(int index)
    {
        foreach(double usd in currentUsd)
        {
            // if (usd < int.Parse(SettingsManager.instance.ReturnWager()))
            // {
            //     IntroManager.instance.Error(error, "One or more players dont have enogh balance to continue");
            //     photonView.RPC("ShowErrorOtherPlayers", RpcTarget.OthersBuffered);
            //     return;
            // }
        }
        
        NetworkManager.instance.photonView.RPC("LoadMultiplayerScene", RpcTarget.All, index);
    }

    // [PunRPC]
    // public void ShowErrorOtherPlayers()
    // {
    //     IntroManager.instance.Error(error, "One or more players dont have enogh balance to continue");
    // }

}
