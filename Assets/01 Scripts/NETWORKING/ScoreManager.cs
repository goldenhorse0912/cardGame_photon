using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    public static ScoreManager instance;
    public RoomData roomData;
    public TextMeshProUGUI scoreRed;
    public TextMeshProUGUI scoreBlue;
    public GameObject scoresScreen;
    public Animator changeScene;

    public List<int> scoresl = new List<int>();

    public List<ShootNetCard> positionsShoot;
    public List<ShootNetCard> Rows0;
    public List<ShootNetCard> Rows1;
    public List<ShootNetCard> Rows2;
    public List<ShootNetCard> Rows3;
    public List<ShootNetCard> Rows4;

    public List<ShootNetCard> Colum0;
    public List<ShootNetCard> Colum1;
    public List<ShootNetCard> Colum2;
    public List<ShootNetCard> Colum3;
    public List<ShootNetCard> Colum4;

    public List<ShootNetCard> CribbaggeCards;

    public List<TextMeshPro> RowsTexts;
    public List<TextMeshPro> ColumsTexts;
    public TMP_Text cribaggeTextScore;

    public bool isMaster;
    ShootNetCard[] shootPositions;

    int rowsScore;
    int columsScore;
    int cribaggeScore;
    int maxScore = 121;
    public int position;
    public int readyContinue;

    public List<ShootNetCard> testList = new List<ShootNetCard>();
    [HideInInspector] public int playerFinish;

    [SerializeField] GameObject waitingText;

    [SerializeField] GameObject newRoundButtonGo;
    [SerializeField] Button newRoundButton;

    PlayerNet player1;
    PlayerNet player2;

    public bool gameFinished;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (roomData.player1Score > 0 || roomData.player2Score > 0)
        {
            changeScene.SetBool("sceneChange", true);
        }

        shootPositions = GameObject.FindObjectsOfType<ShootNetCard>();
        readyContinue = 0;
        gameFinished = false;
    }

    [PunRPC]
    void RPC_CalculateScore()
    {

        CalculateScore();

    }
    void CalculateScore()
    {
        RevealCribaggeCards();
        RowsTexts[0].text = scores(Rows0).ToString();
        RowsTexts[1].text = scores(Rows1).ToString();
        RowsTexts[2].text = scores(Rows2).ToString();
        RowsTexts[3].text = scores(Rows3).ToString();
        RowsTexts[4].text = scores(Rows4).ToString();

        ColumsTexts[0].text = scores(Colum0).ToString();
        ColumsTexts[1].text = scores(Colum1).ToString();
        ColumsTexts[2].text = scores(Colum2).ToString();
        ColumsTexts[3].text = scores(Colum3).ToString();
        ColumsTexts[4].text = scores(Colum4).ToString();

        cribaggeTextScore.text = scores(CribbaggeCards).ToString();

        TotalScore();
    }

    public void RestartScores()
    {
        RowsTexts[0].text = string.Empty;
        RowsTexts[1].text = string.Empty;
        RowsTexts[2].text = string.Empty;
        RowsTexts[3].text = string.Empty;
        RowsTexts[4].text = string.Empty;

        ColumsTexts[0].text = string.Empty;
        ColumsTexts[1].text = string.Empty;
        ColumsTexts[2].text = string.Empty;
        ColumsTexts[3].text = string.Empty;
        ColumsTexts[4].text = string.Empty;

        cribaggeTextScore.text = string.Empty;

        playerFinish = 0;
        readyContinue = 0;
    }

    void TotalScore()
    {
        player1 = GameObject.Find("1").GetComponent<PlayerNet>();
        player2 = GameObject.Find("2").GetComponent<PlayerNet>();
        int row = int.Parse(RowsTexts[0].text) + int.Parse(RowsTexts[1].text) + int.Parse(RowsTexts[2].text) + int.Parse(RowsTexts[3].text) + int.Parse(RowsTexts[4].text);
        int col = int.Parse(ColumsTexts[0].text) + int.Parse(ColumsTexts[1].text) + int.Parse(ColumsTexts[2].text) + int.Parse(ColumsTexts[3].text) + int.Parse(ColumsTexts[4].text);
        cribaggeScore = int.Parse(cribaggeTextScore.text);

        if (GameNetManager.Instance.rowPlayer1)
        {
            if (GameNetManager.Instance.player1Winner)
            {
                row += cribaggeScore;
                player1.TotalScore(row);
                player2.TotalScore(col);
            }
            else
            {
                col += cribaggeScore;
                player1.TotalScore(row);
                player2.TotalScore(col);
            }

            roomData.player1Score += row;
            roomData.player2Score += col;
        }
        else
        {
            if (GameNetManager.Instance.player1Winner)
            {
                col += cribaggeScore;
                player2.TotalScore(row);
                player1.TotalScore(col);
            }
            else
            {
                row += cribaggeScore;
                player2.TotalScore(row);
                player1.TotalScore(col);
            }

            roomData.player2Score += row;
            roomData.player1Score += col;
        }

        if (roomData.player1Score < maxScore && roomData.player2Score < maxScore)
        {
            scoreRed.text = roomData.player1Score.ToString();
            scoreBlue.text = roomData.player2Score.ToString();
            GameNetManager.Instance.start = false;
            StartCoroutine(waitRestart());
        }
        else
        {
            scoreRed.text = roomData.player1Score.ToString();
            scoreBlue.text = roomData.player2Score.ToString();
            if (roomData.player1Score >= roomData.player2Score)
            {
                player1.Win();
                player1.MovePegs();
                player2.Lose();
                player2.MovePegs();
            }
            else
            {
                player1.Lose();
                player1.MovePegs();
                player2.Win();
                player2.MovePegs();
            }
            gameFinished = true;
            scoresScreen.SetActive(true);
            GameNetManager.Instance.quitButton.SetActive(true);
            GameNetManager.Instance.TurnOffAnimationTurn();
        }


    }

    private void RevealCribaggeCards()
    {
        ShootNetCard[] shootNetCards = GameObject.FindObjectsOfType<ShootNetCard>();
        List<ShootNetCard> shootNetCardsList = new List<ShootNetCard>();
        List<ShootNetCard> shootNetCardsCribagge = new List<ShootNetCard>();

        for (int i = 0; i < shootNetCards.Length; i++)
        {
            if (GameNetManager.Instance.thisPlayer.playerNumber == 2 && shootNetCards[i].gameObject.name != "Cribbage") { continue; }
            if (GameNetManager.Instance.thisPlayer.playerNumber == 1 && shootNetCards[i].gameObject.name != "CribbageAI") { continue; }
            shootNetCardsCribagge.Add(shootNetCards[i]);
        }

        shootNetCardsCribagge[0].InstanceFinalCribagge();
        shootNetCardsCribagge[1].InstanceFinalCribagge();
    }

    IEnumerator waitRestart()
    {
        yield return new WaitForSeconds(1);
        player1.MovePegs();
        player2.MovePegs();
        scoresScreen.SetActive(true);
        GameNetManager.Instance.TurnOffAnimationTurn();
        waitingText.SetActive(true);
        newRoundButtonGo.SetActive(true);
        newRoundButton.interactable = true;
    }

    public void OnclickNewRound()
    {
        photonView.RPC(nameof(StartNewRound), RpcTarget.AllViaServer);
    }


    [PunRPC]
    public void StartNewRound()
    {
        readyContinue++;
        if (readyContinue == 2)
        {
            waitingText.SetActive(false);
            newRoundButtonGo.SetActive(false);
            StartCoroutine(HoldStartNewRound());
        }
    }

    IEnumerator HoldStartNewRound()
    {
        yield return new WaitForSeconds(2);
        scoresScreen.SetActive(false);
        GameNetManager.Instance.RestartGame();
    }

    int scores(List<ShootNetCard> ListRow)
    {
        return ScorePar(ListRow) + Fifteen(ListRow) + Stair(ListRow) + Color(ListRow);
    }

    int Color(List<ShootNetCard> ListCard)
    {
        int score = 0;
        int match = 0;

        for (int i = 1; i < ListCard.Count; i++)
        {
            if (ListCard[0].pal == ListCard[i].pal)
            {
                match++;
            }
        }

        if (match == 4)
        {
            score = 5;
        }

        return score;
    }


    int ScorePar(List<ShootNetCard> ListCardUser)
    {
        int score = 0;
        List<ShootNetCard> _as = new List<ShootNetCard>();
        List<ShootNetCard> two = new List<ShootNetCard>();
        List<ShootNetCard> three = new List<ShootNetCard>();
        List<ShootNetCard> four = new List<ShootNetCard>();
        List<ShootNetCard> five = new List<ShootNetCard>();
        List<ShootNetCard> six = new List<ShootNetCard>();
        List<ShootNetCard> seven = new List<ShootNetCard>();
        List<ShootNetCard> eight = new List<ShootNetCard>();
        List<ShootNetCard> nine = new List<ShootNetCard>();
        List<ShootNetCard> ten = new List<ShootNetCard>();
        List<ShootNetCard> j = new List<ShootNetCard>();
        List<ShootNetCard> q = new List<ShootNetCard>();
        List<ShootNetCard> k = new List<ShootNetCard>();

        List<ShootNetCard> ListCard = new List<ShootNetCard>();
        ListCard.AddRange(ListCardUser);


        for (int i = 0; i < ListCard.Count; i++)
        {
            switch (ListCard[i].idCard)
            {
                case 1:
                    _as.Add(ListCard[i]);
                    break;
                case 2:
                    two.Add(ListCard[i]);
                    break;
                case 3:
                    three.Add(ListCard[i]);
                    break;
                case 4:
                    four.Add(ListCard[i]);
                    break;
                case 5:
                    five.Add(ListCard[i]);
                    break;
                case 6:
                    six.Add(ListCard[i]);
                    break;
                case 7:
                    seven.Add(ListCard[i]);
                    break;
                case 8:
                    eight.Add(ListCard[i]);
                    break;
                case 9:
                    nine.Add(ListCard[i]);
                    break;
                case 10:
                    ten.Add(ListCard[i]);
                    break;
                case 11:
                    j.Add(ListCard[i]);
                    break;
                case 12:
                    q.Add(ListCard[i]);
                    break;
                case 13:
                    k.Add(ListCard[i]);
                    break;
            }
        }

        score += CheckCardLists(_as);
        score += CheckCardLists(two);
        score += CheckCardLists(three);
        score += CheckCardLists(four);
        score += CheckCardLists(five);
        score += CheckCardLists(six);
        score += CheckCardLists(seven);
        score += CheckCardLists(eight);
        score += CheckCardLists(nine);
        score += CheckCardLists(ten);
        score += CheckCardLists(j);
        score += CheckCardLists(q);
        score += CheckCardLists(k);

        return score;
    }

    private int CheckCardLists(List<ShootNetCard> cardList)
    {
        switch (cardList.Count)
        {
            case 2:
                return 2;
            case 3:
                return 6;
            case 4:
                return 12;
            default:
                return 0;
        }
    }


    int Stair(List<ShootNetCard> ListCard)
    {
        List<ShootNetCard> _as = new List<ShootNetCard>();
        List<ShootNetCard> two = new List<ShootNetCard>();
        List<ShootNetCard> three = new List<ShootNetCard>();
        List<ShootNetCard> four = new List<ShootNetCard>();
        List<ShootNetCard> five = new List<ShootNetCard>();
        List<ShootNetCard> six = new List<ShootNetCard>();
        List<ShootNetCard> seven = new List<ShootNetCard>();
        List<ShootNetCard> eight = new List<ShootNetCard>();
        List<ShootNetCard> nine = new List<ShootNetCard>();
        List<ShootNetCard> ten = new List<ShootNetCard>();
        List<ShootNetCard> _j = new List<ShootNetCard>();
        List<ShootNetCard> q = new List<ShootNetCard>();
        List<ShootNetCard> k = new List<ShootNetCard>();

        for (int i = 0; i < ListCard.Count; i++)
        {
            switch (ListCard[i].idCard)
            {
                case 1:
                    _as.Add(ListCard[i]);
                    break;
                case 2:
                    two.Add(ListCard[i]);
                    break;
                case 3:
                    three.Add(ListCard[i]);
                    break;
                case 4:
                    four.Add(ListCard[i]);
                    break;
                case 5:
                    five.Add(ListCard[i]);
                    break;
                case 6:
                    six.Add(ListCard[i]);
                    break;
                case 7:
                    seven.Add(ListCard[i]);
                    break;
                case 8:
                    eight.Add(ListCard[i]);
                    break;
                case 9:
                    nine.Add(ListCard[i]);
                    break;
                case 10:
                    ten.Add(ListCard[i]);
                    break;
                case 11:
                    _j.Add(ListCard[i]);
                    break;
                case 12:
                    q.Add(ListCard[i]);
                    break;
                case 13:
                    k.Add(ListCard[i]);
                    break;
            }
        }
        int pair = 0;
        int score = 0;
        OrganizeList(ListCard, 0);

        List<List<ShootNetCard>> bigListCard = new List<List<ShootNetCard>>();

        bigListCard.Add(_as);
        bigListCard.Add(two);
        bigListCard.Add(three);
        bigListCard.Add(four);
        bigListCard.Add(five);
        bigListCard.Add(six);
        bigListCard.Add(seven);
        bigListCard.Add(eight);
        bigListCard.Add(nine);
        bigListCard.Add(ten);
        bigListCard.Add(_j);
        bigListCard.Add(q);
        bigListCard.Add(k);

        for (int i = 0; i < bigListCard.Count; i++)
        {
            if (bigListCard[i].Count == 0) { continue; }
            if (i <= 7)
            {
                int points = 1;
                if (bigListCard[i].Count > 1) { pair = 1; }
                for (int j = 1; j <= 5; j++)
                {
                    if (bigListCard[i + j].Count == 0) { break; }
                    if (bigListCard[i + j].Count > 1) { pair++; }
                    points++;
                }

                if (points >= 3)
                {
                    if (pair > 0)
                    {
                        score = points * 2;
                        break;
                    }
                    else
                    {
                        score = points;
                        break;
                    }

                }
            }

            if (i == 8)
            {
                int points = 1;
                if (bigListCard[i].Count > 1) { pair = 1; }
                for (int j = 1; j <= 4; j++)
                {
                    if (bigListCard[i + j].Count == 0) { break; }
                    if (bigListCard[i + j].Count > 1) { pair++; }
                    points++;
                }

                if (points >= 3)
                {
                    if (pair > 0)
                    {
                        score = points * 2;
                        break;
                    }
                    else
                    {
                        score = points;
                        break;
                    }

                }
            }

            if (i == 9)
            {
                int points = 1;
                if (bigListCard[i].Count > 1) { pair = 1; }
                for (int j = 1; j <= 3; j++)
                {
                    if (bigListCard[i + j].Count == 0) { break; }
                    if (bigListCard[i + j].Count > 1) { pair++; }
                    points++;
                }

                if (points >= 3)
                {
                    if (pair > 0)
                    {
                        score = points * 2;
                        break;
                    }
                    else
                    {
                        score = points;
                        break;
                    }

                }
            }


            if (i == 10)
            {
                int points = 1;
                if (bigListCard[i].Count > 1) { pair = 1; }
                for (int j = 1; j <= 2; j++)
                {
                    if (bigListCard[i + j].Count == 0) { break; }
                    if (bigListCard[i + j].Count > 1) { pair++; }
                    points++;
                }

                if (points >= 3)
                {
                    if (pair > 0)
                    {
                        score = points * 2;
                        break;
                    }
                    else
                    {
                        score = points;
                        break;
                    }

                }
            }
        }

        for (int i = 0; i < bigListCard.Count; i++)
        {
            bigListCard[i].Clear();
        }

        return score;

    }

    public int Fifteen(List<ShootNetCard> listCard)
    {
        int score = 0;
        int points = 0;

        //Checking Combination of 5
        for (int i = 0; i < listCard.Count; i++)
        {
            points += listCard[i].valueCard;
        }

        if (points == 15) { score += 2; points = 0; }
        else { points = 0; }

        //Checking Combination of 4
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < listCard.Count; j++)
            {
                if (j == i) { continue; }
                points += listCard[j].valueCard;
            }
            if (points == 15) { score += 2; points = 0; }
            else { points = 0; }
        }

        //Checking Combination of 3
        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                if (k <= i) { continue; }

                for (int j = 0; j < listCard.Count; j++)
                {
                    if (j == i || j == k) { continue; }
                    points += listCard[j].valueCard;
                }
                if (points == 15) { score += 2; points = 0; }
                else { points = 0; }
            }
        }

        //Checking Combination of 2
        for (int i = 0; i < 5; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                if (k <= i) { continue; }

                points = listCard[i].valueCard + listCard[k].valueCard;

                if (points == 15) { score += 2; points = 0; }
                else { points = 0; }
            }
        }

        return score;

    }

    private void OrganizeList(List<ShootNetCard> list, int type)
    {
        if (list.Count == 0) { return; }


        switch (type)
        {
            case 0:
                list.Sort(delegate (ShootNetCard x, ShootNetCard y)
                {
                    return x.idCard.CompareTo(y.idCard); //par y escalera
                });
                break;
            case 1:
                list.Sort(delegate (ShootNetCard x, ShootNetCard y)
                {
                    return x.pal.CompareTo(y.pal); //Palo
                });
                break;
            default:
                print("No index found");
                break;
        }


    }
    public void PlayerFinish()
    {
        photonView.RPC(nameof(RPC_PlayerFinish), RpcTarget.All);
    }
    [PunRPC]
    void RPC_PlayerFinish()
    {
        playerFinish++;
        if (playerFinish >= 2)
        {
            CalculateScore();
            GameNetManager.Instance.finish = true;
        }
    }
}
