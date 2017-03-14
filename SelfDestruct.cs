using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour {
    public float time;

	void Start () {
        Invoke("Die", time);
	}

    private void Die() {
        Destroy(this.gameObject);
    }
}
