using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCart : MonoBehaviour
{

    [Header("Carts All")]
    public List<GameObject> carts;

    [Header("References")]
    public GameObject centralCart;
    public GameObject mazCart;
    public ShootCart centralPosition;

    private int _random;
    private void Start()
    {
        SelectCentralCart();
    }


    /// <summary>
    /// Mediantes numeros random reparte las cartas a los jugadores 1 y 2 sean o no IA.
    /// </summary>
    /// <param name="player1">Gameobject con controller que representa al player 1</param>
    /// <param name="player2">Gameobject con controller que representa al player 2</param>
    /// <param name="maxCartPerPlayer">El Manager envia la cantidad maxima de cartas por jugador</param>
    public void Mixer(Controller player1, Controller player2, int maxCartPerPlayer)
    {
        int random;

        for (int i = 0; i < carts.Count; i++)
        {

            //----------------PLAYER 1------------------------//
            if (player1.myCarts.Count < maxCartPerPlayer)
            {
                random = Random.Range(0, carts.Count - 1);
                player1.myCarts.Add(carts[random]);
                carts.RemoveAt(random);
            }
            //----------------PLAYER 2------------------------//
            if (player2.myCarts.Count < maxCartPerPlayer)
            {
                random = Random.Range(0, carts.Count - 1);
                player2.myCarts.Add(carts[random]);
                carts.RemoveAt(random);
            }


        }


    }

    /// <summary>
    /// Selecciona una carta al azar de todo el mazo para colocarla en el centro.
    /// </summary>
    private void SelectCentralCart()
    {
        _random = Random.Range(0, carts.Count - 1);
        centralCart = carts[_random];
        carts.RemoveAt(_random);
    }

    /// <summary>
    /// Da comienzo a la corrutina que elimina carta y animacion luego de lllegar al centro.
    /// </summary>
    public void CentralCart()
    {
        StartCoroutine("EnuCentralCart");
    }
    /// <summary>
    /// Destruye carta central back para dejar en ese lugar a la carta real.
    /// </summary>
    /// <returns>Segundos</returns>
    IEnumerator EnuCentralCart()
    {
        yield return new WaitForSeconds(.5f);
        //Destroy(mazCart);
        centralPosition.AddCartCentral(centralCart);
        GameManager.Instance.CheckCenteralCardForJack();
    }




}
