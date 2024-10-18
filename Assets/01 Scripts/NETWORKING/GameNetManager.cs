using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameNetManager : MonoBehaviourPunCallbacks
{

    public GameObject CardBlank;
    public Color cardWinColor;
    public Color red;
    public Color blue;
    public List<GameObject> cards;
    public GameObject TextInfo;
    public Transform positionCut1;
    public Transform positionCut2;
    public GameObject cuts;
    public GameObject buttonCut2;
    public float timeTurn = 20f;
    public static GameNetManager Instance;
    [SerializeField] TextMeshPro namePlayer1;
    [SerializeField] TextMeshPro namePlayer2;
    [SerializeField] TextMeshPro nameCribagge;
    [SerializeField] GameObject startButton;
    [SerializeField] public Transform positionPlayer1;
    [SerializeField] public Transform positionPlayer2;
    public GameObject[] cardsCentrals;
    public Transform positionCardsSelect;
    public GameObject cardSelect;
    public GameObject principalMaz;
    public GameObject rowsArrow;
    public GameObject columnsArrow;
    public int clickStart;
    public int cutClick;
    public string nameActual;
    public int idActual;
    public GameObject player1Go;
    //public GameObject player2;
    GameObject cardCut1;
    GameObject cardCut2;
    GameObject Cut1;
    GameObject Cut2;
    GameObject timer1;
    GameObject timer;
    GameObject timer2;
    int cutSelectMaster;
    int cutSelectMaster2;
    bool playerTurn1;
    bool playerTurn2;
    public Transform centralPos;
    string str = "Waiting for the other player to cut ...";
    string str2 = "Waiting for the other player to select orientation game ...";
    public float currentTimePlayer1;
    public float currentTimePlayer2;
    public bool start;
    public bool rowPlayer1;
    [HideInInspector] public bool finish;
    bool randomShoot1;
    bool randomShoot2;
    public bool firstShoot;
    public PlayerNet thisPlayer;
    public GameObject[] selectionCards;
    public GameObject cutCardText;
    public Text messageText;
    [SerializeField] AudioSource cardSound;
    public AudioSource OneCardSound;
    public GameObject winForDisconnected;
    public GameObject loseForDisconnected;
    public GameObject quitButton;
    public Animator animatorPlayer1;
    public Animator animatorPlayer2;
    public bool player1Winner;
    public bool player2Winner;
    public GameObject orientationPanel;

    public SpriteRenderer[] rowsSprites;
    public SpriteRenderer[] columnsSprites;

    private PlayerNet player1;
    private PlayerNet player2;

    private void Awake()
    {
        Instance = this;
        player1Go = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);

    }
    void Start()
    {
        timer = GameObject.Find("Timers");
        timer1 = GameObject.Find("Timer");
        timer2 = GameObject.Find("Timer2");
        startButton = GameObject.Find("Canvas/StartButton");
        firstShoot = true;
    }

    public void RestartGame()
    {
        //PlayerNet player1 = GameObject.Find("1").GetComponent<PlayerNet>();
        //PlayerNet player2 = GameObject.Find("2").GetComponent<PlayerNet>();
        
        //player1.MovePegs();
        //player2.MovePegs();
        //player1.FirstCardAfterRestart();
        //player2.FirstCardAfterRestart();
        cards.Clear();
        cards.AddRange(CardLibrary.Instance.Cards);
        ScoreManager.instance.RestartScores();
        CartMain[] cardsInField = FindObjectsOfType<CartMain>();
        foreach (CartMain t in cardsInField)
        {
            Destroy(t.gameObject);
        }
        player1.FirstCardAfterRestart();
        player2.FirstCardAfterRestart();
        ShootNetCard[] cardsPositions = FindObjectsOfType<ShootNetCard>();
        foreach (ShootNetCard s in cardsPositions)
        {
            s.RestartCardSpot();
        }
        firstShoot = true;
        // foreach (GameObject a in cardsCentrals)
        // {
        //     a.SetActive(true);
        // }
        if (player2Winner)
        {
            playerTurn1 = true;
            playerTurn2 = false;
            player1Winner = true;
            player2Winner = false;
            photonView.RPC(nameof(SetCribaggeName), RpcTarget.All, PhotonNetwork.PlayerList[0].NickName);
            animatorPlayer1.SetBool("turn", true);
            animatorPlayer2.SetBool("turn", false);
            player1.turn = playerTurn1;
            player2.turn = playerTurn2;
            randomShoot2 = false;
            randomShoot1 = false;
            player1.overPlayer = null;
            player2.overPlayer = null;
        }
        else
        {

            playerTurn1 = false;
            playerTurn2 = true;
            player1Winner = false;
            player2Winner = true;
            photonView.RPC(nameof(SetCribaggeName), RpcTarget.All, PhotonNetwork.PlayerList[1].NickName);
            animatorPlayer2.SetBool("turn", true);
            animatorPlayer1.SetBool("turn", false);
            player1.turn = playerTurn1;
            player2.turn = playerTurn2;
            randomShoot1 = false;
            randomShoot2 = false;
            player1.overPlayer = null;
            player2.overPlayer = null;
        }

        messageText.text = string.Empty;
        cutCardText.SetActive(false);

        Repart();

		//RestartArrows();
        //ActivateSelectPanel();

        CentralCard();
    }

    public void TurnOffAnimationTurn()
    {
        animatorPlayer1.SetBool("turn", false);
        animatorPlayer2.SetBool("turn", false);
    }

    public void SetBoolDirection(bool b)
    {
        photonView.RPC(nameof(RPC_SetBoolDirection), RpcTarget.All, b);
    }
    [PunRPC]
    void RPC_SetBoolDirection(bool rp1)
    {
        rowPlayer1 = rp1;
        ArrowsInit(rp1);
    }

    public void ClickOnSelectOrientation(int orientation)
    {
		print(thisPlayer.playerNumber);
		if (orientation == 0)
		{
			if (thisPlayer.PlayerNumber == 1)
			{ 
				SetBoolDirection(true);
				SetArrows(true, thisPlayer.PlayerNumber);
				photonView.RPC(nameof(SetArrows), PhotonNetwork.PlayerListOthers[0], true, 2); 
			}
			else 
			{
				SetBoolDirection(false);
				SetArrows(false, thisPlayer.PlayerNumber);
				photonView.RPC(nameof(SetArrows), PhotonNetwork.PlayerListOthers[0], false, 1); 
			}
		}
		else
		{
			if (thisPlayer.PlayerNumber == 1)
			{ 
				SetBoolDirection(false);
				SetArrows(false, thisPlayer.PlayerNumber);
				photonView.RPC(nameof(SetArrows), PhotonNetwork.PlayerListOthers[0], false, 2); 
			}
			else 
			{
				SetBoolDirection(true);
				SetArrows(true, thisPlayer.PlayerNumber);
				photonView.RPC(nameof(SetArrows), PhotonNetwork.PlayerListOthers[0], true, 1); 
			}
		}
    }

	public void RestartArrows()
	{

		foreach (SpriteRenderer a in rowsSprites)
		{
			a.gameObject.SetActive(true);
			a.color = Color.white;
		}

		foreach (SpriteRenderer b in columnsSprites)
		{
			b.gameObject.SetActive(true);
			b.color = Color.white;
		}
	}

    void ArrowsInit(bool player1row)
    {
        SpriteRenderer[] rows = rowsArrow.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] columns = columnsArrow.GetComponentsInChildren<SpriteRenderer>();

        if (player1row)
        {
            foreach (SpriteRenderer a in rows)
            {
                a.color = red;
            }
            foreach (SpriteRenderer b in columns)
            {
                b.color = blue;
            }


        }
        else
        {
            foreach (SpriteRenderer a in rows)
            {
                a.color = blue;
            }
            foreach (SpriteRenderer b in columns)
            {
                b.color = red;
            }

        }
    }

	[PunRPC]
    public void SetArrows(bool _rowPlayer1, int PlayerNumber)
    {
        SpriteRenderer[] rows = rowsArrow.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer[] columns = columnsArrow.GetComponentsInChildren<SpriteRenderer>();
        if (PlayerNumber == 1)
        {
            if (_rowPlayer1)
            {
                foreach (SpriteRenderer b in columns)
                {
                    b.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (SpriteRenderer a in rows)
                {
                    a.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (_rowPlayer1)
            {
                foreach (SpriteRenderer a in rows)
                {
                    a.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (SpriteRenderer b in columns)
                {
                    b.gameObject.SetActive(false);
                }
            }

        }
    }

    void Update()
    {
        if (!start) 
        {
            return;
        }

        if (finish)
        {
            timer1.SetActive(false);
            timer2.SetActive(false);
            return;
        }

        if (currentTimePlayer1 > 0 && thisPlayer.CheckTurn())
        {
            currentTimePlayer1 -= Time.deltaTime;
            timer1.transform.localScale = Vector3.Lerp(timer1.transform.localScale, new Vector3(1, currentTimePlayer1 / timeTurn, 0), Time.deltaTime);
            if (currentTimePlayer1 < 0.005f)
            {
                print("randomDrop");
                RandomShootPlayer();
            }
        }

        if (currentTimePlayer1 <= 0)
        {
            timer1.transform.localScale = Vector3.Lerp(timer1.transform.localScale, new Vector3(1, 1, 0), Time.deltaTime * 3f);
        }

        if (currentTimePlayer2 > 0 && thisPlayer.CheckTurn())
        {
            currentTimePlayer2 -= Time.deltaTime;
            timer2.transform.localScale = Vector3.Lerp(timer2.transform.localScale, new Vector3(1, currentTimePlayer2 / timeTurn, 0), Time.deltaTime);
            if (currentTimePlayer2 < 0.005f)
            {
                print("randomDrop");
                RandomShootPlayer();
            }
        }

        if (currentTimePlayer2 <= 0)
        {
            timer2.transform.localScale = Vector3.Lerp(timer2.transform.localScale, new Vector3(1, 1, 0), Time.deltaTime * 3f);
        }

    }

    public void ChangeScene(string scene)
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
        SceneManager.LoadSceneAsync(scene);
    }



    public void PlayerName(string name, int id)
    {
        photonView.RPC(nameof(RPC_PlayerName), RpcTarget.All, name, id);
    }

    [PunRPC]
    void RPC_PlayerName(string name, int id)
    {
        if (id == 1)
        {
            namePlayer1.text = name;
            player1Go.name = id.ToString();


        }
        if (id == 2)
        {
            namePlayer2.text = name;
            player1Go.name = id.ToString();

        }

        if (photonView.IsMine)
        {
            nameActual = name;
            idActual = id;
        }

        PlayerNet[] players = GameObject.FindObjectsOfType<PlayerNet>();

        foreach (PlayerNet item in players)
        {
            if (item.playerNumber == 1)
            {
                item.row = rowPlayer1;
            }
            if (item.playerNumber == 2)
            {
                item.row = !rowPlayer1;
            }
        }



    }

    public void ClickStart(GameObject go)
    {
        Debug.Log("GameObject go: " + go);  // Check if go is null
        Debug.Log("PhotonNetwork.IsConnected: " + PhotonNetwork.IsConnected);  // Check Photon state if applicable

        GameObject player1Obj = GameObject.Find("1");
        if (player1Obj == null)
        {
            Debug.LogError("GameObject '1' not found");
            return;
        }
        player1 = player1Obj.GetComponent<PlayerNet>();

        GameObject player2Obj = GameObject.Find("2");
        if (player2Obj == null)
        {
            Debug.LogError("GameObject '2' not found");
            return;
        }
        player2 = player2Obj.GetComponent<PlayerNet>();

        if (photonView == null)
        {
            Debug.LogError("photonView is null");
            return;
        }

        photonView.RPC(nameof(RPC_ClickStart), RpcTarget.All);

        go.SetActive(false);  // Assuming this GameObject needs to be disabled
        TextInfo.SetActive(true);  // Assuming TextInfo is correctly assigned
    }


    [PunRPC]
    public void RPC_ClickStart()
    {
        clickStart++;

        if (clickStart >= 2)
        {
            TextInfo.GetComponent<TextMeshProUGUI>().text = "";
            TextInfo.SetActive(false);
            CutMode();
        }
    }

    public void CutMode()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(CutModeEnum());
        }

    }
    IEnumerator CutModeEnum()
    {
        for (int i = 0; i <= 52; i++)
        {
            Vector3 vec = new Vector3(positionCardsSelect.position.x, positionCardsSelect.position.y - i * 0.13f, positionCardsSelect.position.z);
            yield return new WaitForSeconds(.03f);
            PhotonNetwork.Instantiate("cardSelect", vec, Quaternion.Euler(0, 0, -90)).name = i + "Select";


            if (principalMaz != null)
            {
                photonView.RPC(nameof(RPC_DestroyPMAZ), RpcTarget.All);
            }
        }
    }

    public void TurnOffCardSelect()
    {
        selectionCards = GameObject.FindGameObjectsWithTag("CardsSelect");

        for (int i = 0; i < selectionCards.Length; i++)
        {
            selectionCards[i].GetComponent<BoxCollider2D>().enabled = false;
        }
    }


    [PunRPC]
    void RPC_DestroyPMAZ()
    {
        Destroy(principalMaz.gameObject);
        cutCardText.SetActive(true);
    }
    public void DeleteCutSelectedCards(int _playerNumber)
    {
        if (cutClick == 0)
        {
            cutSelectMaster = RandomNumber(0, cards.Count - 1);
            cutSelectMaster2 = RandomNumber(0, cards.Count - 1);
        }
        if (cutClick == 1)
        {
            cutSelectMaster = RandomNumber(0, cards.Count - 1);
            cutSelectMaster2 = RandomNumber(0, cards.Count - 1);
        }
        photonView.RPC(nameof(RPC_DeleteCutSelectedCards), RpcTarget.All, _playerNumber, cutSelectMaster, cutSelectMaster2);
    }
    [PunRPC]
    void RPC_DeleteCutSelectedCards(int _playerNumber, int _cutSelectMaster, int _cutSelectMaster2)
    {
        cutClick++;
        if (cutClick == 1)
        {

            InitialCut(_playerNumber, _cutSelectMaster, _cutSelectMaster2);
        }

        if (cutClick >= 2)
        {

            photonView.RPC(nameof(RPC_Cut), RpcTarget.All);
            //CutGame();
            SecondaryCut(_playerNumber, _cutSelectMaster, _cutSelectMaster2);
        }
    }
    [PunRPC]
    void RPC_Cut()
    {
        StartCoroutine(CutModeEnumDelete());
    }

    IEnumerator CutModeEnumDelete()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("CardsSelect");

        foreach (GameObject a in cardsCentrals)
        {
            a.SetActive(true);
        }

        for (int i = 0; i < array.Length; i++)
        {

            yield return new WaitForSeconds(.03f);
            Destroy(array[i].gameObject);

        }
    }
    public void CutGame()
    {
        if (PhotonNetwork.IsMasterClient) {
            MasterCut();
        }
    }

    void InitialCut(int player, int _cutSelectMaster, int _cutSelectMaster2)
    {
        messageText.text = "Low card wins the deal and gets first cribbage.";
        cuts.SetActive(true);
        switch (player)
        {
            case 1:
                //_cutSelectMaster = RandomNumber(0, cards.Count - 1);
                cardCut1 = cards[_cutSelectMaster];
                cards.RemoveAt(_cutSelectMaster);
                Cut1 = Instantiate(cardCut1, positionCut1.position, positionCut1.rotation);
                Cut1.transform.parent = positionCut1;
                Cut1.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
                break;
            case 2:
                //_cutSelectMaster2 = RandomNumber(0, cards.Count - 1);
                cardCut2 = cards[_cutSelectMaster2];
                cards.RemoveAt(_cutSelectMaster2);
                Cut2 = Instantiate(cardCut2, positionCut2.position, positionCut2.rotation);
                Cut2.transform.parent = positionCut2;
                Cut2.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
                break;
        }
    }

    void SecondaryCut(int player, int _cutSelectMaster, int _cutSelectMaster2)
    {
        switch (player)
        {
            case 1:
                //_cutSelectMaster = RandomNumber(0, cards.Count - 2);
                if (_cutSelectMaster == _cutSelectMaster2) { _cutSelectMaster++; }
                cardCut1 = cards[_cutSelectMaster];
                cards.RemoveAt(_cutSelectMaster);
                Cut1 = Instantiate(cardCut1, positionCut1.position, positionCut1.rotation);
                Cut1.transform.parent = positionCut1;
                Cut1.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
                cuts.GetComponent<Animator>().SetTrigger("cut");
                StartCoroutine(SetColor());
                break;
            case 2:
                //_cutSelectMaster2 = RandomNumber(0, cards.Count - 2);
                if (_cutSelectMaster == _cutSelectMaster2) { _cutSelectMaster2++; }
                cardCut2 = cards[_cutSelectMaster2];
                cards.RemoveAt(_cutSelectMaster2);
                Cut2 = Instantiate(cardCut2, positionCut2.position, positionCut2.rotation);
                Cut2.transform.parent = positionCut2;
                Cut2.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
                cuts.GetComponent<Animator>().SetTrigger("cut");
                StartCoroutine(SetColor());
                break;
        }
    }

    void MasterCut() {

        cutSelectMaster = RandomNumber(0, cards.Count - 1);
        cutSelectMaster2 = RandomNumber(0, cards.Count - 1);

        if (cutSelectMaster == cutSelectMaster2)
        {
            MasterCut();
            return;
        }

        photonView.RPC(nameof(RPC_CutGame), RpcTarget.All, cutSelectMaster, cutSelectMaster2);

    }
    int RandomNumber(int init, int finish)
    {
        return Random.Range(init, finish);
    }
    [PunRPC]
    public void RPC_CutGame(int random, int random2)
    {

        messageText.text = "Low card wins the deal and gets first cribbage.";
        cardCut1 = cards[random];
        cardCut2 = cards[random2];
        cards.RemoveAt(random);
        cards.RemoveAt(random2);

        Cut1 = Instantiate(cardCut1, positionCut1.position, positionCut1.rotation);
        Cut2 = Instantiate(cardCut2, positionCut2.position, positionCut2.rotation);


        Cut1.transform.parent = positionCut1;
        Cut2.transform.parent = positionCut2;
        Cut1.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        Cut2.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        cuts.SetActive(true);


        StartCoroutine(SetColor());
    }
    IEnumerator SetColor()
    {
        yield return new WaitForSeconds(1.2f);
        if (Cut1.GetComponent<CartMain>().idCart < Cut2.GetComponent<CartMain>().idCart)
        {
            Cut1.GetComponentInChildren<SpriteRenderer>().color = cardWinColor;
        }
        else
        {
            Cut2.GetComponentInChildren<SpriteRenderer>().color = cardWinColor;
        }

    }
    public void CutFinished() {
        photonView.RPC(nameof(RPC_CutFinished), RpcTarget.All);
    }

    public void Repart() {
        if (PhotonNetwork.IsMasterClient) {

            photonView.RPC(nameof(CleanCards), RpcTarget.All);
            for (int i = 0; i < 14; i++)
            {
                int random = Random.Range(0, cards.Count - 1);
                int random2 = Random.Range(0, cards.Count - 1);

                if (random == random2)
                {
                    random = Random.Range(0, cards.Count - 1);
                    random2 = Random.Range(0, random);
                }

                MasterRepart(cards[random].name, cards[random2].name, random, random2);
            }
        }
    }
    void MasterRepart(string first, string sec, int r1, int r2) {
        photonView.RPC(nameof(RPC_Repart), RpcTarget.All, first, sec, r1, r2);
    }

    [PunRPC]
    void CleanCards()
    {
        player1.Cards.Clear();
        player2.Cards.Clear();
    }

    [PunRPC]
    void RPC_Repart(string f, string s, int _random, int _random2)
    {
        GameObject cardPlayer1 = CardLibrary.Instance.SearchGO(f);
        GameObject cardPlayer2 = CardLibrary.Instance.SearchGO(s);
        cards.Remove(cardPlayer1);
        cards.Remove(cardPlayer2);
        player1.AddCard(f);
        player2.AddCard(s);

    }

    [PunRPC]
    public void RPC_CutFinished()
    {


        if (cardCut1.GetComponent<CartMain>().idCart < cardCut2.GetComponent<CartMain>().idCart)
        {
            playerTurn1 = true;
            player1Winner = true;
            player2Winner = false;
            playerTurn2 = false;
            photonView.RPC(nameof(SetCribaggeName), RpcTarget.All, PhotonNetwork.PlayerList[0].NickName);
            animatorPlayer1.SetBool("turn", true);
        }
        else
        {

            playerTurn1 = false;
            playerTurn2 = true;
            player1Winner = false;
            player2Winner = true;
            photonView.RPC(nameof(SetCribaggeName), RpcTarget.All, PhotonNetwork.PlayerList[1].NickName);
            animatorPlayer2.SetBool("turn", true);
        }
        Cut1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        Cut2.GetComponentInChildren<SpriteRenderer>().color = Color.white;
        cuts.SetActive(false);
        messageText.text = string.Empty;
        cutCardText.SetActive(false);
        cardSound.PlayDelayed(0.7f);
    }

    [PunRPC] public void SetCribaggeName(string playerName)
    {
        nameCribagge.text = $"{playerName}'s Cribagge Cards";
    }

    public void UltimateCut(){

        photonView.RPC(nameof(RPC_UltimateCut), RpcTarget.All);
    }

    public void ActivateSelectPanel(){

        photonView.RPC(nameof(RPC_OrientationSelection), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_UltimateCut(){
        cardSound.Stop();
        orientationPanel.SetActive(false);
        buttonCut2.SetActive(false);
        TextInfo.GetComponent<TextMeshProUGUI>().text = str;
        TextInfo.SetActive(true);

        if(playerTurn1){

            player2.UltimateCut(2, buttonCut2);
        }
        else{

            player1.UltimateCut(1, buttonCut2);
        }
    }

    [PunRPC]
    public void RPC_OrientationSelection(){
        cardSound.Stop();
        TextInfo.GetComponent<TextMeshProUGUI>().text = str2;

        if(playerTurn1){

            player2.SelectOrientationPanel(2, orientationPanel);
        }
        else{

            player1.SelectOrientationPanel(1, orientationPanel);
        }
    }

    public void ClickUltimateCut(){
        photonView.RPC(nameof(RPC_ClickUltimateCut), RpcTarget.All);
    }

    [PunRPC]
    void RPC_ClickUltimateCut(){
        buttonCut2.SetActive(false);
        TextInfo.GetComponent<TextMeshProUGUI>().text = "";
        CentralCard();
    }

    public void CentralCard(){
        
        int number= Random.Range(0, cards.Count - 1);
        if(PhotonNetwork.IsMasterClient){
            photonView.RPC(nameof(RPC_CentralCard), RpcTarget.AllViaServer, number);
        }  
        
    }

    [PunRPC]
    void RPC_CentralCard(int number){

        GameObject ob  = Instantiate(cards[number], centralPos.position, centralPos.rotation);
        ob.transform.parent = centralPos;
        centralPos.GetComponent<ShootNetCard>().idCard = cards[number].GetComponent<CartMain>().idCart;
        centralPos.GetComponent<ShootNetCard>().valueCard = cards[number].GetComponent<CartMain>().cartScore;
        centralPos.GetComponent<ShootNetCard>().pal = cards[number].GetComponent<CartMain>().typeCart;
        StartCoroutine(wait());
                
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        StartGame();
    }
    public void StartGame(){

        timer.SetActive(true);
        timer1.SetActive(true);
        timer2.SetActive(true);
        player1.InstaciateCard();
        player2.InstaciateCard();
        player1.turn = playerTurn1;
        player2.turn = playerTurn2;
        StartCoroutine(waitForFirstTurn());
    }
    IEnumerator waitForFirstTurn()
    {
        yield return new WaitForSeconds(1.2f);
        if (playerTurn1)
        {
            currentTimePlayer1 = timeTurn;
            currentTimePlayer2 = 0;
            finish = false;
            start =true;    
        }
        else
        {
            currentTimePlayer2 = timeTurn;
            currentTimePlayer1 = 0;
            finish = false;
            start =true;
        }
    }


    public void ChangeTurn(bool isCribbage=false)
    {
        if (isCribbage)
        {
            photonView.RPC(nameof(RPC_NoChangeTurn), RpcTarget.All);
        }
        else
        {
            photonView.RPC(nameof(RPC_ChangeTurn), RpcTarget.All);
        }
    }
    [PunRPC]
    void RPC_ChangeTurn()
    {
        if (playerTurn1)
        {
            playerTurn1 = false;
            playerTurn2 = true;
            animatorPlayer1.SetBool("turn", false);
            animatorPlayer2.SetBool("turn", true);
            player1.turn = playerTurn1;
            player2.turn = playerTurn2;
  
            currentTimePlayer2 = timeTurn;
            currentTimePlayer1 = 0;
            randomShoot2 = false;

            player1.overPlayer = null;
            player2.overPlayer = null;


            return;
        }
        else if (playerTurn2)
        {
            playerTurn1 = true;
            playerTurn2 = false;
            animatorPlayer1.SetBool("turn", true);
            animatorPlayer2.SetBool("turn", false);
            player1.turn = playerTurn1;
            player2.turn = playerTurn2;
           
            currentTimePlayer1 = timeTurn;
            currentTimePlayer2 = 0;
            randomShoot1 = false;

            player1.overPlayer = null;
            player2.overPlayer = null;

            return;
        }
    }

    [PunRPC]
    void RPC_NoChangeTurn()
    {
        if (playerTurn1)
        {
            currentTimePlayer2 = 0;
            randomShoot1 = false;
            return;
        }
        else if (playerTurn2)
        {
            currentTimePlayer1 = 0;
            randomShoot2 = false;
            return;
        }
    }

    void RandomShootPlayer()
    {

        if (firstShoot)
        {
            if (playerTurn1 && !randomShoot1)
            {
                player1.RandomDrop();
                randomShoot1 = true;
            }
            if (playerTurn2 && !randomShoot2)
            {
                player2.RandomDrop();
                randomShoot2 = true;
            }
        }


    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (ScoreManager.instance.gameFinished) { return; }
        
        if (otherPlayer.IsLocal)
        {
            loseForDisconnected.SetActive(true);
            quitButton.SetActive(true);
            PhotonNetwork.LeaveLobby();
        }
        else
        {
            winForDisconnected.SetActive(true);
            quitButton.SetActive(true);
            PhotonNetwork.LeaveLobby();
        }
    }


}
