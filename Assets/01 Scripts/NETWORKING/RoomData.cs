using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class RoomData : ScriptableObject
{
    public int player1Score = 0;
    public int player2Score = 0;


    public void RestartData()
    {
        player1Score = 0;
        player2Score = 0;
    }

}
