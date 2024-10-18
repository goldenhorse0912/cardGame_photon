using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{


    private Vector3 startPos;
    public Controller _controller;
    public bool turn;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;

        if (GetComponent<Controller>())
        {
            _controller = GetComponent<Controller>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDrag()
    {
        Drag();
    }
    private void OnMouseUp()
    {
        Drop();
    }



    public void Drag()
    {
        if (!turn)
        {
            return;
        }
        Vector2 screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        this.transform.position = worldPosition;
       

    }

    public void Drop()
    {
        if (!turn)
        {
            return;
        }
        ReturnPositionInitial();
       


    }

    public void ReturnPositionInitial()
    {

        this.transform.position = startPos;

    }


}
