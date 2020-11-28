using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1malSp : MonoBehaviour
{
    public Transform[] p1Sp;

    private void Awake()
    {
        p1Sp = new Transform[transform.childCount];
        for(int i = 0; i < p1Sp.Length; i++)
        {
            p1Sp[i] = transform.GetChild(i);
        }
    }
}
