using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerNet : MonoBehaviourPunCallbacks
{
    public GameObject text_totalScore;
    public RoomData roomData;
    public Counter counter;
    public List<GameObject> Cards;
    public List<GameObject> allCards;
    public Sprite player1texture;
    public Sprite player2texture;
    public GameObject[] blankCards;
    private Vector3 startPos;
    private Vector3 startScale;
    public Transform overPlayer;
    string playerName;
    public GameObject winPanel;
    public GameObject loserPanel;
    public GameObject winForDisconnected;
    public GameObject loseForDisconnected;
    public int playerNumber;
    public bool row;
    public bool turn;
    public bool isMaster;
    bool LocalPlayer;
    Sprite mySprite;
    [HideInInspector]public string actualCard;
    GameObject actual;
    int totalScore;
    public Counter[] counters;
    public int cribbageCount;
    public int PlayerNumber { get => playerNumber; set => playerNumber = value; }

    private PhotonTransformView photonTransformView;
    
    void Start()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        counters = GameObject.FindObjectsOfType<Counter>(); 
        playerName = PhotonNetwork.NickName;
        photonTransformView = GetComponent<PhotonTransformView>();
        if (photonView.IsMine)
        {
            this.tag = "LocalPlayer";
            LocalPlayer = true;
            GameNetManager.Instance.thisPlayer = this;

            if (PhotonNetwork.IsMasterClient)
            { 
                PlayerNumber = 1;
                mySprite = player1texture;
            }
            else
            {
                PlayerNumber = 2;
                mySprite = player2texture;
            }
        }

        // if (PlayerNumber == 1) mySprite = player1texture;
        // else { mySprite = player2texture; }

        startScale = this.transform.localScale;

        PlayerName();

        // if (totalScore != 0) { Count.instance.MovePinInd(playerNumber, totalScore); }

        if (photonView.IsMine)
        {
            winPanel = GameObject.FindGameObjectWithTag("yourwin").gameObject;
            loserPanel = GameObject.FindGameObjectWithTag("yourlose").gameObject;
            winForDisconnected = GameObject.FindGameObjectWithTag("WinForDisconnected").gameObject;
            loseForDisconnected = GameObject.FindGameObjectWithTag("LoseForDisconnected").gameObject;

            winPanel.SetActive(false);
            loserPanel.SetActive(false);
            winForDisconnected.SetActive(false);
            loseForDisconnected.SetActive(false);
        }


    }

    public void MovePegs()
    {
        Debug.Log("Move Pegs");
        Count.instance.MovePinInd(playerNumber, totalScore);
    }

    [PunRPC]
    public void SetArrows(bool rowPLayer1, int _playerNumber)
    {
        GameNetManager.Instance.SetArrows(rowPLayer1, _playerNumber);
    }
    
    void RPC(string a){
        photonView.RPC(a, RpcTarget.All);
    }

    public void PlayerName()
    {
        photonView.RPC(nameof(RPC_PlayerName), RpcTarget.All, playerName, PlayerNumber);
    }
    public void FirstCard(){
        if(photonView.IsMine){
            if(this.name == "1")
            {
            PhotonNetwork.Instantiate(blankCards[0].name, this.transform.position, this.transform.rotation);
            }
            if(this.name == "2")
            {
            PhotonNetwork.Instantiate(blankCards[1].name, this.transform.position, this.transform.rotation);
            }
        }
    }

    public void FirstCardAfterRestart()
    {
        if(this.name == "1")
        {
            Instantiate(blankCards[0], this.transform.position, this.transform.rotation);
        }
        if(this.name == "2")
        {
            Instantiate(blankCards[1], this.transform.position, this.transform.rotation);
        }
        
    }
    
    [PunRPC]
    void RPC_FirstCard(){

        if(photonView.IsMine){
             if(this.name == "1"){
            PhotonNetwork.Instantiate(blankCards[0].name, this.transform.position, this.transform.rotation);
            }
            if(this.name == "2"){
            PhotonNetwork.Instantiate(blankCards[1].name, this.transform.position, this.transform.rotation);
            }
        }
       
    }
    [PunRPC]
    void RPC_PlayerName(string name, int id)
    {
        
        if(id == 1)
        {
            Counter[] counters = GameObject.FindObjectsOfType<Counter>();
            GameObject.Find("t_Player1Name").GetComponent<TextMeshPro>().text = name;
            this.transform.position = GameNetManager.Instance.positionPlayer1.position;
            startPos = GameNetManager.Instance.positionPlayer1.position;
            text_totalScore = GameObject.Find("Player1score");
            foreach(Counter a in counters)
            {
                if(a.playerColor == "red")
                {
                    counter = a;
                }
            }
            TotalScore(roomData.player1Score);
            this.name = id.ToString();
        }
       
        if(id ==2)
        {
            Counter[] counters = GameObject.FindObjectsOfType<Counter>();
            GameObject.Find("t_Player2Name").GetComponent<TextMeshPro>().text = name;
            this.transform.position = GameNetManager.Instance.positionPlayer2.position;
            startPos = GameNetManager.Instance.positionPlayer2.position;
            text_totalScore = GameObject.Find("Player2score");
            foreach (Counter a in counters)
            {
                if (a.playerColor == "blue")
                {
                    counter = a;
                }
            }
            TotalScore(roomData.player2Score);

            this.name =id.ToString();

        }

        


    }
    public void AddCard(string card){

        for (int i = 0; i < allCards.Count; i++)
        {
            if(card == allCards[i].name){
                Cards.Add(allCards[i]);
            }
        }
        
        
    }

    // MOVEMENT
    private void OnMouseDrag()
    {
        if (!photonView.IsMine) return;
        Drag();
    }
    private void OnMouseUp()
    {
        if(!photonView.IsMine) return;
        Drop();
    }



    public void Drag()
    {
        float timeStatus;
        if (playerNumber == 1) { timeStatus = GameNetManager.Instance.currentTimePlayer1; }
        else { timeStatus = GameNetManager.Instance.currentTimePlayer2; }

        if (!turn || timeStatus <= 0)
        {
            ReturnPositionInitial();
            return;
        }
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        this.transform.position = worldPosition;
    }

    public void Drop()
    {
        float timeStatus;
        if (playerNumber == 1) { timeStatus = GameNetManager.Instance.currentTimePlayer1; }
        else { timeStatus = GameNetManager.Instance.currentTimePlayer2; }
        
        if (!turn || timeStatus <= 0)
        {
            ReturnPositionInitial();
            return;
        }
        ReturnPositionInitial();
        if(overPlayer != null)
        {
            ShootNetCard[] shootNetCards = GameObject.FindObjectsOfType<ShootNetCard>();
            List<ShootNetCard> shootNetCardsCribagge = new List<ShootNetCard>();
            ShootNetCard temp = overPlayer.gameObject.GetComponent<ShootNetCard>();
            int freeSlots = 0;

            for (int i = 0; i < shootNetCards.Length; i++)
            {
                if (shootNetCards[i].valueCard == 0)
                {
                    if (playerNumber == 1 && shootNetCards[i].gameObject.name != "Cribbage") { continue; }
                    if (playerNumber == 2 && shootNetCards[i].gameObject.name != "CribbageAI") { continue; }
                    shootNetCardsCribagge.Add(shootNetCards[i]);
                }
            }

            for (int i = 0; i < shootNetCards.Length; i++)
            {
                if (shootNetCards[i].valueCard == 0)
                {
                    if (playerNumber == 1 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                    if (playerNumber == 2 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                    freeSlots++;
                }
            }

            if (shootNetCardsCribagge.Count != 0 && Cards.Count == 2)
            {
                GameObject.Find("CribbageTextWarn").GetComponent<TextMeshPro>().text = "You must play two cards into the Cribbage Card boxes!!";
            }

            if (!temp.cribbagge)
            {
                if (Cards.Count > 2)
                {
                    if (temp != null) { temp.InstaceCard(); }
                    ReturnPositionInitial();
                    GameNetManager.Instance.firstShoot = true;
                    InstaciateCard();
                    StartCoroutine(waitForTurnChaged());
                    return;
                }
                else if (Cards.Count == 2)
                {                    
                    if (shootNetCardsCribagge.Count == 2)
                    {
                        GameObject.Find("CribbageTextWarn").GetComponent<TextMeshPro>().text = "You must play two cards into the Cribbage Card boxes!!";
                        if (temp != null) { temp.InstaceCard(); }
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        InstaciateCard();
                        StartCoroutine(waitForTurnChaged(true));
                        return;
                    }
                    else
                    {
                        if (temp != null) { temp.InstaceCard(); }
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        InstaciateCard();
                        StartCoroutine(waitForTurnChaged());
                        return;
                    }
                }
                else if (Cards.Count == 1)
                {
                    if (shootNetCardsCribagge.Count == 2)
                    {
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        return;
                    }
                    else if (shootNetCardsCribagge.Count == 1)
                    {
                        
                        if (temp != null) { temp.InstaceCard(); }
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        InstaciateCard();
                        StartCoroutine(waitForTurnChaged(true));
                        return;
                    }
                    else if (shootNetCardsCribagge.Count == 0)
                    {
                        if (temp != null) { temp.InstaceCard(); }
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        InstaciateCard();
                        StartCoroutine(waitForTurnChaged());
                        return;
                    }
                }
                else if (Cards.Count == 0)
                {
                    if (shootNetCardsCribagge.Count == 1)
                    {
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        return;
                    }
                    else if (shootNetCardsCribagge.Count == 0)
                    {
                        if (temp != null) { temp.InstaceCard(); }
                        ReturnPositionInitial();
                        GameNetManager.Instance.firstShoot = true;
                        InstaciateCard();
                        StartCoroutine(waitForTurnChaged());
                        return;
                    }
                }
            }
            else
            {
                if (Cards.Count >= 1)
                {
                    if (temp != null) { temp.InstaceCard(); }
                    ReturnPositionInitial();
                    GameNetManager.Instance.firstShoot = true;
                    InstaciateCard();
                    StartCoroutine(waitForTurnChaged(true));
                    return;
                }
                else
                {
                    if (temp != null) { temp.InstaceCard(); }
                    ReturnPositionInitial();
                    GameNetManager.Instance.firstShoot = true;
                    InstaciateCard();
                    StartCoroutine(waitForTurnChaged());
                    return;
                }
            }
        }


    }

    public void RandomDrop(bool lastMove = false)
    {
        if (!photonView.IsMine) return;
        ReturnPositionInitial();
        ShootNetCard[] shootNetCards = GameObject.FindObjectsOfType<ShootNetCard>();
        List<ShootNetCard> shootNetCardsList = new List<ShootNetCard>();
        List<ShootNetCard> shootNetCardsCribagge = new List<ShootNetCard>();
        ShootNetCard shootNetPos;

        for (int i = 0; i < shootNetCards.Length; i++)
        {
            if (shootNetCards[i].valueCard == 0)
            {
                if (playerNumber == 1 && shootNetCards[i].gameObject.name != "Cribbage") { continue; }
                if (playerNumber == 2 && shootNetCards[i].gameObject.name != "CribbageAI") { continue; }
                shootNetCardsCribagge.Add(shootNetCards[i]);
            }
        }
        if (Cards.Count >= 3)
        {
            for (int i = 0; i < shootNetCards.Length; i++)
            {
                if (shootNetCards[i].valueCard == 0)
                {
                    if (playerNumber == 1 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                    if (playerNumber == 2 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                    shootNetCardsList.Add(shootNetCards[i]);
                }
            }
            int random = RandomNumber(0, shootNetCardsList.Count);
            shootNetPos = shootNetCardsList[random];
            shootNetCardsList.RemoveAt(random);
            shootNetPos.InstaceCardRandom(actualCard);
            StartCoroutine(waitForTurnChaged());
            ReturnPositionInitial();
            InstaciateCard();
        }
        else
        {
            if (shootNetCardsCribagge.Count == 0)
            {
                for (int i = 0; i < shootNetCards.Length; i++)
                {
                    if (shootNetCards[i].valueCard == 0)
                    {
                        if (playerNumber == 1 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                        if (playerNumber == 2 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                        shootNetCardsList.Add(shootNetCards[i]);
                    }
                }
                int random = RandomNumber(0, shootNetCardsList.Count);
                shootNetPos = shootNetCardsList[random];
                shootNetCardsList.RemoveAt(random);
                shootNetPos.InstaceCardRandom(actualCard);
                StartCoroutine(waitForTurnChaged());
                ReturnPositionInitial();
                InstaciateCard();
            }
            else
            {
                if (Cards.Count == 2)
                {
                    if (shootNetCardsCribagge.Count == 2)
                    {
                        int random = RandomNumber(0, shootNetCardsCribagge.Count);
                        shootNetPos = shootNetCardsCribagge[random];
                        shootNetCardsCribagge.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        ReturnPositionInitial();
                        InstaciateCard();
                        RandomDrop();
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < shootNetCards.Length; i++)
                        {
                            if (shootNetCards[i].valueCard == 0)
                            {
                                if (playerNumber == 1 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                                if (playerNumber == 2 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                                shootNetCardsList.Add(shootNetCards[i]);
                            }
                        }
                        int random = RandomNumber(0, shootNetCardsList.Count);
                        shootNetPos = shootNetCardsList[random];
                        shootNetCardsList.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        StartCoroutine(waitForTurnChaged());
                        ReturnPositionInitial();
                        InstaciateCard();
                        return;
                    }
                }

                if (Cards.Count == 1)
                {
                    if (shootNetCardsCribagge.Count == 2)
                    {
                        int random = RandomNumber(0, shootNetCardsCribagge.Count);
                        shootNetPos = shootNetCardsCribagge[random];
                        shootNetCardsCribagge.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        ReturnPositionInitial();
                        InstaciateCard();
                        RandomDrop();
                        return;
                    }
                    if (shootNetCardsCribagge.Count == 1)
                    {
                        int random = RandomNumber(0, shootNetCardsCribagge.Count);
                        shootNetPos = shootNetCardsCribagge[random];
                        shootNetCardsCribagge.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        ReturnPositionInitial();
                        InstaciateCard();
                        RandomDrop();
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < shootNetCards.Length; i++)
                        {
                            if (shootNetCards[i].valueCard == 0)
                            {
                                if (playerNumber == 1 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                                if (playerNumber == 2 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                                shootNetCardsList.Add(shootNetCards[i]);
                            }
                        }
                        int random = RandomNumber(0, shootNetCardsList.Count);
                        shootNetPos = shootNetCardsList[random];
                        shootNetCardsList.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        StartCoroutine(waitForTurnChaged());
                        ReturnPositionInitial();
                        InstaciateCard();
                        return;
                    }
                }

                if (Cards.Count == 0)
                {
                    if (shootNetCardsCribagge.Count == 1)
                    {
                        int random = RandomNumber(0, shootNetCardsCribagge.Count);
                        shootNetPos = shootNetCardsCribagge[random];
                        shootNetCardsCribagge.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        StartCoroutine(waitForTurnChaged());
                        ReturnPositionInitial();
                        InstaciateCard();
                    }
                    else
                    {
                        for (int i = 0; i < shootNetCards.Length; i++)
                        {
                            if (shootNetCards[i].valueCard == 0)
                            {
                                if (playerNumber == 1 && shootNetCards[i].gameObject.name == "CribbageAI") { continue; }
                                if (playerNumber == 2 && shootNetCards[i].gameObject.name == "Cribbage") { continue; }
                                shootNetCardsList.Add(shootNetCards[i]);
                            }
                        }
                        int random = RandomNumber(0, shootNetCardsList.Count);
                        shootNetPos = shootNetCardsList[random];
                        shootNetCardsList.RemoveAt(random);
                        shootNetPos.InstaceCardRandom(actualCard);
                        StartCoroutine(waitForTurnChaged());
                        ReturnPositionInitial();
                        InstaciateCard();
                    }
                }
            }
        }
    }

    private int RandomNumber(int init, int finish)
    {
        return Random.Range(init, finish);
    }
    

    public void ReturnPositionInitial()
    {
        this.transform.position = startPos;
        this.transform.localScale = startScale;
    }

    public void UltimateCut(int _id, GameObject button){

        if(_id == PlayerNumber){

            GameNetManager.Instance.TextInfo.GetComponent<TextMeshProUGUI>().text = "";

            button.SetActive(true);
            
        }
       
    }

    public void SelectOrientationPanel(int _id, GameObject panel)
    {
        if(_id == PlayerNumber){

            GameNetManager.Instance.TextInfo.GetComponent<TextMeshProUGUI>().text = "";
            panel.SetActive(true); 
        }
    }

    IEnumerator waitForTurnChaged(bool cribbage=false)
    {
        yield return new WaitForSeconds(0.5f);
        GameNetManager.Instance.ChangeTurn(cribbage);
       
    }
    public void InstaciateCard()
    {
        if(transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        if (Cards.Count <= 0)
        {
            GameObject.Find("CribbageTextWarn").GetComponent<TextMeshPro>().text = "";
            ScoreManager.instance.PlayerFinish();
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            if (playerNumber == 1)
            {
                Destroy(GameObject.Find("BlankRed 1(Clone)"));
                photonView.RPC(nameof(RPC_DestroyBlanks), RpcTarget.Others, "BlankRed 1(Clone)", photonView.ViewID);
            }
            else
            {
                Destroy(GameObject.Find("BlankBlue 1(Clone)"));
                photonView.RPC(nameof(RPC_DestroyBlanks), RpcTarget.Others, "BlankBlue 1(Clone)", photonView.ViewID);

            }
            return;
        }
        actual = Instantiate(Cards[0], this.transform.position, this.transform.rotation);
        if (!LocalPlayer)
        {
            actual.GetComponentInChildren<SpriteRenderer>().sprite = mySprite;
            actual.transform.localScale = new Vector3(0.18f, actual.transform.localScale.y, 0);
            actual.GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(.55f, .65f, 1);
        }
        actualCard = Cards[0].name;
        actual.tag = "LocalPlayer";
        actual.transform.parent = this.transform;
        actual.GetComponentInChildren<SpriteRenderer>().sortingOrder = 99;
        Cards.RemoveAt(0);
    }

    [PunRPC]
    void RPC_DestroyBlanks(string blank, int number)
    {
        GameObject player = PhotonView.Find(number).gameObject;

        Destroy(GameObject.Find(blank));

        for (int i = 0; i < player.transform.childCount; i++)
        {
            Destroy(player.transform.GetChild(i).gameObject);
        }
    }

    public void TotalScore(int _score)
    {
        totalScore += _score;
    }

    public void Win()
    {
        if(!photonView.IsMine) { return; }
        loserPanel.SetActive(false);
        winPanel.SetActive(true);
        GameNetManager.Instance.quitButton.SetActive(true);
    }

    public void Lose()
    {
        if(!photonView.IsMine) { return; }
        loserPanel.SetActive(true);
        winPanel.SetActive(false);
        GameNetManager.Instance.quitButton.SetActive(true);
    }



    public bool CheckTurn()
    {
        return turn;
    }
}
