using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCart : MonoBehaviour
{
    [Header("References")]
    public GameObject playerCollider;
    //public ScoreMain scoreMain;
    public GameManager gameManager;
    public GameObject redCard;
    public GameObject blueCard;
    public Count count;
    public int rowNumber;
    public int columNumber;
    public bool cribbage;
    [Header("Color Feedback")]
    public Color colorMouseUp;

    public bool active;
    public bool occuped;

    // nuevas
    [Header("CARDS NEW ")]
    public Vector3 rowOrColum;
    public Color overColor;
    Color startColor;
    string cardOver;
    CardLibrary cards;
    public int playerNumber;


    public int idCard;
    public int valueCard;
    public string pal;
    public string cardName;
    public bool cribbagge;


    Vector3 scaleCardInit;
    Vector3 scale = new Vector3(0.18f, 0.168f, 1);


    private void Start()
    {
        startColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (playerCollider == null)
            {

                return;
            }
            AddCart();
        }



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (cribbagge && playerNumber != 1)
            {
                return;
            }
            active = true;
            playerCollider = collision.gameObject;

            if (this.active)
            {
                GetComponentInChildren<SpriteRenderer>().color = colorMouseUp;
            }
            if (cribbage)
            {
                collision.transform.localScale = this.transform.localScale;
            }


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        active = false;
        playerCollider = null;
        if (collision.CompareTag("Player"))
        {
            GetComponentInChildren<SpriteRenderer>().color = startColor;

        }
        if (cribbage)
        {
            collision.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    public void AddCart()
    {
        Debug.Log("Adding to Crtibbage");

        if (!active || occuped)
        {
            Debug.Log("1111");
            return;
        }
        if (cribbage)
        {
            Debug.Log("2222");

            gameManager.cribbageCards.Add(playerCollider.GetComponent<Controller>().myCarts[0]);
            Instantiate(playerCollider.GetComponent<Controller>().myCarts[0], this.transform);
            AddProperty(playerCollider.GetComponent<Controller>().myCarts[0].GetComponent<CartMain>());
        }
        else
        {
            Debug.Log("3333");

            GameObject cartTemporal = Instantiate(playerCollider.GetComponent<Controller>().myCarts[0], this.transform);
            AddProperty(playerCollider.GetComponent<Controller>().myCarts[0].GetComponent<CartMain>());
        }

        playerCollider.GetComponent<Controller>().ShootCart();
        //Destroy(playerCollider.transform.GetChild(0).gameObject);
        //scoreMain.CheckCarts(rowNumber,columNumber, cartTemporal);
        occuped = true;

        if (!cribbage)
        {
            Debug.Log("Player 1");
            gameManager.TurnController(true, false);
        }
        else
        {
            Debug.Log("Player 2");
            gameManager.TurnController(false, true);
        }
    }
    public void AddCartFinisTime(GameObject playerCollider)
    {
        if (occuped)
        {
            return;
        }

        GameObject cartTemporal = Instantiate(playerCollider.GetComponent<Controller>().myCarts[0], this.transform);
        playerCollider.GetComponent<Controller>().ShootCart();
        //Destroy(playerCollider.transform.GetChild(0).gameObject);
        //scoreMain.CheckCarts(rowNumber,columNumber, cartTemporal);
        occuped = true;
        gameManager.TurnController(true, false);
    }


    public void AddCartIA(GameObject go)
    {
        if (occuped)
        {
            print("Lugar Ocupado Error");
            GetComponentInChildren<SpriteRenderer>().color = startColor;
            return;
        }
        if (cribbage)
        {
            gameManager.cribbageCards.Add(go);
            Instantiate(blueCard, this.transform);

        }
        else
        {
            Instantiate(go, this.transform);
        }

        //scoreMain.CheckCarts(rowNumber, columNumber, go);
        occuped = true;
        AddProperty(go.GetComponent<CartMain>());

    }




    public void AddCartCentral(GameObject go)
    {
        Instantiate(go, this.transform);
        //scoreMain.CheckCarts(rowNumber, columNumber, go);
        occuped = true;
        AddProperty(go.GetComponent<CartMain>());

    }

    void AddProperty(CartMain cm)
    {
        pal = cm.typeCart;
        valueCard = cm.cartScore;
        idCard = cm.idCart;
    }

}
