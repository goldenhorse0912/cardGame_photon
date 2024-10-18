using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootNetCard : MonoBehaviourPunCallbacks
{
    public Vector3 rowOrColum;
    public Color overColor;
    Color startColor;
    string cardOver;
    CardLibrary cards;
    public int playerNumber;
    
   
    [HideInInspector] public int idCard;
    public int valueCard;
    [HideInInspector] public string pal;
    public string cardName;
    public bool cribbagge;
    

    Vector3 scaleCardInit;
    Vector3 scale = new Vector3(0.148f, 0.168f, 1);
    Vector3 scaleRpc = new Vector3(0.18f, 0.168f, 1);
    void Start()
    {
        startColor = this.GetComponentInChildren<SpriteRenderer>().color;
        cards = GameObject.FindObjectOfType<CardLibrary>();
    }



    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LocalPlayer"))
        {
            if (valueCard != 0) { return; }
            if (!GameNetManager.Instance.start) return;
            if (cribbagge && playerNumber != collision.transform.GetComponent<PlayerNet>().playerNumber) return;
            this.GetComponentInChildren<SpriteRenderer>().color = overColor;
            cardOver = collision.GetComponent<PlayerNet>().actualCard;
            collision.GetComponent<PlayerNet>().overPlayer = this.transform;
   
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("LocalPlayer"))
        {
            if (valueCard != 0) { return; }
            if (!GameNetManager.Instance.start) return;
            if (cribbagge && playerNumber != collision.transform.GetComponent<PlayerNet>().playerNumber) return;
            if (cribbagge)
            {
                collision.transform.localScale = this.transform.localScale;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LocalPlayer"))
        {
            if (valueCard != 0) 
            {
                collision.GetComponent<PlayerNet>().overPlayer = null;
                return; 
            }
            this.GetComponentInChildren<SpriteRenderer>().color = startColor;
            if (!cribbagge)
            {
                collision.GetComponent<PlayerNet>().overPlayer = null;
            }
            else
            {
                collision.GetComponent<PlayerNet>().overPlayer = null;
            }
            
            cardOver = "";
          
        }
    }
    public void InstaceCardRandom(string card)
    {
        if (cribbagge)
        {

            GameObject cb = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(card)], this.transform);
            cardName = card;
            cb.transform.localScale = scale;

            idCard = cb.GetComponentInChildren<CartMain>().idCart;
            print(cb.GetComponentInChildren<CartMain>().cartScore);
            valueCard = cb.GetComponentInChildren<CartMain>().cartScore;

            pal = cb.GetComponent<CartMain>().typeCart;
            GameNetManager.Instance.OneCardSound.Play();
            photonView.RPC(nameof(RPC_InstanceCard), RpcTarget.Others, card);
            return;
        }
        GameObject go = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(card)], this.transform.position, this.transform.rotation);
        cardName = card;
        go.transform.SetParent(this.transform);
        idCard = go.GetComponentInChildren<CartMain>().idCart;
        valueCard = go.GetComponentInChildren<CartMain>().cartScore;
        pal = go.GetComponent<CartMain>().typeCart;
        GameNetManager.Instance.OneCardSound.Play();
        photonView.RPC(nameof(RPC_InstanceCard), RpcTarget.Others, card);
    }



    public void InstaceCard()
    {
        if (cribbagge)
        {
           
            
            GameObject cb = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(cardOver)], this.transform);
            
                        
            cardName = cardOver;
            cb.transform.localScale = scale;
            
         
            idCard = cb.GetComponentInChildren<CartMain>().idCart;
            valueCard = cb.GetComponentInChildren<CartMain>().cartScore;
            
            pal = cb.GetComponent<CartMain>().typeCart;
            GameNetManager.Instance.OneCardSound.Play();
            photonView.RPC(nameof(RPC_InstanceCard), RpcTarget.Others, cardOver);
            return;
        }
        GameObject go = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(cardOver)], this.transform.position, this.transform.rotation);
        cardName = cardOver;
        go.transform.SetParent(this.transform);
        idCard = go.GetComponentInChildren<CartMain>().idCart;
        valueCard = go.GetComponentInChildren<CartMain>().cartScore;
        pal = go.GetComponent<CartMain>().typeCart;
        GameNetManager.Instance.OneCardSound.Play();
        photonView.RPC(nameof(RPC_InstanceCard), RpcTarget.Others, cardOver);
    }
    [PunRPC]
    void RPC_InstanceCard(string card)
    {
       

        if (cribbagge)
        {

            GameObject cb = Instantiate(GameNetManager.Instance.CardBlank, this.transform);
            GameObject temp = CardLibrary.Instance.Cards[CardLibrary.Instance.Search(card)];
            cardName = card;
            cb.transform.localScale = scaleRpc;
            idCard = cb.GetComponentInChildren<CartMain>().idCart;
            valueCard = temp.GetComponentInChildren<CartMain>().cartScore;
            
            pal = cb.GetComponent<CartMain>().typeCart;
            GameNetManager.Instance.OneCardSound.Play();
            return;
        }

        GameObject go = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(card)], this.transform.position, this.transform.rotation);
        cardName = card;
        idCard = go.GetComponentInChildren<CartMain>().idCart;
        valueCard = go.GetComponentInChildren<CartMain>().cartScore;
       
        pal = go.GetComponent<CartMain>().typeCart;
        GameNetManager.Instance.OneCardSound.Play();
        go.transform.SetParent(this.transform);
    }

    public void InstanceFinalCribagge()
    {
        GameObject cb = Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(cardName)], this.transform);
        
                    
        cardName = cardOver;
        cb.transform.localScale = scaleRpc;
        
        
        idCard = cb.GetComponentInChildren<CartMain>().idCart;
        valueCard = cb.GetComponentInChildren<CartMain>().cartScore;
        
        pal = cb.GetComponent<CartMain>().typeCart;
        GameNetManager.Instance.OneCardSound.Play();
        Destroy(this.gameObject.transform.GetChild(1).gameObject);
    } 

    public void ViewCards()
    {
        for(int i=0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i));

        }
        Instantiate(CardLibrary.Instance.Cards[CardLibrary.Instance.Search(cardName)], this.transform);

    }

    public void RestartCardSpot()
    {
        this.GetComponentInChildren<SpriteRenderer>().color = startColor;
        valueCard = 0;
        cardName = string.Empty;
        cardOver = string.Empty;
    }
    
}
