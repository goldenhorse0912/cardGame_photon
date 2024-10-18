using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public string playerColor;
    public static Counter Instance;
    public List<GameObject> positions;
 

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            positions.Add(transform.GetChild(i).gameObject);
        }
    }

    public void PaintScore(int score)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            positions[i].SetActive(false);
            if (i == score)
            {
                positions[i-1].SetActive(true);
            }
        }
    }



}
