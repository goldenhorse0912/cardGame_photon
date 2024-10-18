using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoringManager : MonoBehaviour
{
    public static ScoringManager instance;
    public List<int> scoresl = new List<int>();

    public List<ShootCart> positionsShoot;
    public List<ShootCart> Rows0;
    public List<ShootCart> Rows1;
    public List<ShootCart> Rows2;
    public List<ShootCart> Rows3;
    public List<ShootCart> Rows4;

    public List<ShootCart> Colum0;
    public List<ShootCart> Colum1;
    public List<ShootCart> Colum2;
    public List<ShootCart> Colum3;
    public List<ShootCart> Colum4;

    public List<ShootCart> CribbaggeCards;

    public List<TextMeshPro> RowsTexts;
    public List<TextMeshPro> ColumsTexts;

    public bool isMaster;
    ShootCart[] shootPositions;

    int rowsScore;
    int columsScore;
    int cribaggeScore;
    public int position;

    public List<ShootCart> testList = new List<ShootCart>();
    [HideInInspector] public int playerFinish;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        shootPositions = GameObject.FindObjectsOfType<ShootCart>();

    }

    public void CheckPositions()
    {
        if (position < 28)
        {
            position++;

        }
        if (position >= 28)
        {

            //photonView.RPC(nameof(RPC_CalculateScore), RpcTarget.Others);
        }

    }





    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {

            CalculateScore();
           
        }
    }
    
    void CalculateScore()
    {
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

        TotalScore();
    }

    void TotalScore()
    {
        PlayerNet[] players = GameObject.FindObjectsOfType<PlayerNet>();
        int row = scores(Rows0) + scores(Rows1) + scores(Rows2) + scores(Rows3) + scores(Rows4);
        int col = scores(Colum0) + scores(Colum1) + scores(Colum2) + scores(Colum3) + scores(Colum4);
        cribaggeScore = scores(CribbaggeCards);

        print("Cribbage Score= " + cribaggeScore);

        /*if (GameNetManager.Instance.rowPlayer1)
        {
            GameObject.Find("1").GetComponent<PlayerNet>().TotalScore(row);
            GameObject.Find("2").GetComponent<PlayerNet>().TotalScore(col);
            print("EL JUGADOR 1 ES ROW = PUNTOS = " + row);
        }
        else
        {
            GameObject.Find("2").GetComponent<PlayerNet>().TotalScore(row);
            GameObject.Find("1").GetComponent<PlayerNet>().TotalScore(col);
            print("EL JUGADOR 2 ES ROW = PUNTOS = " + row);

        }*/


        foreach (PlayerNet a in players)
        {
            if (GameNetManager.Instance.rowPlayer1)
            {
                if (a.PlayerNumber == 1)
                {
                    a.TotalScore(row);
                }
                else
                {
                    a.TotalScore(col);
                }
            }
            else
            {
                if (a.PlayerNumber == 1)
                {
                    a.TotalScore(col);
                }
                else
                {
                    a.TotalScore(row);
                }
            }
        }


    }

    int scores(List<ShootCart> ListRow)
    {
        return ScorePar(ListRow) + Fifteen(ListRow) + Stair(ListRow);
    }

    int Color(List<ShootCart> ListCard)
    {
        int score = 0;
        int match = 0;

        for (int i = 0; i < ListCard.Count; i++)
        {
            int index = i + 1;

            for (int a = index; a < ListCard.Count; a++)
            {
                if (ListCard[i].pal == ListCard[a].pal)
                {
                    match++;
                }
            }


        }

        if (match >= 4)
        {
            score = 5;
        }

        return score;
    }


    int ScorePar(List<ShootCart> ListCardUser)
    {
        int matchR0 = 0;
        int score = 0;

        List<ShootCart> ListCard = new List<ShootCart>();
        ListCard.AddRange(ListCardUser);

        /*for (int i = 0; i < ListCard.Count; i++)
        {
            int index = i + 1;

            for (int a = index; a < ListCard.Count; a++)
            {
                if(ListCard[i].idCard == ListCard[a].idCard)
                {
                    matchR0++;
                    scoresl.Add(ListCard[i].idCard);
                    scoresl.Sort();
                }
                
            }
        }*/

        OrganizeList(ListCard, 0);



        for (int i = 0; i < ListCard.Count; i++)
        {
            if (i + 1 < ListCard.Count)
            {
                if (ListCard[i].idCard == ListCard[i + 1].idCard)
                {
                    matchR0++;
                    score += 2;

                    if (i + 2 < ListCard.Count)
                    {
                        if (ListCard[i].idCard == ListCard[i + 2].idCard)
                        {
                            matchR0++;
                            score += 4;

                            if (i + 3 < ListCard.Count)
                            {
                                if (ListCard[i].idCard == ListCard[i + 3].idCard)
                                {
                                    matchR0++;
                                    score += 6;

                                    i += 3;


                                }

                            }


                            i += 2;


                        }

                    }

                }

            }
        }


        print($"Pair Score is= {score}");

        return score;

    }


    int Stair(List<ShootCart> ListCard)
    {
        int matchR0 = 0;
        int par = 0;
        int parCardId = 0;
        int score = 0;
        List<int> tempList = new List<int>();
        OrganizeList(ListCard, 0);


        for (int i = 0; i < ListCard.Count; i++)
        {
            if (i + 1 < ListCard.Count)
            {
                if (ListCard[i].idCard == ListCard[i + 1].idCard)
                {
                    par++;
                    parCardId = ListCard[i].idCard;
                    ListCard.RemoveAt(i);
                }
            }


        }

        OrganizeList(ListCard, 0);


        for (int i = 0; i < ListCard.Count; i++)
        {
            //if (i + 1 < ListCard.Count)
            //{

            //    if (ListCard[i].idCard == ListCard[i + 1].idCard)
            //    {
            //        par++;
            //        if (ListCard[i].idCard + 1 == ListCard[i + 2].idCard &&
            //            ListCard[i].idCard + 2 == ListCard[i + 3].idCard) //|| ListCard[i].idCard - 1 == ListCard[i + 1].idCard)
            //        {
            //            matchR0 += 2;
            //        }
            //    }
            // else
            //{
            //if (ListCard[i + 1].idCard == ListCard[i + 2].idCard)
            //{
            //    if (ListCard[i].idCard + 1 == ListCard[i + 1].idCard &&
            //        ListCard[i].idCard + 2 == ListCard[i + 3].idCard) //|| ListCard[i].idCard - 1 == ListCard[i + 1].idCard)
            //    {
            //        matchR0 += 2;
            //    }
            //}
            //else
            //     {
            if (i + 2 < ListCard.Count)
            {
                if (ListCard[i].idCard + 1 == ListCard[i + 1].idCard &&
                           ListCard[i].idCard + 2 == ListCard[i + 2].idCard) //|| ListCard[i].idCard - 1 == ListCard[i + 1].idCard)
                {
                    matchR0 += 2;
                    tempList.Add(ListCard[i].idCard + 1);
                    tempList.Add(ListCard[i].idCard + 2);
                }
            }

            //   }
            //}


            //  }

        }


        switch (matchR0)
        {
            case 2:
                score = 3;
                break;
            case 4:
                score = 4;
                break;
            case 6:
                score = 5;
                break;
        }

        if (tempList.Contains(parCardId))
        {
            if (par == 1)
            {
                score *= 2;
            }
            else if (par == 2)
            {
                score *= 3;
            }
        }

        return score;
    }

    public int Fifteen(List<ShootCart> listCard)
    {
        int score = 0;


        for (int i = 0; i < listCard.Count; i++)
        {
            int add = 0;

            for (int a = i; a < listCard.Count; a++)
            {
                add += listCard[i].valueCard + listCard[a].valueCard;

                if (add == 15)
                {
                    score += 2;
                }

            }
        }

        print("Fifteen " + "RorC " + listCard + " : ---- > " + score);

        return score;

    }

    private void OrganizeList(List<ShootCart> list, int type)
    {
        if (list.Count == 0) { return; }


        switch (type)
        {
            case 0:
                list.Sort(delegate (ShootCart x, ShootCart y)
                {
                    return x.idCard.CompareTo(y.idCard); //par y escalera
                });
                break;
            case 1:
                list.Sort(delegate (ShootCart x, ShootCart y)
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
        playerFinish++;
        if (playerFinish >= 2)
        {
            CalculateScore();
            GameNetManager.Instance.finish = true;
        }
    }
   
}


