using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    public static CardLibrary Instance;
    public List<GameObject> Cards;

    void Awake() {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int Search(string name) {

        for (int i = 0; i < Cards.Count; i++)
        {
            if (name == Cards[i].name) {

                return i;
            }

        }

        return 99;
    }

    public GameObject SearchGO(string name)
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            if (name == Cards[i].name)
            {

                return Cards[i];
            }

        }

        return null;
    }
}
