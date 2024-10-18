using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.PunBasics;
using System.Linq;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public bool startGame;

    [Header("References")]
    public GameObject principalMaz;
    public GameObject[] cardsCentrals;
    public Transform positionCardsSelect;
    public GameObject cardSelect;
    public GameObject player1;
    public GameObject player2;
    public RandomCart randomCart;
    public GameObject centralCart;
    public Button cutButton;
    public GameObject instruction_Label;
    public Color turnColor;
    public GameObject redCard;
    public GameObject blueCard;
    public GameObject timerIA;
    public GameObject timer;
    public Transform positionCut1;
    public Transform positionCut2;
    public GameObject cuts;
    public Color colorCribbage;
    [Header("Rules")]
    [Range(0, 14f)]
    public int MaxcartPerPlayer;
    public bool turnPlayer1;
    public bool turnPlayer2;
    public float maxTime;
    [Header("Cribbage")]
    public List<GameObject> cribbageCards;

    [Header("CONTROLLERS")]

    public Controller controllerPlayer1;
    public Controller controllerPlayer2;
    public IaMain aiMain;
    public ShootCart[] randomShootPositions;
    [Header("FLECHS")]
    public Color colorPlayer1;
    public Color colorPlayer2;
    public SpriteRenderer[] spriteFlechasRow;
    public SpriteRenderer[] spriteFlechasCol;


    bool startCount;
    int random;
    float time;
    float timeIA;
    float count;
    bool playerTurn;
    bool IAturn;
    bool start;
    bool p1;
    bool p2;
    bool finish;
    bool player1Row;
    GameObject cardCut1;
    GameObject cardCut2;


    GameObject Cut1;
    GameObject Cut2;


    [Header("Game Results")]
    public GameObject gameWinPanel;
    public GameObject gameLosePanel;

    public GameObject countDownPanel;
    public TMP_Text countdown;

    public TMP_Text cribbageUserName;

    public GameObject startButton;

    public GameObject _aiCribagge1, _aiCribagge2;
    public GameObject _extraPointinfo, _oneextraPointinfo;


    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        //playerTurn = Random.value >= 0.5;
        player1Row = Random.value >= 0.5;

        randomCart = GetComponent<RandomCart>();


        if (player1Row)
        {
            controllerPlayer1.rowPlayer = true;
            controllerPlayer1.columPlayer = false;
            controllerPlayer2.rowPlayer = false;
            controllerPlayer2.columPlayer = true;
            for (int i = 0; i < spriteFlechasRow.Length; i++)
            {
                spriteFlechasRow[i].color = colorPlayer1;
            }
            for (int i = 0; i < spriteFlechasCol.Length; i++)
            {
                spriteFlechasCol[i].color = colorPlayer2;
            }

        }
        else
        {
            controllerPlayer1.rowPlayer = false;
            controllerPlayer1.columPlayer = true;
            controllerPlayer2.rowPlayer = true;
            controllerPlayer2.columPlayer = false;
            for (int i = 0; i < spriteFlechasRow.Length; i++)
            {
                spriteFlechasRow[i].color = colorPlayer2;
            }
            for (int i = 0; i < spriteFlechasCol.Length; i++)
            {
                spriteFlechasCol[i].color = colorPlayer1;
            }
        }

        controllerPlayer1 = player1.GetComponent<Controller>();
        controllerPlayer2 = player2.GetComponent<Controller>();

        List<ShootCart> randomShootPositionsTemp = GameObject.FindObjectsOfType<ShootCart>().ToList();
        List<ShootCart> randomShootPositionsTemp2 = new List<ShootCart>();
        for (int i = 0; i < randomShootPositionsTemp.Count; i++)
        {
            if (!randomShootPositionsTemp[i].cribbagge)
            {
                randomShootPositionsTemp2.Add(randomShootPositionsTemp[i]);
            }
        }

        ///TODO: Remove Cribbage Positions from Array
        randomShootPositions = randomShootPositionsTemp2.ToArray();
        if (PegsScoreManager.isNewGameStarted)
        {
            GeneralMaz.Instance.gameObject.SetActive(false);
        }
        else
        {
            startButton.SetActive(false);
            if (principalMaz != null)
            {
                principalMaz.SetActive(false);
            }
            //GeneralMaz.Instance.gameObject.SetActive(true);
            CutMode();
        }

    }

    public void CheckCenteralCardForJack()
    {
        if (GetComponent<Scoring>().row3[2].GetComponent<ShootCart>().idCard == 11)
        {
            Debug.Log("Centeral Card is Jack ");
            RewardDealerWith2Points();
        }
        else
        {
            Debug.Log("Centeral Card is Not Jack ");
        }
    }

    public void RewardDelaerWith1Point()
    {

        if (controllerPlayer1.cribbagePlayer)
        {
            Debug.Log("Rewards Player 1 with one point ");
            controllerPlayer1.SetScoreColum(1, false);
            controllerPlayer1.SetScoreRow(1, false);
            StartCoroutine(ShowExtraPointsInfo(_oneextraPointinfo));
        }
        else
        {
            Debug.Log("Rewards Player 2 with one point ");

            controllerPlayer2.SetScoreColum(1, false);
            controllerPlayer2.SetScoreRow(1, false);
            StartCoroutine(ShowExtraPointsInfo(_oneextraPointinfo));
        }
    }

    public void RewardDealerWith2Points()
    {
        if (controllerPlayer1.cribbagePlayer)
        {
            controllerPlayer1.SetScoreColum(2, false);
            controllerPlayer1.SetScoreRow(2, false);
            StartCoroutine(ShowExtraPointsInfo(_extraPointinfo));
        }
        else
        {
            controllerPlayer2.SetScoreColum(2, false);
            controllerPlayer2.SetScoreRow(2, false);
            StartCoroutine(ShowExtraPointsInfo(_extraPointinfo));
        }

    }


    IEnumerator ShowExtraPointsInfo(GameObject obj)
    {
        yield return null;
        obj.SetActive(true);
        yield return new WaitForSeconds(2);
        obj.SetActive(false);

    }
    public void Finish()
    {
        finish = true;
    }

    #region Game Result calculations
    public void CheckForGameResults()
    {
        if (PegsScoreManager.RedPegsScore >= 121 || PegsScoreManager.BluePegsScore >= 121)
        {
            GameFinishedShowTheResults();
        }
        else
        {
            Debug.Log("Not Finished ");
            Debug.Log("Red : " + PegsScoreManager.RedPegsScore + "  Blue: " + PegsScoreManager.BluePegsScore);
            PegsScoreManager.isNewGameStarted = false;
            StartCoroutine(CountdownTimer());
        }

    }

    public void GameFinishedShowTheResults()
    {
        Debug.Log("Red Score: " + PegsScoreManager.RedPegsScore + "  BlueScoreManager: " + PegsScoreManager.BluePegsScore);
        if (PegsScoreManager.RedPegsScore > PegsScoreManager.BluePegsScore)
        {
            gameWinPanel.SetActive(true);
        }
        else
        {
            gameLosePanel.SetActive(true);
        }
    }
    public GameObject crib1, crib2;
    public void RevealAICribagge()
    {
        Scoring scor = GetComponent<Scoring>();
        crib1 = Instantiate(aiMain.cardsCribbage[0], _aiCribagge1.transform.parent);
        crib2 = Instantiate(aiMain.cardsCribbage[1], _aiCribagge2.transform.parent);
        _aiCribagge1.SetActive(false);
        _aiCribagge2.SetActive(false);
        crib1.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;
        crib2.GetComponentInChildren<SpriteRenderer>().sortingOrder = 5;

    }

    public void RestartNewGame()
    {
        PegsScoreManager.ResetScores();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ResetScore()
    {
        PegsScoreManager.ResetScores();
    }

    public IEnumerator CountdownTimer()
    {
        yield return new WaitForSeconds(5f);
        countdown.text = "10";
        countDownPanel.SetActive(true);
        int timer = 10;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            countdown.text = timer.ToString();
        }
        countDownPanel.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    public void boolStart()
    {

        timer.transform.root.gameObject.SetActive(true);
        Invoke(nameof(startBar), .5f);

    }
    void startBar()
    {
        start = true;
    }
    public void CutMode()
    {
        StartCoroutine(CutModeEnum());
    }
    IEnumerator CutModeEnum()
    {
        List<GameObject> cardsSpawned = new List<GameObject>();
        if (PegsScoreManager.isNewGameStarted)
        {
            for (int i = 0; i <= 52; i++)
            {
                Vector3 vec = new Vector3(positionCardsSelect.position.x, positionCardsSelect.position.y - i * 0.13f, positionCardsSelect.position.z);
                yield return new WaitForSeconds(.03f);
                GameObject gObj = Instantiate(cardSelect, vec, cardSelect.transform.rotation);
                gObj.name = i + "Select";
                cardsSpawned.Add(gObj);
                if (principalMaz != null)
                {
                    Destroy(principalMaz.gameObject);
                }
            }

            foreach (GameObject ob in cardsSpawned)
            {
                ob.GetComponent<BoxCollider2D>().enabled = true;
            }

        }

        //NEW
        //TODO Distribute the Cards
        else
        {
            if (principalMaz != null)
            {
                Destroy(principalMaz.gameObject);
            }
            Debug.Log("11111111111");
            GameManager.Instance.DeleteCutSelectedCards();
            GeneralMaz.Instance.gameObject.SetActive(true);
            StartCards();
        }
    }
    public void DeleteCutSelectedCards()
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
            Destroy(GameObject.Find(i.ToString() + "Select"));

        }
    }

    private void Update()
    {
        if (!start)
        {
            return;
        }
        if (finish)
        {
            return;
        }

        if (time > 0)
        {
            time -= Time.deltaTime;

            RefeshTimer();

        }
        else if (time <= 0)
        {
            if (p1)
            {
                RandomShootPlayer();
                p1 = false;
            }

            timer.transform.localScale = Vector3.Lerp(timer.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);


        }
        if (timeIA > 0)
        {
            timeIA -= Time.deltaTime;


            RefeshTimerIA();
        }
        else if (timeIA <= 0)
        {

            timerIA.transform.localScale = Vector3.Lerp(timerIA.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 2f);
        }


    }
    public void DisableAllCards(bool isplayer)
    {
        if (isplayer)
        {
            GameObject[] blankCards = GameObject.FindGameObjectsWithTag("DestroyCribbagePlayer");

            foreach (GameObject a in blankCards)
            {
                a.SetActive(false);
            }
        }
        else
        {
            GameObject[] blankCards = GameObject.FindGameObjectsWithTag("DestroyCribbage");

            foreach (GameObject a in blankCards)
            {
                a.SetActive(false);
            }
        }

    }
    void RefeshTimer()
    {
        float timer_ratio = (float)time / (float)maxTime;

        timer.transform.localScale = Vector3.Lerp(timer.transform.localScale, new Vector3(1, timer_ratio, 1), Time.deltaTime * 2f);
    }
    void RefeshTimerIA()
    {
        float timer_ratio = (float)timeIA / (float)maxTime;

        timerIA.transform.localScale = Vector3.Lerp(timerIA.transform.localScale, new Vector3(1, timer_ratio, 1), Time.deltaTime * 2f);
    }


    public void PlayCribbageCardsIfRemainingForPlayer()
    {

    }

    public void TurnController(bool _player1, bool _player2)
    {
        if (player2.GetComponent<Controller>().myCarts.Count <= 0)
        {
            if (player1.GetComponent<Controller>().myCarts.Count > 0 && player1.GetComponent<Controller>().myCarts.Count <= 2 && GameManager.Instance.cribbageCards.Count < 4)
            {
                Debug.Log("Last Turn 2");
                StartCoroutine(PlayCribbageCardsForPlayerifRemaining());
                _player1 = false; _player2 = true;
            }
        }

        if (_player1)
        {
            startCount = false;


            turnPlayer1 = false;
            turnPlayer2 = true;
            aiMain.turn = true;
            aiMain.AiTurn();
            player1.GetComponent<Movement>().turn = false;

            timeIA = maxTime;
            time = 0f;
            p2 = true;
            p1 = false;

        }
        if (_player2)
        {
            startCount = false;
            turnPlayer1 = true;
            turnPlayer2 = false;
            aiMain.turn = false;
            player1.GetComponent<Movement>().turn = true;
            time = maxTime;
            timeIA = 0f;
            p2 = false;
            p1 = true;
        }
    }

    public void CutGame()
    {
        int random = Random.Range(0, randomCart.carts.Count - 1);
        int random2 = GenerateRandomNumberforCutCards(random);//Random.Range(0, randomCart.carts.Count - 1);
        Debug.Log("Generated " + random2);
        cardCut1 = randomCart.carts[random];
        cardCut2 = randomCart.carts[random2];
        randomCart.carts.RemoveAt(random);
        randomCart.carts.RemoveAt(random2);

        Cut1 = Instantiate(cardCut1, positionCut1);
        Cut2 = Instantiate(cardCut2, positionCut2);
        Cut1.transform.localScale = new Vector3(1, 1, 1);
        Cut2.transform.localScale = new Vector3(1, 1, 1);
        Cut1.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        Cut2.GetComponentInChildren<SpriteRenderer>().sortingOrder = 100;
        cuts.SetActive(true);

        StartCoroutine(SetColor());


    }
    public int GenerateRandomNumberforCutCards(int notThis)
    {

        int cardIndex = randomCart.carts[notThis].GetComponent<CartMain>().idCart;
        int random2 = Random.Range(0, randomCart.carts.Count - 1);

        if (cardIndex != randomCart.carts[random2].GetComponent<CartMain>().idCart)
        {
            return random2;
        }
        else
        {
            Debug.Log("Got Same ID Generated New");
            return (GenerateRandomNumberforCutCards(notThis));
        }
    }

    IEnumerator SetColor()
    {

        yield return new WaitForSeconds(1.5f);
        if (cardCut1.GetComponent<CartMain>().idCart < cardCut2.GetComponent<CartMain>().idCart)
        {

            Cut1.GetComponentInChildren<SpriteRenderer>().color = colorCribbage;

        }
        else
        {
            Cut2.GetComponentInChildren<SpriteRenderer>().color = colorCribbage;

        }
    }
    public void CutFinished()
    {
        Debug.Log("CUT FINISHED");
        if (PegsScoreManager.isNewGameStarted)
        {

            if (cardCut1.GetComponent<CartMain>().idCart < cardCut2.GetComponent<CartMain>().idCart)
            {
                controllerPlayer1.cribbagePlayer = true;
                controllerPlayer2.cribbagePlayer = false;

                playerTurn = true;
                PegsScoreManager.isPlayerADealer = true;
            }
            else
            {
                controllerPlayer2.cribbagePlayer = true;
                controllerPlayer1.cribbagePlayer = false;

                playerTurn = false;
                PegsScoreManager.isPlayerADealer = false;


            }

            controllerPlayer1.cutPlayer = playerTurn;
            controllerPlayer2.cutPlayer = !playerTurn;


            Cut1.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            Cut2.GetComponentInChildren<SpriteRenderer>().color = Color.white;
            cuts.SetActive(false);
            PegsScoreManager.isPlayerADealer = !PegsScoreManager.isPlayerADealer;
        }
        else
        {
            CutFinishedNextHandTurnDecide();
        }
    }

    public void CutFinishedNextHandTurnDecide()
    {
        if (PegsScoreManager.isPlayerADealer)
        {
            controllerPlayer1.cribbagePlayer = true;
            controllerPlayer2.cribbagePlayer = false;

            playerTurn = true;
        }
        else
        {
            controllerPlayer2.cribbagePlayer = true;
            controllerPlayer1.cribbagePlayer = false;

            playerTurn = false;

        }
        PegsScoreManager.isPlayerADealer = !PegsScoreManager.isPlayerADealer;
        controllerPlayer1.cutPlayer = playerTurn;
        controllerPlayer2.cutPlayer = !playerTurn;
        cuts.SetActive(false);
    }

    public void StartGame()
    {
        Debug.Log("Start");
        randomCart.Mixer(controllerPlayer1, controllerPlayer2, MaxcartPerPlayer);
        boolStart();
        Invoke(nameof(TurnAsing), 3.5f);


    }
    void TurnAsing()
    {
        if (!playerTurn)
        {
            cribbageUserName.text = "Samuel's Cribagge";
            TurnController(false, true);
        }
        else
        {
            cribbageUserName.text = "Your Cribagge";
            TurnController(true, false);

        }

        /*  if (controllerPlayer1.cutPlayer)
          {

          }
          if (controllerPlayer2.cutPlayer)
          {
              TurnController(false, true);
          }*/
    }
    public void StartCards()
    {
        Debug.Log("Start");
        instruction_Label.SetActive(false);
        if (PegsScoreManager.isNewGameStarted)
        {
            StartCoroutine(waitForCart());
            CutGame();
        }
        else
        {
            CutFinished();
            GeneralMaz.Instance.gameObject.SetActive(true);
            GeneralMaz.Instance.Repart();
        }


    }
    public void InstanceBlank()
    {
        controllerPlayer1.InstanceBlank(false);

    }

    public int GetRandomShootPosition()
    {
        List<int> unoccupiedPosition = new List<int>();
        for (int ind = 0; ind < randomShootPositions.Length; ind++)
        {
            if (!randomShootPositions[ind].occuped)
            {
                unoccupiedPosition.Add(ind);
            }
        }

        if (unoccupiedPosition.Count > 0)
        {
            return unoccupiedPosition[Random.Range(0, unoccupiedPosition.Count)];
        }
        else
        {
            Debug.Log("No Position ");
            return -1;
        }


    }

    IEnumerator PlayCribbageCardsForPlayerifRemaining()
    {

        yield return null;
        int playerCribbagePending = 0;
        foreach (var card in player1.GetComponent<Controller>().cribbageCarts)
        {
            if (!card.GetComponent<ShootCart>().occuped)
            {
                playerCribbagePending++;
            }
        }
        if (player1.GetComponent<Controller>().myCarts.Count == playerCribbagePending)
        {
            for (int i = 0; i < playerCribbagePending; i++)
            {
                foreach (var card in player1.GetComponent<Controller>().cribbageCarts)
                {
                    if (!card.GetComponent<ShootCart>().occuped)
                    {
                        card.GetComponent<ShootCart>().active = true;
                        card.GetComponent<ShootCart>().playerCollider = player1;
                        card.GetComponent<ShootCart>().AddCart();
                    }
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    void RandomShootPlayer()
    {
        int number = GetRandomShootPosition();
        if (number == -1)
        {


            StartCoroutine(PlayCribbageCardsForPlayerifRemaining());

            return;
        }
        else {
            Debug.Log("Shoot Player Card");
            if (!randomShootPositions[number].GetComponent<ShootCart>().occuped)
            {
                randomShootPositions[number].GetComponent<ShootCart>().active = true;
                randomShootPositions[number].GetComponent<ShootCart>().playerCollider = player1;
                randomShootPositions[number].GetComponent<ShootCart>().AddCart();
            }
        }
        /* if (randomShootPositions[number].occuped)
         {

             Debug.Log("No Position Found Check PlayerCribbage");
             RandomShootPlayer();

         }
         else
         {*/
        print("Carta al azar");
       // randomShootPositions[number].AddCartFinisTime(player1);
        // }


    }

    public void InstanceCardBlank()
    {
        Vector3 p1 = new Vector3(player1.transform.position.x, player1.transform.position.y, player1.transform.position.z + 7);
        Vector3 p2 = new Vector3(player2.transform.position.x, player2.transform.position.y, player2.transform.position.z + 7);

        Instantiate(redCard, p1, Quaternion.identity).tag = "DestroyCribbagePlayer";
        Instantiate(blueCard, p2, Quaternion.identity).tag = "DestroyCribbage";
    }

    IEnumerator waitForCart()
    {
        yield return new WaitForSeconds(1.5f);
        controllerPlayer1.InstanceBlank(true);
        cuts.GetComponent<Animator>().SetTrigger("cut");
        //
        //controllerPlayer2.InstanceCart();


    }

    public void DisableMiddle()
    {
        Destroy(centralCart);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
