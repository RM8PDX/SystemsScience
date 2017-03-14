using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulateDrift : MonoBehaviour {
    public float driftFreq  = 10f;
    public float recalcFreq = 60f;
    public float driftMagnitude = 1f;
    protected SimInstantiator simCtrl;
    protected Rigidbody rb;


    public void Start() {
        simCtrl = FindObjectOfType<SimInstantiator>();
        rb = GetComponent<Rigidbody>();
    }


}
