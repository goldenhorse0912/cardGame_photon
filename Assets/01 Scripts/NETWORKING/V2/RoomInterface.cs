using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomInterface : MonoBehaviourPunCallbacks
{
    [SerializeField] Text roomName;
    [SerializeField] Transform nameHolder;
    [SerializeField] GameObject nickName;
    [SerializeField] Button startGame;
    public override void OnEnable() {
        
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        LoadPlayers();
    }

    public void LoadPlayers()
    {
        foreach (Transform child in nameHolder)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject temp = Instantiate(nickName, nameHolder);
            temp.GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            startGame.interactable = true;
        }
        else
        {
            startGame.interactable = false;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        LoadPlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        LoadPlayers();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("02");
    }

}
