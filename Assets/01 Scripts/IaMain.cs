using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IaMain : MonoBehaviour
{

    public ShootCart[] shootPos; // Posiciones para lanzar carta

    [HideInInspector]

    public Controller _controller;
    public bool turn;
    [Header("REFERENCES")]
    public GameManager gameManager;
    private Vector3 startPost;
    private bool b_wait;
    public CartMain actualCart;
    [Header("----------------- CRIBAGGE HAND -----------------")]

    public List<GameObject> cardsCribbage;
    public GameObject[] posCribbage;

    [Header("----------------- COLUM LIST -----------------")]
    public List<ShootCart> colum1;
    public List<ShootCart> colum2;
    public List<ShootCart> colum3;
    public List<ShootCart> colum4;
    public List<ShootCart> colum5;
    [Header("----------------- ROW LIST -----------------")]
    public List<ShootCart> row1;
    public List<ShootCart> row2;
    public List<ShootCart> row3;
    public List<ShootCart> row4;
    public List<ShootCart> row5;

    public bool stop;
    public Vector3 posShoot;
    public float speed;
    public bool shoot;
    bool cribbage;
    bool finish = false;
    int randomPos;
    bool shootCribbage;

    public SpriteRenderer placeholderBackCardRenderer;
    void Start()
    {
        startPost = this.transform.position; //posicion  inicial
        GetComponentInChildren<SpriteRenderer>().sortingOrder = 80;
        _controller = GetComponent<Controller>();


    }

    // Update is called once per frame
    void Update()
    {
        if (shoot)
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, posShoot, step);

            if (Vector3.Distance(transform.position, posShoot) < 0.001f)
            {
                //transform.position = startPost;
                transform.position = posShoot;
                shoot = false;

            }

            //this.transform.position = new Vector3(posShoot.x, posShoot.y, posShoot.z);
            // this.transform.Translate(posShoot * Time.deltaTime);
        }
        if (shoot && shootCribbage)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, posShoot, step);
            transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(0.32f, 0.32f, this.transform.localScale.z), Time.deltaTime * 20f);

            if (Vector3.Distance(transform.position, posShoot) < 0.001f)
            {
                //transform.position = startPost;
                transform.position = posShoot;

                shoot = false;

            }
        }

    }


    public void AiTurn()
    {
        if (turn && !stop)
        {
            transform.position = startPost;
            StartCoroutine(waittingShoot());
            b_wait = true;
        }
    }

    /// <summary>
    /// Accion de lanzar una carta a cualquier posicion.
    /// </summary>
    public void Shoot()
    {

        finish = false;

        if (b_wait) // Si esta dentro de la corrutina no se ejecuta esta funcion.
        {
            //print("sefu");
            return;
        }
        else
        {


            if (_controller.myCarts.Count <= 1)
            {
               // GetComponentInChildren<SpriteRenderer>().enabled = false;
                GameObject[] blankCards = GameObject.FindGameObjectsWithTag("DestroyCribbage");

                foreach (GameObject a in blankCards)
                {
                    a.SetActive(false);
                }
            }

            if (!cribbage)
            {
                CribbageShoot();

                return;
            }
            if (_controller.rowPlayer)
            {
                ParCheckRow();
                print("IA ROW");

            }

            if (_controller.columPlayer)
            {
                ParCheckColum();
                print("IA COLUM");
            }

        }





    }

    private void ParCheckRow()
    {
        if (!finish)
        {
            MainPar(row1);

            if (!finish)
            {
                MainPar(row2);
                if (!finish)
                {
                    MainPar(row3);
                    if (!finish)
                    {
                        MainPar(row4);
                        if (!finish)
                        {
                            MainPar(row5);

                            if (!finish)
                            {
                                StairCheckRow();

                            }
                        }
                    }
                }
            }

        }
    }
    private void ParCheckColum()
    {
        if (!finish)
        {
            MainPar(colum1);

            if (!finish)
            {
                MainPar(colum2);
                if (!finish)
                {
                    MainPar(colum3);
                    if (!finish)
                    {
                        MainPar(colum4);
                        if (!finish)
                        {
                            MainPar(colum5);

                            if (!finish)
                            {
                                StairCheckCol();

                            }
                        }
                    }
                }
            }

        }
    }

    private void StairCheckRow()
    {
        if (!finish)
        {
            MainStair(row1);

            if (!finish)
            {
                MainStair(row2);
                if (!finish)
                {
                    MainStair(row3);
                    if (!finish)
                    {
                        MainStair(row4);
                        if (!finish)
                        {
                            MainStair(row5);

                            if (!finish)
                            {
                                RandomShoot();

                            }
                        }
                    }
                }
            }
        }
    }


    private void StairCheckCol()
    {
        if (!finish)
        {
            MainStair(colum1);

            if (!finish)
            {
                MainStair(colum2);
                if (!finish)
                {
                    MainStair(colum3);
                    if (!finish)
                    {
                        MainStair(colum4);
                        if (!finish)
                        {
                            MainStair(colum5);

                            if (!finish)
                            {
                                RandomShoot();

                            }
                        }
                    }
                }
            }
        }
    }



    private void MainPar(List<ShootCart> rowList)
    {

        for (int i = 0; i < rowList.Count; i++)
        {
            if (rowList[i].GetComponentInChildren<CartMain>() == null)
            {

                finish = false;

                continue;
            }


            if (_controller.myCarts[0].GetComponent<CartMain>().idCart == rowList[i].GetComponentInChildren<CartMain>().idCart)
            {
                if (i == 0)
                {
                    if (!rowList[i + 1].occuped)
                    {



                        StartCoroutine(shooter(rowList, i + 1));
                        //rowList[i + 1].AddCartIA(_controller.myCarts[0]);
                        posShoot = rowList[i + 1].transform.position;

                        //this.transform.position = rowList[i + 1].transform.position;
                        // gameManager.TurnController(false, true);

                        shoot = true;
                        StartCoroutine(wait(rowList[i + 1].cribbage));
                        finish = true;
                        return;

                    }
                    else
                    {


                        finish = false;
                        return;

                    }


                }
                if (i == 4)
                {
                    if (!rowList[i - 1].occuped)
                    {


                        //rowList[i - 1].AddCartIA(_controller.myCarts[0]);
                        StartCoroutine(shooter(rowList, i - 1));

                        posShoot = rowList[i - 1].transform.position;

                        //this.transform.position = rowList[i - 1].transform.position;
                        //gameManager.TurnController(false, true);

                        shoot = true;
                        StartCoroutine(wait(rowList[i - 1].cribbage));
                        finish = true;
                        return;

                    }
                    else
                    {


                        finish = false;
                        return;

                    }
                }
                if (i > 0 && i < 4)
                {
                    if (!rowList[i + 1].occuped)
                    {


                        //rowList[i + 1].AddCartIA(_controller.myCarts[0]);
                        //this.transform.position = rowList[i + 1].transform.position;
                        StartCoroutine(shooter(rowList, i + 1));

                        posShoot = rowList[i + 1].transform.position;

                        //gameManager.TurnController(false, true);

                        shoot = true;
                        StartCoroutine(wait(rowList[i + 1].cribbage));
                        finish = true;
                        return;

                    }
                    if (!rowList[i - 1].occuped)
                    {
                        //print("Se enctroaron cartas simialres");

                        //rowList[i - 1].AddCartIA(_controller.myCarts[0]);
                        StartCoroutine(shooter(rowList, i - 1));

                        posShoot = rowList[i - 1].transform.position;

                        //this.transform.position = rowList[i - 1].transform.position;
                        //gameManager.TurnController(false, true);
                        shoot = true;
                        StartCoroutine(wait(rowList[i - 1].cribbage));
                        finish = true;
                        return;

                    }
                    else
                    {

                        //gameManager.TurnController(false, true);
                        finish = false;
                        return;

                    }
                }



            }

        }




    }


    IEnumerator shooter(List<ShootCart> rowpos, int pos)
    {
        yield return new WaitForSeconds(1f);
        rowpos[pos].AddCartIA(_controller.myCarts[0]);
        _controller.myCarts.RemoveAt(0);



    }
    IEnumerator shooterRandom(ShootCart[] rowpos, int pos)
    {
        yield return new WaitForSeconds(1f);
        rowpos[pos].AddCartIA(_controller.myCarts[0]);
        _controller.myCarts.RemoveAt(0);


    }


    private void MainStair(List<ShootCart> rowList)
    {


        for (int i = 0; i < rowList.Count; i++)
        {


            if (i == 0)
            {

                if (rowList[i + 1].GetComponentInChildren<CartMain>() != null)
                {
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart + 1 == rowList[i + 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {


                            //rowList[i].AddCartIA(_controller.myCarts[0]);
                            StartCoroutine(shooter(rowList, i));
                            posShoot = rowList[i].transform.position;


                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart - 1 == rowList[i + 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //rowList[i].AddCartIA(_controller.myCarts[0]);
                            posShoot = rowList[i].transform.position;




                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                }


            }
            if (i == 4)
            {

                if (rowList[i - 1].GetComponentInChildren<CartMain>() != null)
                {
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart + 1 == rowList[i - 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //rowList[i].AddCartIA(_controller.myCarts[0]);
                            posShoot = rowList[i].transform.position;




                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart - 1 == rowList[i - 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //ist[i - 1].AddCartIA(_controller.myCarts[0]);
                            posShoot = rowList[i].transform.position;




                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                }


            }
            if (i > 0 && i < 4)
            {

                if (rowList[i + 1].GetComponentInChildren<CartMain>() != null)
                {
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart + 1 == rowList[i + 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {
                            print("Esta carta es una mas que la anterior");

                            // rowList[i].AddCartIA(_controller.myCarts[0]);
                            // this.transform.position = rowList[i].transform.position;

                            StartCoroutine(shooter(rowList, i));
                            posShoot = rowList[i].transform.position;
                            //gameManager.TurnController(false, true);
                            shoot = true;
                            //_controller.myCarts.RemoveAt(0);
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart - 1 == rowList[i + 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //rowList[i].AddCartIA(_controller.myCarts[0]);
                            posShoot = rowList[i].transform.position;




                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                }

                if (rowList[i - 1].GetComponentInChildren<CartMain>() != null)
                {
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart + 1 == rowList[i - 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //rowList[i].AddCartIA(_controller.myCarts[0]);

                            posShoot = rowList[i].transform.position;




                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }


                    }
                    if (_controller.myCarts[0].GetComponent<CartMain>().idCart - 1 == rowList[i - 1].GetComponentInChildren<CartMain>().idCart)
                    {
                        if (!rowList[i].occuped)
                        {

                            StartCoroutine(shooter(rowList, i));
                            //rowList[i].AddCartIA(_controller.myCarts[0]);

                            posShoot = rowList[i].transform.position;





                            shoot = true;
                            StartCoroutine(wait(rowList[i].cribbage));
                            finish = true;
                            return;

                        }

                    }
                }


            }
            else
            {


                finish = false;


            }
        }
    }

    /// <summary>
    /// Dispara aleatoriamente una carta en cualquier posicion
    /// </summary>
    /// 
    public int GetRandomShootPosition()
    {
        int index = 0;

        List<int> unoccupiedPosition = new List<int>();
        for (int ind = 0; ind < shootPos.Length; ind++)
        {
            if (!shootPos[ind].occuped)
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
            return 0;
        }


    }

    private void RandomShoot()
    {

        int randomPos = GetRandomShootPosition();//Random.Range(0, shootPos.Length - 1);
        Debug.Log("Position " + randomPos);
        if (shootPos[randomPos].occuped)
        {
            RandomShoot();
            return;
        }
        else
        {

            StartCoroutine(shooterRandom(shootPos, randomPos));

            //shootPos[randomPos].AddCartIA(_controller.myCarts[0]);
            posShoot = shootPos[randomPos].transform.position;

            //this.gameObject.transform.position = shootPos[randomPos].transform.position;
            finish = true;

            shoot = true;

            StartCoroutine(wait(shootPos[randomPos].cribbage));


        }


    }
    private void CribbageShoot()
    {

        if (posCribbage[0] == null && posCribbage[1] == null)
        {
            cribbage = true;

            Shoot();

            return;
        }

        if (posCribbage[1] != null)
        {
            randomPos = 1;
        }
        else if (posCribbage[0] != null)
        {
            randomPos = 0;
        }

        posCribbage[randomPos].GetComponent<ShootCart>().AddCartIA(_controller.myCarts[0]);
        StartCoroutine(wait(posCribbage[randomPos].GetComponent<ShootCart>().cribbage));
        GameObject obj = _controller.myCarts[0];
        cardsCribbage.Add(obj);
        _controller.myCarts.RemoveAt(0);

        posShoot = posCribbage[randomPos].transform.position;

        shootCribbage = true;

        finish = true;

        shoot = true;

        posCribbage[randomPos] = null;
    }


    /// <summary>
    /// Espera para devolver carta a su posicion Inicial.
    /// </summary>
    /// <returns></returns>
    IEnumerator wait(bool _cribbage)
    {
        yield return new WaitForSeconds(1f);
        if (_controller.myCarts.Count <= 0)
        {
            // this.gameObject.SetActive(false);

            if (!_cribbage)
            {
                gameManager.TurnController(false, true);
            }
            else
            {
                gameManager.TurnController(true, false);
            }

            shoot = false;
        }
        transform.position = startPost;
        if (!_cribbage)
        {
            gameManager.TurnController(false, true);
        }
        else
        {
            gameManager.TurnController(true, false);
        }

        shootCribbage = false;
        this.transform.localScale = new Vector3(1, 1, 1);
        shoot = false;
        if (_controller.myCarts.Count <= 0)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }


    }
    /// <summary>
    /// Espera random para lanzar carta.
    /// </summary>
    /// <returns></returns>
    IEnumerator waittingShoot()
    {

        yield return new WaitForSeconds(1f);
        b_wait = false;
        if (_controller.myCarts.Count > 0)
        {
            Shoot();
        }
        else if (GameManager.Instance.player1.GetComponent<Controller>().myCarts.Count > 0)
        {
            gameManager.TurnController(true, false);
        }

    }

}
