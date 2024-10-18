using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMain : MonoBehaviour
{

    /*[Header("----------- REFERENCES ------------")]
    public Controller controllerP1;
    public Controller controllerP2;

    [Header("----------- ROWS ------------")]
    public List<GameObject> row1;
    public List<GameObject> row2;
    public List<GameObject> row3;
    public List<GameObject> row4;
    public List<GameObject> row5;

    [Header("----------- COLUMS ------------")]
    public List<GameObject> colum1;
    public List<GameObject> colum2;
    public List<GameObject> colum3;
    public List<GameObject> colum4;
    public List<GameObject> colum5;

    [Header("----------- PAR INFO ROW ------------")]

    [HideInInspector]
    public List<int> score_row1;
    [HideInInspector]
    public List<int> score_row2;
    [HideInInspector]
    public List<int> score_row3;
    [HideInInspector]
    public List<int> score_row4;
    [HideInInspector]
    public List<int> score_row5;

    [Header("----------- SCORE INFO ROW ------------")]

    public List<int> id_row1;
    [HideInInspector]
    public List<int> id_row2;
    [HideInInspector]
    public List<int> id_row3;
    [HideInInspector]
    public List<int> id_row4;
    [HideInInspector]
    public List<int> id_row5;

    [Header("----------- STAIR INFO ROW ------------")]
    
    public List<int> stair_row1;
   
    public List<int> stair_row2;
   
    public List<int> stair_row3;
  
    public List<int> stair_row4;
  
    public List<int> stair_row5;


    [Header("----------- PAR INFO COLUMS ------------")]


    public List<int> score_colum1;
    
    public List<int> score_colum2;
    
    public List<int> score_colum3;
    
    public List<int> score_colum4;
    
    public List<int> score_colum5;

    [Header("----------- SCORE INFO ROW ------------")]

    [HideInInspector]

    public List<int> id_colum1;

    public List<int> id_colum2;

    public List<int> id_colum3;

    public List<int> id_colum4;

    public List<int> id_colum5;



    [Header("----------- SCORE INFO COLUMS ------------")]
    public List<int> matchCartsRows;
    public List<int> matchCartsColums;


    [Header("----------- TOTAL SCORES ------------")]
    public int scorePlayerRow;
    public int scorePlayerColum;

    int stair1;
    int stair2 = 1;
    int stair3 = 1;
    int stair4 = 1;
    int stair5 = 1;

    int stairColum1;
    int stairColum2 = 1;
    int stairColum3 = 1;
    int stairColum4 = 1;
    int stairColum5 = 1;
    private void Start()
    {
        stair1 = 1;
        stair2 = 1;
        stair3 = 1;
        stair4 = 1;
        stair5 = 1;

        stairColum1 = 1;
        stairColum2 = 1;
        stairColum3 = 1;
        stairColum4 = 1;
        stairColum5 = 1;


    }
    public void CheckCarts(int row, int colum, GameObject cart)
    {

        CheckRows(row, colum, cart);
        CheckColums(row, colum, cart);


    }


    private void CheckRows(int row, int colum, GameObject cart)
    {
        if (row == 1)
        {

            row1[colum - 1] = cart;
            score_row1[colum - 1] = cart.GetComponent<CartMain>().idCart;
            id_row1[colum - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------


            Stair(colum, row, cart.GetComponent<CartMain>(), id_row1, stair1, true);

            //Check Score -----------------------------------------
            if (id_row1[0] != 0 &&
                id_row1[1] != 0 &&
                id_row1[2] != 0 &&
                id_row1[3] != 0 &&
                id_row1[4] != 0)
            {
                Fifteen(id_row1, true);
                print("fift");
            }
            //Finish Check Score -----------------------------------------


            //Check Par -----------------------------------------------

            CheckPar(colum, row, cart, score_row1, cart.GetComponent<CartMain>(), true);

            //Finish Check Par -----------------------------------------------




        }
        if (row == 2)
        {
            row2[colum - 1] = cart;
            score_row2[colum - 1] = cart.GetComponent<CartMain>().idCart;
            id_row2[colum - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------


            Stair(colum, row, cart.GetComponent<CartMain>(), id_row2, stair2, true);
            //Check Score -----------------------------------------
            if (id_row2[0] != 0 &&
                id_row2[1] != 0 &&
                id_row2[2] != 0 &&
                id_row2[3] != 0 &&
                id_row2[4] != 0)
            {
                Fifteen(id_row2, true);
                print("fift");
            }
            //Finish Check Score -----------------------------------------

            //Check Par -----------------------------------------------

            CheckPar(colum, row, cart, score_row2, cart.GetComponent<CartMain>(), true);

            //Finish Check Par -----------------------------------------------



        }
        if (row == 3)
        {
            row3[colum - 1] = cart;
            score_row3[colum - 1] = cart.GetComponent<CartMain>().idCart;
            id_row3[colum - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------


            Stair(colum, row, cart.GetComponent<CartMain>(), id_row3, stair3, true);
            //Check Score -----------------------------------------
            if (id_row3[0] != 0 &&
                id_row3[1] != 0 &&
                id_row5[2] != 0 &&
                id_row3[3] != 0 &&
                id_row3[4] != 0)
            {
                Fifteen(id_row3, true);
                print("fift");
            }
            //Finish Check Score -----------------------------------------


            //Check Par -----------------------------------------------

            CheckPar(colum, row, cart, score_row3, cart.GetComponent<CartMain>(), true);

            //Finish Check Par -----------------------------------------------



        }
        if (row == 4)
        {
            row4[colum - 1] = cart;
            score_row4[colum - 1] = cart.GetComponent<CartMain>().idCart;
            id_row4[colum - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------


            Stair(colum, row, cart.GetComponent<CartMain>(), id_row4, stair4, true);

            //Check Score -----------------------------------------
            if (id_row4[0] != 0 &&
                id_row4[1] != 0 &&
                id_row4[2] != 0 &&
                id_row4[3] != 0 &&
                id_row4[4] != 0)
            {
                Fifteen(id_row4, true);
                print("fift");
            }
            //Finish Check Score -----------------------------------------


            //Check Par -----------------------------------------------

            CheckPar(colum, row, cart, score_row4, cart.GetComponent<CartMain>(), true);

            //Finish Check Par -----------------------------------------------



        }
        if (row == 5)
        {
            row5[colum - 1] = cart;
            score_row5[colum - 1] = cart.GetComponent<CartMain>().idCart;
            id_row5[colum - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------


            Stair(colum, row, cart.GetComponent<CartMain>(), id_row5, stair5, true);

            //Check Score -----------------------------------------
            if (id_row5[0] != 0 &&
                id_row5[1] != 0 &&
                id_row5[2] != 0 &&
                id_row5[3] != 0 &&
                id_row5[4] != 0)
            {
                Fifteen(id_row5, true);

            }
            //Finish Check Score -----------------------------------------



            //Check Par -----------------------------------------------

            CheckPar(colum, row, cart, score_row5, cart.GetComponent<CartMain>(), true);

            //Finish Check Par -----------------------------------------------




        }
    }


    private void CheckColums(int row, int colum, GameObject cart)
    {
        if (colum == 1)
        {
            colum1[row - 1] = cart;
            score_colum1[row - 1] = cart.GetComponent<CartMain>().idCart;
            id_colum1[row - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------

            Stair(row, colum, cart.GetComponent<CartMain>(), id_colum1, stairColum1, false);



            //Check PAR---------------------------------------------
            CheckPar(row, colum, cart, score_colum1, cart.GetComponent<CartMain>(), false);
            //------------------------------------------------------finish check par

        }
        if (colum == 2)
        {
            colum2[row - 1] = cart;
            score_colum2[row - 1] = cart.GetComponent<CartMain>().idCart;
            id_colum2[row - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------

            Stair(row, colum, cart.GetComponent<CartMain>(), id_colum2, stairColum2, false);

            //Check PAR---------------------------------------------
            CheckPar(row, colum, cart, score_colum2, cart.GetComponent<CartMain>(), false);

            //------------------------------------------------------finish check par

        }
        if (colum == 3)
        {
            colum3[row - 1] = cart;
            score_colum3[row - 1] = cart.GetComponent<CartMain>().idCart;
            id_colum3[row - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------

            Stair(row, colum, cart.GetComponent<CartMain>(), id_colum3, stairColum3, false);

            //Check PAR---------------------------------------------
            CheckPar(row, colum, cart, score_colum4, cart.GetComponent<CartMain>(), false);

            //------------------------------------------------------finish check par


        }
        if (colum == 4)
        {
            colum4[row - 1] = cart;
            score_colum4[row - 1] = cart.GetComponent<CartMain>().idCart;
            id_colum4[row - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------

            Stair(row, colum, cart.GetComponent<CartMain>(), id_colum4, stairColum4, false);

            //Check PAR---------------------------------------------
            CheckPar(row, colum, cart, score_colum4, cart.GetComponent<CartMain>(), false);

            //------------------------------------------------------finish check par


        }
        if (colum == 5)
        {
            colum5[row - 1] = cart;
            score_colum5[row - 1] = cart.GetComponent<CartMain>().idCart;
            id_colum5[row - 1] = cart.GetComponent<CartMain>().cartScore;

            //Check Stair -----------------------------------------

            Stair(row, colum, cart.GetComponent<CartMain>(), id_colum5, stairColum5, false);

            //Check PAR---------------------------------------------
            CheckPar(row, colum, cart, score_colum5, cart.GetComponent<CartMain>(), false);

            //------------------------------------------------------finish check par


        }
    }

    /// <summary>
    /// Luego de que se llena una lista o columna esta funcion realiza la suma de toda la lista, delimitada de izquierda a derecha.
    /// Si alguna de las sumas da 15 suma los puntos correspondientes.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="isrow"></param>
    private void Fifteen(List<int> list, bool isrow)
    {
        //Check Score -----------------------------------------

        for (int i = 1; i < list.Count; i++)
        {
            if (i >= 1)
            {
                if (list[0] + list[i] == 15)
                {
                    MoreScore(2, isrow);
                }
            }
            if (i >= 2)
            {
                if (list[1] + list[i] == 15)
                {
                    MoreScore(2, isrow);
                }
            }
            if (i >= 3)
            {
                if (list[2] + list[i] == 15)
                {
                    MoreScore(2, isrow);
                }
            }
            if (i >= 4)
            {
                if (list[3] + list[i] == 15)
                {
                    MoreScore(2, isrow);
                }
            }

        }


        //Finish Check Score -----------------------------------------
    }


    /// <summary>
    /// Logica de Par chequea si las cartas siguientes o anteriores son iguales se encuentra limitada la busqueda dependiendo 
    /// la posicion del elemento, como por ejemploel elemento 0 y el 4 son los unicos que pueden chequear si existen 4 cartas iguales
    /// ya que no lo permite la longitud del la lista en caso de ser menores. 
    /// </summary>
    /// <param name="colum"></param>
    /// <param name="row"></param>
    /// <param name="cart2"></param>
    /// <param name="scoreRow"></param>
    /// <param name="cart"></param>
    /// <param name="isrow"></param>
    private void CheckPar(int colum, int row, GameObject cart2, List<int> scoreRow, CartMain cart, bool isrow)
    {

        if (colum - 1 == 0)
        {
            if (cart.idCart == scoreRow[colum])
            {

                MoreScore(2, isrow);

                if (cart.idCart == scoreRow[colum + 1])
                {
                    MoreScore(6, isrow);

                    if (cart.idCart == scoreRow[colum + 2])
                    {
                        MoreScore(12, isrow);

                    }
                }
            }

            return;
        }
        if (colum - 1 == 4)
        {
            if (cart.idCart == scoreRow[colum - 2])
            {

                MoreScore(2, isrow);

                if (cart.idCart == scoreRow[colum - 3])
                {
                    MoreScore(6, isrow);

                    if (cart.idCart == scoreRow[colum - 4])
                    {
                        MoreScore(12, isrow);

                    }

                }
            }



            return;

        }
        if (cart.idCart == scoreRow[colum])
        {
            MoreScore(2, isrow);

            if (colum - 1 <= 1 || colum - 1 >= 3)
            {
                return;
            }

            if (cart.idCart == scoreRow[colum + 1])
            {


                MoreScore(6, isrow);

                if (colum - 1 != 0 || colum - 1 != 4)
                {
                    return;
                }

                if (cart.idCart == scoreRow[colum + 2])
                {
                    
                    MoreScore(12, isrow);

                }
            }
        }


        if (cart.idCart == scoreRow[colum - 2])
        {
            MoreScore(2, isrow);


            if (colum - 1 <= 1 || colum - 1 >= 3)
            {
                return;
            }

            if (cart.idCart == scoreRow[colum - 3])
            {
                MoreScore(6, isrow);

                if (colum - 1 != 0 || colum - 1 != 4)
                {
                    return;
                }

                if (cart.idCart == scoreRow[colum - 4])
                {
                    

                    MoreScore(12, isrow);

                }

            }
        }

       
    }

    /// <summary>
    /// Funcion encargada de la logica del sistema de puntuacion escalera 
    /// busca en la lista si el elemento anterior o el siguiente son iguales
    /// busca en la lista el elemento siguiente restandole uno tambien sumandole uno
    /// busca en la lista el elemento anterior restandole uno tambien sumandole uno.
    /// </summary>
    /// <param name="colum"></param>
    /// <param name="row"></param>
    /// <param name="cart"></param>
    /// <param name="scoreRow"></param>
    /// <param name="match"></param>
    /// <param name="isrow"></param>
    private void Stair(int colum, int row, CartMain cart, List<int> scoreRow, int match, bool isrow)
    {
        if (colum - 1 == 0)
        {
            if (cart.idCart == scoreRow[colum])
            {
                Match(row, 1, isrow);
            }
            if (cart.idCart + 1 == scoreRow[colum])
            {
                Match(row, 1, isrow);
            }
            if (cart.idCart - 1 == scoreRow[colum])
            {
                Match(row, 1, isrow);
            }
            return;
        }
        if (colum - 1 == 4)
        {
            if (cart.idCart == scoreRow[colum - 2])
            {
                Match(row, 1, isrow);
            }
            if (cart.idCart - 1 == scoreRow[colum - 2])
            {
                Match(row, 1, isrow);
            }
            if (cart.idCart + 1 == scoreRow[colum - 2])
            {
                Match(row, 1, isrow);
            }

            return;

        }
        if (cart.idCart == scoreRow[colum])
        {
            Match(row, 1, isrow);
        }
        if (cart.idCart + 1 == scoreRow[colum])
        {
            Match(row, 1, isrow);
        }
        if (cart.idCart + 1 == scoreRow[colum - 2])
        {
            Match(row, 1, isrow);
        }
        if (cart.idCart - 1 == scoreRow[colum])
        {
            Match(row, 1, isrow);
        }
        if (cart.idCart - 1 == scoreRow[colum - 2])
        {
            Match(row, 1, isrow);
        }
        if (cart.idCart == scoreRow[colum - 2])
        {
            Match(row, 1, isrow);
        }
    }


    /// <summary>
    /// Match suma los scores de las escaleras a partir de 3 cartas
    /// </summary>
    /// <param name="row"></param>
    /// <param name="points"></param>
    /// <param name="isrow"></param>
    private void Match(int row, int points, bool isrow)
    {
        row -= 1;

        if (row == 0)
        {
            stair1 += points;
            if (stair1 == 3)
            {
                MoreScore(3, isrow);
            }
            if (stair1 == 4)
            {
                MoreScore(4, isrow);
            }
            if (stair1 == 5)
            {
                MoreScore(5, isrow);
            }
        }
        if (row == 1)
        {
            stair2 += points;
            if (stair2 == 3)
            {
                MoreScore(3, isrow);
            }
            if (stair2 == 4)
            {
                MoreScore(4, isrow);
            }
            if (stair2 == 5)
            {
                MoreScore(5, isrow);
            }
        }
        if (row == 2)
        {
            stair3 += points;
            if (stair3 == 3)
            {
                MoreScore(3, isrow);
            }
            if (stair3== 4)
            {
                MoreScore(4, isrow);
            }
            if (stair3 == 5)
            {
                MoreScore(5, isrow);
            }
        }
        if (row == 3)
        {
            stair4 += points;
            if (stair4 == 3)
            {
                MoreScore(3, isrow);
            }
            if (stair4 == 4)
            {
                MoreScore(4, isrow);
            }
            if (stair4 == 5)
            {
                MoreScore(5, isrow);
            }
        }
        if (row == 4)
        {
            stair5 += points;
            if (stair5 == 3)
            {
                MoreScore(3, isrow);
            }
            if (stair5 == 4)
            {
                MoreScore(4, isrow);
            }
            if (stair5 == 5)
            {
                MoreScore(5, isrow);
            }
        }

    }

    /// <summary>
    /// Suma de puntos para el jugador de las filas
    /// </summary>
    /// <param name="score">Punto agregar</param>
    private void MoreScore(int score, bool isrow)
    {
        if (isrow)
        {
            scorePlayerRow += score;

            if (controllerP1.rowPlayer)
            {
                controllerP1.SetScoreRow(score);
            }
            if (controllerP2.rowPlayer)
            {
                controllerP2.SetScoreRow(score);
            }
           
            return;
        }
        if (!isrow)
        {
            scorePlayerColum += score;
            if (controllerP1.columPlayer)
            {
                controllerP1.SetScoreColum(score);
            }
            if (controllerP2.columPlayer)
            {
                controllerP2.SetScoreColum(score);
            }
        }
        
    }


  


    */



}
