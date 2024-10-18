using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    [Header("Carts Player")]
    public GameObject blankCart;
    public GameObject cartdown;
    public List<GameObject> myCarts;
    public List<GameObject> cribbageCarts;

    [Header("References")]
    public ShootCart shootCartPos;
    public GameObject playerCart;
    public Count count;
    public Text t_score;
    public int score;
    public GameObject scoreAnim;
    public TextMeshPro scoreTMP;


    public Color colorPlayer;
    public bool rowPlayer;
    public bool cribbagePlayer;
    public bool columPlayer;
    public bool redPlayer;
    public bool cutPlayer;
    [Header("Controller IA?")]
    public bool isIa;
    public Text t_scoreIa;
    public string nameAi;

    // Start is called before the first frame update
    void Start()
    {

        scoreAnim.SetActive(false);
        /* if (redPlayer)
         {
             scoreTMP.text = PegsScoreManager.RedPegsScore.ToString();
         }
         else
         {
             scoreTMP.text = PegsScoreManager.BluePegsScore.ToString();

         }*/
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.startGame && !isIa && myCarts.Count <= 1)
        {
            GameManager.Instance.DisableAllCards(true);


        }
        else if (GameManager.Instance.startGame && isIa && myCarts.Count < 1)
        {
            GameManager.Instance.DisableAllCards(false);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PositionShoot"))
        {
            shootCartPos = collision.GetComponent<ShootCart>();
        }
    }


    public void InstanceCart()
    {
        if (isIa)
        {
            cartdown = Instantiate(blankCart, this.transform);


            return;
        }

        if (myCarts.Count > 0)
        {

            GameObject go = Instantiate(myCarts[0], this.transform);
            go.GetComponentInChildren<SpriteRenderer>().sortingOrder = 50;

        }

        else
        {
            Instantiate(blankCart, this.transform);
        }

    }

    public void InstanceBlank(bool destroy)
    {
        if (destroy)
        {
            Destroy(playerCart);
            return;
        }
        playerCart = Instantiate(blankCart, this.transform);
    }




    public void ShootCart()
    {
        Destroy(GetComponentInChildren<CartMain>().gameObject);
        GetComponent<Movement>().ReturnPositionInitial();
        GameManager.Instance.startGame = true;
        if (myCarts.Count < 1 && !isIa)
        {
            Cribbage.Instance.DisableAll();
        }
        myCarts.RemoveAt(0);
        print("Shoot ");
        if (myCarts.Count <= 0)
        {
            if (isIa)
            {
                if (GameManager.Instance.player1.GetComponent<Controller>().myCarts.Count > 0)
                {
                    Debug.Log("to shoot cribAGGE cARD");
                    GameManager.Instance.TurnController(true, false);
                }
            }
            return;

        }
        InstanceCart();

    }
    public void CribbagePoint(int scr)
    {
        if (cutPlayer) return;
        score += scr;
    }
    public void RecivePoints(int row, int colum)
    {
        if (rowPlayer)
        {
            SetScoreRow(row);
        }
        else
        {
            SetScoreColum(colum);
        }


    }

    public void CutGame()
    {
        if (cutPlayer && isIa)
        {
            /* CentralCard.Instance.CentralCart();
             GeneralMaz.Instance.finishMaz();
             GameManager.Instance.StartCards();
             Invoke(nameof(IAdisableCutButton), Random.Range(1.5f, 3f));*/
        }
    }
    void IAdisableCutButton()
    {
        //GameManager.Instance.cutButton.gameObject.SetActive(false);
    }
    public void SetScoreRow(int _score, bool isToShowScoreAnimation = true)
    {
        if (rowPlayer)
        {
            score = _score;
            if (isToShowScoreAnimation)
                WatchPoints();
            if (redPlayer)
            {
                count.SetRedPegScore(score);
                count.MoveRedPin();
            }
            else
            {
                count.SetBluePegScore(score);
                count.MoveBluePin();

            }
        }
        return;

    }
    public void SetScoreColum(int _score, bool isToShowScoreAnimation = true)
    {
        if (columPlayer)
        {
            score = _score;
            if (isToShowScoreAnimation)
                WatchPoints();
            if (redPlayer)
            {
                count.SetRedPegScore(score);
                count.MoveRedPin();
            }
            else
            {
                count.SetBluePegScore(score);
                count.MoveBluePin();

            }
        }
    }

    public void ShowCribbageScore(int score)
    {
        if (cribbagePlayer)
        {
            if (redPlayer)
            {
                scoreTMP.text = score.ToString();
                count.SetRedPegScore(score);
                count.MoveRedPin();
            }
            else
            {
                count.SetBluePegScore(score);
                count.MoveBluePin();

            }
            scoreTMP.text = score.ToString();
        }
        else
        {
            scoreTMP.text = "0";
        }
    }
    public void WatchPoints()
    {
        scoreAnim.SetActive(true);
        scoreAnim.GetComponentInChildren<TextMeshPro>().text = "+ " + score.ToString();
        //  scoreTMP.text = score.ToString();
        Destroy(scoreAnim, 20f);
        // StartCoroutine(RestartScene());
    }

    public IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
