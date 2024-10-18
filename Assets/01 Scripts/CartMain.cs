using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartMain : MonoBehaviour
{
    [Header("Points")]
    public int cartScore;

    [Header("Cart Info")]
    public int idCart;
    public string idInStringCart;
    public string typeCart;
    public string nameCart;

    [Header("Cribagge")]
    public Color colorSelect;
    public bool cardCribbage;
    public bool cribbageStart;

    public ShootCart shootCart;
    bool select;
    bool drag;
   // Color baseColor;
    // Start is called before the first frame update
    void Start()
    {

        nameCart = idInStringCart + typeCart.ToLower();
       // baseColor = GetComponentInChildren<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   

    





}
