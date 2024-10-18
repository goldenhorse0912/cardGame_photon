using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralCard : MonoBehaviour
{
    public static CentralCard Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CentralCart()
    {
        GameObject.FindObjectOfType<RandomCart>().CentralCart();
    }
}
