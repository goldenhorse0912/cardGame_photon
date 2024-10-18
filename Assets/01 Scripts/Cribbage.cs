using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cribbage : MonoBehaviour
{
    public static Cribbage Instance;

    public List<GameObject> posInstance;
    public GameObject cribbageBlank;
    public Animator logo;
    public Controller controller;
    public IaMain IAController;
    
    public List<GameObject> disableCart;


    bool activador;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
        cribbageBlank.SetActive(false);
        
    }

    public void CribbageStart()
    {
        StartCoroutine(wait());
        //IAController.cribbageHand = true;

    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1.5f);
        logo.SetTrigger("Start");

        yield return new WaitForSeconds(2f);
        GameManager.Instance.StartGame();
        activador = true;
        
        Invoke(nameof(InstanceCart), 2f);
        //disableMaz = GameObject.FindGameObjectsWithTag("DestroyCribbage");
        //StartBonus();
    }

    void InstanceCart()
    {
        cribbageBlank.SetActive(true);

       /* cribbageBlank.GetComponent<Animator>().SetBool("Start", true);
        Destroy(controller.transform.GetChild(0).gameObject);
        disableCart.Add(GameObject.Find("BlankRed(Clone)"));
        disableCart.Add(GameObject.Find("BlankBlue(Clone)"));*/
        controller.InstanceCart();

    }

    public void InstanceCribbageHand()
    {
        for (int i = 0; i < GameManager.Instance.cribbageCards.Count; i++)
        {
            posInstance[i].GetComponentInChildren<SpriteRenderer>().transform.gameObject.SetActive(false);
            posInstance[i].transform.localScale = new Vector3(1,1,1);
            Instantiate(GameManager.Instance.cribbageCards[i], posInstance[i].transform);
        }

        
    }

    public void DisableAll()
    {
        disableCart[0].SetActive(false);
        disableCart[1].SetActive(false);
        controller.gameObject.SetActive(false);
        IAController.gameObject.SetActive(false);
    }

    /*public void EnableOrDisable(bool on, GameObject go)
    {
        if (on)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i].SetActive(true);
                
            }
        }
        else
        {
            for (int i = 0; i < positions.Count; i++)
            {
                positions[i].SetActive(false);

                if (positions[i].GetComponentInChildren<CartMain>())
                {
                    if (positions[i].GetComponentInChildren<CartMain>().gameObject == go)
                    {
                        positions[i].SetActive(true);

                    }
                }
               
            }
        }
    }
    public void SelectCard()
    {
        if (selectCard < 1)
        {
            selectCard++;
            

        }
        else
        {
            controller.transform.gameObject.SetActive(true);

            for (int i = 0; i < positions.Count; i++)
            {
                if (!positions[i].GetComponentInChildren<CartMain>())
                {
                    DeleteGameObject(i);
                   
                }

                Destroy(positions[i].GetComponentInChildren<CartMain>().gameObject);
            }

            GameManager.Instance.StartCards();

        }
    }

    public void StartBonus()
    {

        controller.transform.gameObject.SetActive(false);

        for (int i = 0; i < positions.Count; i++)
        {
            Instantiate(controller.myCarts[i], positions[i].transform);
            positions[i].GetComponentInChildren<CartMain>().gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            positions[i].GetComponentInChildren<CartMain>().gameObject.AddComponent<Movement>().turn=true;
            positions[i].GetComponentInChildren<CartMain>().cribbageStart = true;
            IAController.GetComponent<Controller>().myCarts[i].GetComponent<CartMain>().cribbageStart = false;
        }
        for (int i = 0; i < disableMaz.Length; i++)
        {
            disableMaz[i].SetActive(false);
        }

        playerDisable.SetActive(false);
        positionsShoot.SetActive(true);
       

    }

    public void DeleteGameObject(int number)
    {
        positions.RemoveAt(number);
        controller.myCarts.RemoveAt(number);
        IAController.ultimateCard = true;
        for (int i = 0; i < disableMaz.Length; i++)
        {
            disableMaz[i].SetActive(true);
        }
        positionsShootCol.SetActive(true);


        playerDisable.SetActive(true);
        StartCoroutine(waitForDisable());

    }
    IEnumerator waitForDisable()
    {
        yield return new WaitForSeconds(10f);
        DisableNoUsable();

     }

    public void DisableNoUsable()
    {
        for (int i = 0; i < positionsCol.Count; i++)
        {
            if (!positionsCol[i].GetComponentInChildren<CartMain>())
            {
                positionsCol[i].SetActive(false);
            }
            else
            {
                positionsCol[i].GetComponentInChildren<SpriteRenderer>().color =new Color(0, 0, 0, 0);
                positionsCol[i].GetComponentInChildren<CartMain>().GetComponentInChildren<SpriteRenderer>().color = FinishColor;
            }
        }
        for (int i = 0; i < positionsRow.Count; i++)
        {
            if (!positionsRow[i].GetComponentInChildren<CartMain>())
            {
                positionsRow[i].SetActive(false);

            }
            else
            {
                positionsRow[i].GetComponentInChildren<SpriteRenderer>().color = new Color(0,0,0,0);

                positionsRow[i].GetComponentInChildren<CartMain>().GetComponentInChildren<SpriteRenderer>().color = FinishColor;

            }
        }
    }*/

}
