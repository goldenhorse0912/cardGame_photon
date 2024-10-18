using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SelectCard : MonoBehaviourPunCallbacks
{
    public Color SelectColor;
    Color startColor;
    public bool network;
    
    void Start()
    {
        startColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    private void OnMouseOver()
    {
        GetComponentInChildren<SpriteRenderer>().color = SelectColor;
    }
    private void OnMouseExit()
    {
        GetComponentInChildren<SpriteRenderer>().color = startColor;
    }
    private void OnMouseDown()
    {

        if (network)
        {
            GameNetManager.Instance.DeleteCutSelectedCards(GameNetManager.Instance.thisPlayer.playerNumber);
            GameNetManager.Instance.TurnOffCardSelect();
            photonView.RPC(nameof(RPC_Click), RpcTarget.All);
        }
        else
        {
            GameManager.Instance.DeleteCutSelectedCards();
            GameManager.Instance.StartCards();
            Destroy(gameObject);
        }
    
       
        
    }

    [PunRPC]
    void RPC_Click()
    {
        
        Destroy(gameObject);
      
    }
}
