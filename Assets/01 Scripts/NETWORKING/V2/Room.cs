using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    private string roomName;
    [SerializeField] Text roomNameText;
    [SerializeField] Text players;
    [SerializeField] Button joinRoomButton;

    public void AssignValues(string name, int playersInRoom)
    {
        roomName = name;
        roomNameText.text = roomName;
        players.text = $"{playersInRoom}/2";
        Debug.Log("playsers In Room:  " + playersInRoom);
        if (playersInRoom == 2)
        {
            joinRoomButton.interactable = false;
        }
    }

    public void JoinRoom()
    {
        NetworkManager.instance.JoinRoomPublic(roomName);
    }
}
