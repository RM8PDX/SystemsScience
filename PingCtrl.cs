using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingCtrl : MonoBehaviour {
    public Vector3 ptA;
    public Vector3 ptB;
    public float speed;
    public float maxChangeDist;

    protected Vector3 curTgt;
    protected Vector3 notTgt;
    protected Rigidbody rb;
    protected bool disableSwap = false;


    public void Start() {
        rb = GetComponent<Rigidbody>();
        curTgt = ptB;
        notTgt = ptA;
    }


    /// <summary>
    /// We need some way to prevent Swap from being called a hundred times in a row.
    /// </summary>
    public void EnableSwap() {
        disableSwap = false;
    }


    public void SwapTgt() {
        if (disableSwap)
            return;

        Vector3 temp = curTgt;
        curTgt = notTgt;
        notTgt = temp;

        disableSwap = true;
        Invoke("EnableSwap", 0.25f);
    }

    public void Update() {
        Vector3 pos = gameObject.transform.position;
        float distance = Vector3.Distance(pos, curTgt);
        if (distance <= maxChangeDist)
            SwapTgt();
        Vector3 toTgt = curTgt - pos;
        rb.MovePosition(pos + toTgt.normalized * speed);
    }
}
