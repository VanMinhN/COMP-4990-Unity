using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisiblewall : MonoBehaviour
{


    void WallInvisible()
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = false;

    }


}
