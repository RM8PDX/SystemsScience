using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Make spheres travel along inside their box at a fixed speed.
/// </summary>
public class RollAround : MonoBehaviour {
    public float speed = 10f;
    protected Rigidbody rb;


	void Start () {
        rb = GetComponent<Rigidbody>();
        FixSpeed();
	}

    /// <summary>
    /// Ensure that the object in question is maintaining a fixed speed.
    /// </summary>
    protected void FixSpeed() {
        rb.velocity = rb.velocity.normalized * speed;
        // We choose a random number to call this again so that every
        // object is calling this function on the exact same frame.
        Invoke("FixSpeed", Random.Range(0.1f, 0.4f));
    }
}
