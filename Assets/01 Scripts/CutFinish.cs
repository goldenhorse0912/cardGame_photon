using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CutFinish : MonoBehaviourPunCallbacks
{
    public bool network;
    public static CutFinish instance;

    private void Awake() 
    {
        instance = this;
    }   

    void Start(){
        if(network){
            this.gameObject.AddComponent<PhotonView>();
        }
    }
   public void CutFinished()
    {
        if(network){
            NetworkCutFinished();
            return;
        }
        GameManager.Instance.CutFinished();
        GeneralMaz.Instance.Repart();
    }
    public void NetworkCutFinished(){
         GameNetManager.Instance.CutFinished();
        photonView.RPC(nameof(RPC_NetworkCutFinished), RpcTarget.All);

    }

    [PunRPC]
    void RPC_NetworkCutFinished(){
        GeneralMaz.Instance.Repart();
    }

    public void Repart()
    {
        print("Trying to call RPC");
        photonView.RPC(nameof(RPC_NetworkCutFinished), RpcTarget.All);
    }

}
