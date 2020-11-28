using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P2malSp : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform[] p2Sp;

    private void Awake()
    {
        p2Sp = new Transform[transform.childCount];
        for (int i = 0; i < p2Sp.Length; i++)
        {
            p2Sp[i] = transform.GetChild(i);
        }
    }
}
