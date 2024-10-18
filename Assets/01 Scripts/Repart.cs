using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repart : MonoBehaviour
{

    public List<GameObject> cartsRed;
    public List<GameObject> cartsBlue;

    public GameObject targetRed;
    public GameObject targetBlue;

    public float time;
    public float speed;

    public bool repartStart;
    public bool active;
    private void Update()
    {
        if (repartStart && active)
        {
            InitRepart();
        }
    }

    public void StartRepart(bool b)
    {
        repartStart = b;
        active = true;
    }

    public void InitRepart()
    {

       if(cartsRed.Count==0 && cartsBlue.Count == 0)
        {
            return;
        }

        if (cartsRed.Count > 0)
        {
            cartsRed[0].transform.position = Vector3.Lerp(cartsRed[0].transform.position, targetRed.transform.position, speed);
           
        }
        if (cartsBlue.Count > 0)
        {

            cartsBlue[0].transform.position = Vector3.Lerp(cartsBlue[0].transform.position, targetBlue.transform.position, speed);
           

        }

        if (cartsBlue[0].transform.position == targetBlue.transform.position || cartsRed[0].transform.position == targetRed.transform.position)
        {
            active = false;
            StartCoroutine(wait());
        }



    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(time);
        cartsRed.RemoveAt(0);
        cartsBlue.RemoveAt(0);
        active = true;

    }
}
