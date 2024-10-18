using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnim : MonoBehaviour
{
    // Start is called before the first frame update
    public void FinishAndDestroy()
    {
        Destroy(this.gameObject, 1.5f);
    }
}
