using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    [SerializeField] private Camera gameCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        float windowaspect = 1080f / 1920f;

        gameCamera.aspect = windowaspect;
    }
}
