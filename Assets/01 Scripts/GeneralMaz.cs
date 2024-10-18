using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralMaz : MonoBehaviour
{
    public static GeneralMaz Instance;
    public GameManager gameManager;
    public Controller player1;
    public Controller player2;
    public GameObject IA;
    public Animator animMaz;
    public Animator centralAnimator;
    public GameObject buttCut;

    public bool Network;

    private void Awake()
    {
        Instance = this;
    }
    public void FinshAnim()
    {
        if (Network)
        {
            GameNetManager.Instance.Repart();
            //GameNetManager.Instance.UltimateCut();
            GameNetManager.Instance.ActivateSelectPanel();
            return;

        }
        if (!player1.cribbagePlayer)
        {
            Debug.Log("Enable");
            if (PegsScoreManager.isNewGameStarted)
            {
                buttCut.SetActive(true);
            }
            else
            {
                buttCut.SetActive(false);
                Invoke(nameof(finishMaz), 1.2f);
            }
            //buttCut.GetComponent<Button>().interactable = true;
        }
        else if (!player2.cribbagePlayer)
        {
            buttCut.SetActive(false);
            //buttCut.GetComponent<Button>().interactable = false;
            Invoke(nameof(finishMaz), 1.2f);
        }

        //
    }

    public void finishMaz()
    {
        if (Network) return;
        buttCut.SetActive(false);

        centralAnimator.SetTrigger("Start");
        animMaz.SetTrigger("Finish");
        GameObject.FindObjectOfType<Cribbage>().CribbageStart();
        Destroy(this.gameObject);
    }

    public void FirstCard()
    {
        if (Network)
        {

            GameObject.Find("1").GetComponent<PlayerNet>().FirstCard();
            GameObject.Find("2").GetComponent<PlayerNet>().FirstCard();
            return;

        }
        gameManager.InstanceCardBlank();
        IA.SetActive(true);
        IA.GetComponentInChildren<SpriteRenderer>().enabled = true;

    }

    public void Repart()
    {
        GetComponent<Animator>().SetTrigger("Repart");
    }

}
