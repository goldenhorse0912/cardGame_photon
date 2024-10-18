using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Drawing;
using System.Linq;
using System;


public class Scoring : MonoBehaviour
{


    [Header("Row Ref -------------------")]
    public List<GameObject> row1;
    public List<GameObject> row2;
    public List<GameObject> row3;
    public List<GameObject> row4;
    public List<GameObject> row5;

    [Header("Colum Ref-------------------")]
    public List<GameObject> colum1;
    public List<GameObject> colum2;
    public List<GameObject> colum3;
    public List<GameObject> colum4;
    public List<GameObject> colum5;

    [Header("Points Ref-----------------")]
    public List<int> rowsPoints;
    public List<int> columsPoints;

    [Header("Text Ref-----------------")]
    public List<TextMeshPro> t_rowPoints;
    public List<TextMeshPro> t_columPoints;

    public int matchPar;
    public int matchParC;
    public int matchStair;

    public List<int> rowsStairMore;
    public List<int> rowsStairMin;
    public List<int> rowsStair;

    public List<int> rowsFifteen;

    public List<int> columsStairMore;
    public List<int> columsStairMin;
    public List<int> columsStair;

    public List<int> columsFifteen;

    public int totalProw;
    public int totalPcolum;


    public Animator cribbageHand;
    public Controller controller1;
    public Controller controller2;
    public GameObject backrow;
    public GameObject backcolum;

    [Header("New Values")]

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

    int cribaggeScore;
    int maxScore = 121;
    bool scoring;
    public int extraPointRow = 0;
    public int extraPointColum = 0;
    private void Start()
    {

        scoring = false;

    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            CheckPoints();

        }*/
        if (Input.GetKeyDown(KeyCode.B))
        {

            cribbageHand.SetBool("Start", false);
            Invoke(nameof(CribbageFinish), 3f);

        }

        if (GameManager.Instance.startGame && controller1.myCarts.Count <= 0 && controller2.myCarts.Count <= 0 && !scoring)
        {
            Debug.Log("Calculating Scores");
            scoring = true;
            //CheckPoints();
            //print(CribbageScore().ToString());
            CalculateScore();
        }

    }
    void CribbageFinish()
    {
        Cribbage.Instance.InstanceCribbageHand();
    }

    //public void CheckPoints()
    //{
    //    Par(row1, rowsPoints, 1, matchPar);
    //    Par(row2, rowsPoints, 2, matchPar);
    //    Par(row3, rowsPoints, 3, matchPar);
    //    Par(row4, rowsPoints, 4, matchPar);
    //    Par(row5, rowsPoints, 5, matchPar);


    //    Escalera(row1, " --> row1",0);
    //    Escalera(row2, " --> row2",1);
    //    Escalera(row3, " --> row3",2);
    //    Escalera(row4, " --> row4",3);
    //    Escalera(row5, " --> row5",4);

    //    Fifteen(row1, rowsPoints,0, "Fila");
    //    Fifteen(row2, rowsPoints, 1, "Fila");
    //    Fifteen(row3, rowsPoints, 2, "Fila");
    //    Fifteen(row4, rowsPoints, 3, "Fila");
    //    Fifteen(row5, rowsPoints, 4, "Fila");

    //    Par(colum1, columsPoints, 1, matchParC);
    //    Par(colum2, columsPoints, 2, matchParC);
    //    Par(colum3, columsPoints, 3, matchParC);
    //    Par(colum4, columsPoints, 4, matchParC);
    //    Par(colum5, columsPoints, 5, matchParC);


    //    Escalera(colum1, " --> colum1", 0);
    //    Escalera(colum2, " --> colum2", 1);
    //    Escalera(colum3, " --> colum3", 2);
    //    Escalera(colum4, " --> colum4", 3);
    //    Escalera(colum5, " --> colum5", 4);


    //    Fifteen(colum1, columsPoints, 0, "Columna");
    //    Fifteen(colum2, columsPoints, 1, "Columna");
    //    Fifteen(colum3, columsPoints, 2, "Columna");
    //    Fifteen(colum4, columsPoints, 3, "Columna");
    //    Fifteen(colum5, columsPoints, 4, "Columna");


    //    ScoringLogic(rowsPoints, t_rowPoints, true);
    //    ScoringLogic(columsPoints, t_columPoints, false);



    //    GameManager.Instance.Finish();

    //}


    public int CalculateExtraPoint(List<GameObject> cardslist)
    {
        List<ShootCart> jackCardsList = new List<ShootCart>();
        for (int i = 0; i < cardslist.Count; i++)
        {
            if (i != 2 && cardslist[i].GetComponent<ShootCart>().idCard == 11)
            {
                jackCardsList.Add(cardslist[i].GetComponent<ShootCart>());
            }
        }
        Debug.Log("Extra Point Jack Count " + jackCardsList.Count);
        if (jackCardsList.Count > 0)
        {
            for (int i = 0; i < jackCardsList.Count; i++)
            {
                Debug.Log(jackCardsList[i].pal + "SCORED EXTRA POINT " + cardslist[2].GetComponent<ShootCart>().pal);

                if (jackCardsList[i].pal == cardslist[2].GetComponent<ShootCart>().pal)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    void CalculateScore()
    {
        int instantExtraPointCenterJack = 0;
        ///TODO: If the middle cut card is a jack, the dealer gets awarded 2 points. These 2 points are scored on the board right away



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

        if (row3[2].GetComponent<ShootCart>().idCard != 11)
        {
            extraPointColum = CalculateExtraPoint(colum3);
            extraPointRow = CalculateExtraPoint(row3);
        }



        Debug.Log("Scored Extra Row " + extraPointRow);
        Debug.Log("Scored Extra Column " + extraPointColum);


        if (controller1.cribbagePlayer)
        {
            if (controller1.rowPlayer)
            {
                if (extraPointRow != 0)
                {
                    Debug.Log("Extra Row Point for 1");
                    GameManager.Instance.RewardDelaerWith1Point();
                }
            }
            else
            {
                if (extraPointColum != 0)
                {
                    GameManager.Instance.RewardDelaerWith1Point();
                    Debug.Log("Extra Column Point for 1");
                }
            }
        }
        else if (controller2.cribbagePlayer)
        {
            if (controller2.rowPlayer)
            {
                if (extraPointRow != 0)
                {

                    GameManager.Instance.RewardDelaerWith1Point();
                    Debug.Log("Extra Row Point for 2");
                }
            }
            else
            {
                if (extraPointColum != 0)
                {
                    GameManager.Instance.RewardDelaerWith1Point();
                    Debug.Log("Extra Column Point for 1");
                }
            }
        }

        ///TODO: Jack Played in middle column or row with same suit with middle cut card it will be +1 point to Dealer
        ///



        cribaggeScore = scores(CribbaggeCards);

        int row = int.Parse(RowsTexts[0].text) + int.Parse(RowsTexts[1].text) + int.Parse(RowsTexts[2].text) + int.Parse(RowsTexts[3].text) + int.Parse(RowsTexts[4].text);
        int col = int.Parse(ColumsTexts[0].text) + int.Parse(ColumsTexts[1].text) + int.Parse(ColumsTexts[2].text) + int.Parse(ColumsTexts[3].text) + int.Parse(ColumsTexts[4].text);

        PegsScoreManager.BluePegsPastScore = PegsScoreManager.BluePegsScore;
        PegsScoreManager.RedPegsPastScore = PegsScoreManager.RedPegsScore;

        controller1.RecivePoints(row, col);
        controller2.RecivePoints(row, col);
        Debug.Log("Crubage Score" + cribaggeScore);
        GameManager.Instance.RevealAICribagge();
        controller1.ShowCribbageScore(cribaggeScore);
        controller2.ShowCribbageScore(cribaggeScore);
        GameManager.Instance.Finish();
        GameManager.Instance.CheckForGameResults();
        //TotalScore();
    }

    int scores(List<ShootCart> ListRow)
    {
        return ScorePar(ListRow) + Fifteen(ListRow) + Stair(ListRow) + Color(ListRow);
    }

    int Color(List<ShootCart> ListCard)
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

    public int Fifteen(List<ShootCart> listCard)
    {
        int score = 0;

        // Early exit if there are fewer than 2 cards, no combinations are possible.
        if (listCard == null || listCard.Count < 2)
        {
            return 0;
        }

        // Ensure all card values are within the valid range (1 to 13)
        foreach (var card in listCard)
        {
            if (card.valueCard < 1 || card.valueCard > 13)
            {
                throw new ArgumentException("Card values must be between 1 and 13.");
            }
        }

        // Helper method to calculate combinations of any size
        IEnumerable<List<int>> GetCombinations(List<int> cards, int combinationSize)
        {
            return from indices in Enumerable.Range(0, 1 << cards.Count)
                   where BitCount(indices) == combinationSize
                   select cards.Where((v, i) => (indices & (1 << i)) != 0).ToList();
        }

        // BitCount to calculate number of 1s in binary representation (i.e., how many cards are selected in a combination)
        int BitCount(int n)
        {
            int count = 0;
            while (n != 0)
            {
                count++;
                n &= (n - 1); // Remove the lowest set bit
            }
            return count;
        }

        // Extract card values
        var cardValues = listCard.Select(c => c.valueCard).ToList();

        // Check combinations of size 2, 3, 4, and 5
        for (int size = 2; size <= 5; size++)
        {
            foreach (var combination in GetCombinations(cardValues, size))
            {
                if (combination.Sum() == 15)
                {
                    score += 2;
                }
            }
        }

        return score;
    }


    /*public int Fifteen(List<ShootCart> listCard)
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

    }*/
    /* int Stair(List<ShootCart> ListCard)
     {
         List<ShootCart> _as = new List<ShootCart>();
         List<ShootCart> two = new List<ShootCart>();
         List<ShootCart> three = new List<ShootCart>();
         List<ShootCart> four = new List<ShootCart>();
         List<ShootCart> five = new List<ShootCart>();
         List<ShootCart> six = new List<ShootCart>();
         List<ShootCart> seven = new List<ShootCart>();
         List<ShootCart> eight = new List<ShootCart>();
         List<ShootCart> nine = new List<ShootCart>();
         List<ShootCart> ten = new List<ShootCart>();
         List<ShootCart> _j = new List<ShootCart>();
         List<ShootCart> q = new List<ShootCart>();
         List<ShootCart> k = new List<ShootCart>();

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

         List<List<ShootCart>> bigListCard = new List<List<ShootCart>>();

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

     }*/


    int Stair(List<ShootCart> ListCard)
    {
        // Group cards by rank
        List<ShootCart> _as = new List<ShootCart>();
        List<ShootCart> two = new List<ShootCart>();
        List<ShootCart> three = new List<ShootCart>();
        List<ShootCart> four = new List<ShootCart>();
        List<ShootCart> five = new List<ShootCart>();
        List<ShootCart> six = new List<ShootCart>();
        List<ShootCart> seven = new List<ShootCart>();
        List<ShootCart> eight = new List<ShootCart>();
        List<ShootCart> nine = new List<ShootCart>();
        List<ShootCart> ten = new List<ShootCart>();
        List<ShootCart> _j = new List<ShootCart>();
        List<ShootCart> q = new List<ShootCart>();
        List<ShootCart> k = new List<ShootCart>();

        // Categorize by rank
        for (int i = 0; i < ListCard.Count; i++)
        {
            switch (ListCard[i].idCard)
            {
                case 1: _as.Add(ListCard[i]); break;
                case 2: two.Add(ListCard[i]); break;
                case 3: three.Add(ListCard[i]); break;
                case 4: four.Add(ListCard[i]); break;
                case 5: five.Add(ListCard[i]); break;
                case 6: six.Add(ListCard[i]); break;
                case 7: seven.Add(ListCard[i]); break;
                case 8: eight.Add(ListCard[i]); break;
                case 9: nine.Add(ListCard[i]); break;
                case 10: ten.Add(ListCard[i]); break;
                case 11: _j.Add(ListCard[i]); break;
                case 12: q.Add(ListCard[i]); break;
                case 13: k.Add(ListCard[i]); break;
            }
        }

        int totalScore = 0;

        List<List<ShootCart>> bigListCard = new List<List<ShootCart>>() {
        _as, two, three, four, five, six, seven, eight, nine, ten, _j, q, k
    };

        // Loop through to find all combinations of valid straights (3 consecutive ranks)
        for (int i = 0; i <= 9; i++) // Stop at 9 because we need at least 3 consecutive cards
        {
            List<ShootCart> firstRankCards = bigListCard[i];
            List<ShootCart> secondRankCards = bigListCard[i + 1];
            List<ShootCart> thirdRankCards = bigListCard[i + 2];

            // For each combination of cards from three consecutive ranks, count points
            foreach (ShootCart card1 in firstRankCards)
            {
                foreach (ShootCart card2 in secondRankCards)
                {
                    foreach (ShootCart card3 in thirdRankCards)
                    {
                        // Ensure the cards belong to a straight (by their rank, and potentially different suits)
                        totalScore += 3;  // Each valid combination of consecutive cards gets 3 points
                    }
                }
            }
        }

        // Clear lists after calculation
        for (int i = 0; i < bigListCard.Count; i++)
        {
            bigListCard[i].Clear();
        }

        return totalScore;
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


    int ScorePar(List<ShootCart> ListCardUser)
    {
        //int matchR0 = 0;
        int score = 0;
        List<ShootCart> _as = new List<ShootCart>();
        List<ShootCart> two = new List<ShootCart>();
        List<ShootCart> three = new List<ShootCart>();
        List<ShootCart> four = new List<ShootCart>();
        List<ShootCart> five = new List<ShootCart>();
        List<ShootCart> six = new List<ShootCart>();
        List<ShootCart> seven = new List<ShootCart>();
        List<ShootCart> eight = new List<ShootCart>();
        List<ShootCart> nine = new List<ShootCart>();
        List<ShootCart> ten = new List<ShootCart>();
        List<ShootCart> j = new List<ShootCart>();
        List<ShootCart> q = new List<ShootCart>();
        List<ShootCart> k = new List<ShootCart>();

        List<ShootCart> ListCard = new List<ShootCart>();
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

        print($"Pair Score is= {score}");
        return score;
    }

   

    private int CheckCardLists(List<ShootCart> cardList)
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






    //private void Par(List<GameObject> listRef, List<int> scoring, int Posnumber, int _matchPar)
    //{
    //    Posnumber -= 1;

    //    for (int i = 0; i < listRef.Count; i++)
    //    {
    //        if(i == listRef.Count - 1)
    //        {
    //            _matchPar = 0;
    //            return;
    //        }

    //        if(listRef[i].GetComponentInChildren<CartMain>().idCart  == listRef[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            _matchPar++;

    //            if(_matchPar == 1)
    //            {
    //                scoring[Posnumber] += 2;
    //            }
    //            if (_matchPar == 2)
    //            {
    //                scoring[Posnumber] += 4;
    //            }
    //            if (_matchPar == 3)
    //            {
    //                scoring[Posnumber] += 8;
    //            }

    //        }
    //        else if(listRef[i].GetComponentInChildren<CartMain>().idCart != listRef[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            _matchPar = 0;
    //        }
    //    }

    //}
    //private void JoinPoint(List<GameObject> listRef, bool isRow, List<int> scoring, int posNumber)
    //{
    //    if (isRow)
    //    {
    //        rowsStair[0] += rowsStairMore[0] + rowsStairMin[0];
    //        rowsStair[1] += rowsStairMore[1] + rowsStairMin[1];
    //        rowsStair[2] += rowsStairMore[2] + rowsStairMin[2];
    //        rowsStair[3] += rowsStairMore[3] + rowsStairMin[3];
    //        rowsStair[4] += rowsStairMore[4] + rowsStairMin[4];

    //        if (rowsStair[0] >= 2)
    //        {
    //            scoring[0] += rowsStair[0] + 1;
    //        }
    //        if (rowsStair[1] >= 2)
    //        {
    //            scoring[1] += rowsStair[1] + 1;
    //        }
    //        if (rowsStair[2] >= 2)
    //        {
    //            scoring[2] += rowsStair[2] + 1;
    //        }
    //        if (rowsStair[3] >= 2)
    //        {
    //            scoring[3] += rowsStair[3] + 1;
    //        }
    //        if (rowsStair[4] >= 2)
    //        {
    //            scoring[4] += rowsStair[4] + 1;
    //        }

    //    }
    //    else
    //    {
    //        columsStair[0] += columsStairMore[0] + columsStairMin[0];
    //        columsStair[1] += columsStairMore[1] + columsStairMin[1];
    //        columsStair[2] += columsStairMore[2] + columsStairMin[2];
    //        columsStair[3] += columsStairMore[3] + columsStairMin[3];
    //        columsStair[4] += columsStairMore[4] + columsStairMin[4];
    //         /* scoring[0] += columsStair[0];
    //         scoring[1] += columsStair[1];
    //         scoring[2] += columsStair[2];
    //         scoring[3] += columsStair[3];
    //         scoring[4] += columsStair[4];*/


    //    }



    //}

    //private void Escalera(List<GameObject> list, string str, int pos)
    //{

    //    int match = 1;

    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        if(i == list.Count - 1) {
    //            if (match >= 3)
    //            {
    //                rowsPoints[pos] += match;
    //            }
    //            return; }

    //        if (list[i].GetComponentInChildren<CartMain>().idCart == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            match++;

    //        }
    //        if (list[i].GetComponentInChildren<CartMain>().idCart + 1 == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            match++;

    //        }
    //        if (list[i].GetComponentInChildren<CartMain>().idCart - 1 == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            match++;

    //        }
    //        else 
    //        {

    //            {
    //                if (match >= 3)
    //                {
    //                    rowsPoints[pos] += match;

    //                    match = 1;

    //                }
    //                else
    //                {
    //                    match = 1;

    //                }
    //            }


    //        }
    //    }

    //}

    //private void Stair(List<GameObject> listRef, List<int> scoring=null, int Posnumber=0, bool isRow= false)
    //{

    //    Posnumber -= 1;



    //    for (int i = 0; i < listRef.Count; i++)
    //    {
    //        if(i == listRef.Count-1)
    //        {
    //            if (isRow)
    //            {
    //                JoinPoint(listRef, true, scoring, Posnumber);
    //            }
    //            else
    //            {
    //                JoinPoint(listRef, false, scoring, Posnumber);
    //            }
    //            return;
    //        }


    //        if(listRef[i].GetComponentInChildren<CartMain>().idCart == listRef[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            if (isRow)
    //            {
    //                rowsStair[Posnumber]++;
    //            }
    //            else
    //            {
    //                columsStair[Posnumber]++;
    //            }
    //        }

    //        if (listRef[i].GetComponentInChildren<CartMain>().idCart + 1 == listRef[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {


    //            if (isRow)
    //            {
    //                ++rowsStairMore[Posnumber];

    //            }
    //            else
    //            {
    //                ++columsStairMore[Posnumber];


    //            }






    //        }
    //        if (listRef[i].GetComponentInChildren<CartMain>().idCart - 1 == listRef[i + 1].GetComponentInChildren<CartMain>().idCart)
    //        {
    //            if (isRow)
    //            {
    //                ++rowsStairMin[Posnumber];


    //            }
    //            else
    //            {
    //                ++columsStairMin[Posnumber];


    //            }


    //        }




    //    }

    //}


    //private void Fifteen(List<GameObject> listRef, List<int> scoring, int Posnumber, string str)
    //{

    //    int suma = 0;
    //    int sumaCompleta = 0;
    //    int index = 0;


    //    foreach (GameObject a in listRef)
    //    {
    //        CartMain cart =  a.GetComponentInChildren<CartMain>();
    //        index++;
    //        sumaCompleta = cart.cartScore;

    //        for (int i = index; i < listRef.Count; i++)
    //        {

    //            if(listRef[i].GetComponentInChildren<CartMain>() == null)
    //            {
    //                continue;
    //            }

    //            suma = cart.cartScore + listRef[i].GetComponentInChildren<CartMain>().cartScore;
    //            sumaCompleta += listRef[i].GetComponentInChildren<CartMain>().cartScore;

    //            if(sumaCompleta == 15)
    //            {
    //                scoring[Posnumber] += 2;
    //                sumaCompleta = 0;
    //            }
    //            else if(sumaCompleta > 15)
    //            {
    //                sumaCompleta = 0;
    //            }
    //            if (suma == 15)
    //            {

    //                scoring[Posnumber] += 2;

    //            }

    //        }



    //    }
    //}





    //private int CribbageScore()
    //{
    //    List<GameObject> list = GameManager.Instance.cribbageCards;
    //    int matchPar = 0;
    //    int matchStair = 1;
    //    int score = 0;
    //    int scorePar = 0;
    //    int scoreFift = 0;
    //    int scoreStair = 0;
    //    int indexFift = 0;
    //    int suma = 0;
    //    int sumaTotal = 0;
    //    bool first = false;
    //    int iCount = 0;

    //    //CeckPar

    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        if(i < 3)
    //        {
    //            if (list[i].GetComponentInChildren<CartMain>().idCart == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //            {
    //                matchPar++;

    //                if (matchPar == 1)
    //                {
    //                    scorePar += 2;
    //                }

    //                if (matchPar == 2)
    //                {
    //                    scorePar += 4;
    //                }

    //                if (matchPar == 3)
    //                {
    //                    scorePar += 8;
    //                }
    //            }
    //            else
    //            {
    //                matchPar = 0;
    //            }
    //        }


    //    }


    //    //Check Stair
    //    for (int i = 0; i < list.Count; i++)
    //     {
    //         if(i < 3)
    //         {
    //             if(i < 2)
    //             {
    //                 if (list[i].GetComponentInChildren<CartMain>().idCart == list[i + 1].GetComponentInChildren<CartMain>().idCart
    //                 && list[i + 1].GetComponentInChildren<CartMain>().idCart + 1 == list[i + 2].GetComponentInChildren<CartMain>().idCart ||
    //                  list[i].GetComponentInChildren<CartMain>().idCart - 1 == list[i + 2].GetComponentInChildren<CartMain>().idCart)
    //                 {
    //                     matchStair++;
    //                 }
    //             }

    //             if (list[i].GetComponentInChildren<CartMain>().idCart + 1 == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //             {
    //                 matchStair++;

    //             }
    //             if (list[i].GetComponentInChildren<CartMain>().idCart - 1 == list[i + 1].GetComponentInChildren<CartMain>().idCart)
    //             {
    //                 matchStair++;

    //             }

    //         }

    //         if (matchStair >= 3)
    //         {
    //             scoreStair = matchStair;
    //         }
    //     }

    //    //CheckFifteen
    //    foreach (GameObject a in list)
    //    {
    //        CartMain cart = a.GetComponentInChildren<CartMain>();

    //        sumaTotal = cart.cartScore;

    //        indexFift++;


    //        for (int i = indexFift; i < list.Count; i++)
    //        {
    //            suma = cart.cartScore + list[i].GetComponentInChildren<CartMain>().cartScore;
    //            sumaTotal += list[i].GetComponentInChildren<CartMain>().cartScore;

    //            if(suma == 15)
    //            {
    //                scoreFift += 2;
    //            }




    //        }





    //    }


    //    score = scoreStair + scorePar + scoreFift;

    //    return score;
    //}




    //private void ScoringLogic(List<int> listRefCorR, List<TextMeshPro> t_columOrRow, bool isRow)
    //{




    //    for (int i = 0; i < listRefCorR.Count; i++)
    //    {
    //        t_columOrRow[i].text = listRefCorR[i].ToString();
    //        if (isRow)
    //        {
    //            totalProw += listRefCorR[i];
    //        }
    //        if (!isRow)
    //        {
    //            totalPcolum += listRefCorR[i];
    //        }
    //    }
    //    SendScore();

    //}
    //private void SendScore()
    //{
    //    controller1.CribbagePoint(CribbageScore());
    //    controller2.CribbagePoint(CribbageScore());

    //    controller1.RecivePoints(totalProw, totalPcolum);

    //    controller2.RecivePoints(totalProw, totalPcolum);
    //    ChangeColorBack(controller1.colorPlayer, controller1.rowPlayer);
    //    ChangeColorBack(controller2.colorPlayer, controller2.rowPlayer);
    //    backrow.SetActive(true);
    //    backcolum.SetActive(true);


    //}

    //private void ChangeColorBack(Color color, bool isrow)
    //{
    //    if (isrow)
    //    {
    //        backrow.GetComponent<SpriteRenderer>().color = color;
    //    }
    //    else
    //    {
    //        backcolum.GetComponent<SpriteRenderer>().color = color;

    //    }
    //}


}

