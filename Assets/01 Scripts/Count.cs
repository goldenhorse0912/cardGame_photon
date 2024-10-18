using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    [Header("---------------- POSITIONS ----------------")]
    public List<GameObject> positionsRed = new List<GameObject>();
    public List<GameObject> positionsBlue = new List<GameObject>();
    public List<GameObject> startPositionsBlue = new List<GameObject>();
    public List<GameObject> startPositionsRed = new List<GameObject>();

    [Header("---------------- REFERENCES ----------------")]
    public GameObject pinRed;
    public GameObject pinBlue;
    public GameObject pinRedPast;
    public GameObject pinBluePast;

    public int pinRedScore;
    public int pinBlueScore;

    public static Count instance;

    private void Awake()
    {
        instance = this;
        pinRedScore = PegsScoreManager.RedPegsScore;
        pinBlueScore = PegsScoreManager.BluePegsScore;
        Debug.Log("Red Score: " + PegsScoreManager.RedPegsScore + "  BlueScoreManager: " + PegsScoreManager.BluePegsScore);
        MoveRedPin();
        MoveBluePin();
    }

    public void SetRedPegScore(int score)
    {
        Debug.Log("Old Red Score " + pinRedScore);
        pinRedScore += score;
        PegsScoreManager.RedPegsScore = pinRedScore;
        Debug.Log("New Red Score " + pinRedScore);
    }

    public void SetBluePegScore(int score)
    {
        Debug.Log("Old Blue Score " + pinBlueScore);
        pinBlueScore += score;
        Debug.Log("New Blue Score " + pinBlueScore);
        PegsScoreManager.BluePegsScore = pinBlueScore;
    }

    public void MovePinInd(int player, int score)
    {
        int prevPosition;
        switch (player)
        {
            case 1:
                prevPosition = 0;
                foreach (GameObject g in positionsRed)
                {
                    if (g.activeSelf)
                    {
                        prevPosition++;
                    }
                }
                Debug.Log("333Score " + score);
                if (score > 121) { score = score % 121; }
                foreach (GameObject g in positionsRed)
                {
                    if (g.activeSelf && prevPosition > 1)
                    {
                        g.SetActive(false);
                        break;
                    }
                }
                Debug.Log("444Score " + score);

                positionsRed[score - 1].SetActive(true);
                pinRed.transform.SetParent(positionsRed[score - 1].transform, false);
                pinRed.transform.localPosition = Vector3.zero;
                if (PegsScoreManager.RedPegsPastScore != 0)
                {
                    positionsRed[PegsScoreManager.RedPegsPastScore - 1].SetActive(true);
                    pinRedPast.transform.SetParent(positionsRed[PegsScoreManager.RedPegsPastScore - 1].transform, false);
                    pinRedPast.transform.localPosition = Vector3.zero;
                }

                if (prevPosition == 0) { startPositionsRed[0].SetActive(false); }
                if (prevPosition == 1) { startPositionsRed[1].SetActive(false); }
                break;

            case 2:
                prevPosition = 0;
                foreach (GameObject g in positionsBlue)
                {
                    if (g.activeSelf)
                    {
                        prevPosition++;
                    }
                }
                Debug.Log("111Score " + score);
                if (score > 121) { score = score % 121; }
                foreach (GameObject g in positionsBlue)
                {
                    if (g.activeSelf && prevPosition > 1)
                    {
                        g.SetActive(false);
                        break;
                    }
                }
                Debug.Log("222Score " + score);
                positionsBlue[score - 1].SetActive(true);
                pinBlue.transform.SetParent(positionsBlue[score - 1].transform, false);
                pinBlue.transform.localPosition = Vector3.zero;
                if (PegsScoreManager.BluePegsPastScore != 0)
                {
                    positionsBlue[PegsScoreManager.BluePegsPastScore - 1].SetActive(true);
                    pinBluePast.transform.SetParent(positionsBlue[PegsScoreManager.BluePegsPastScore - 1].transform, false);
                    pinBluePast.transform.localPosition = Vector3.zero;
                }

                if (prevPosition == 0) { startPositionsBlue[0].SetActive(false); }
                if (prevPosition == 1) { startPositionsBlue[1].SetActive(false); }
                break;

            case 0:
                if (GameNetManager.Instance.thisPlayer.playerNumber == 1)
                {
                    prevPosition = 0;
                    foreach (GameObject g in positionsBlue)
                    {
                        if (g.activeSelf)
                        {
                            prevPosition++;
                        }
                    }
                    if (score > 121) { score = score % 121; }
                    foreach (GameObject g in positionsBlue)
                    {
                        if (g.activeSelf && prevPosition > 1)
                        {
                            g.SetActive(false);
                            break;
                        }
                    }
                    positionsBlue[score - 1].SetActive(true);
                    if (prevPosition == 0) { startPositionsBlue[0].SetActive(false); }
                    if (prevPosition == 1) { startPositionsBlue[1].SetActive(false); }
                }
                else
                {
                    prevPosition = 0;
                    foreach (GameObject g in positionsRed)
                    {
                        if (g.activeSelf)
                        {
                            prevPosition++;
                        }
                    }
                    if (score > 121) { score = score % 121; }
                    foreach (GameObject g in positionsRed)
                    {
                        if (g.activeSelf && prevPosition > 1)
                        {
                            g.SetActive(false);
                            break;
                        }
                    }
                    positionsRed[score - 1].SetActive(true);
                    if (prevPosition == 0) { startPositionsRed[0].SetActive(false); }
                    if (prevPosition == 1) { startPositionsRed[1].SetActive(false); }
                }
                break;
        }
    }

    public void MoveRedPin()
    {
        if (pinRedScore > 0)
            MovePinInd(1, pinRedScore);
    }

    public void MoveBluePin()
    {
        if (pinBlueScore > 0)
            MovePinInd(2, pinBlueScore);
    }

}
