using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHotKeys : MonoBehaviour {
    public float sensitivity = 5f;

	void Update () {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F8))
            Time.timeScale = 100;
        else if (Input.GetKeyDown(KeyCode.F8))
            Time.timeScale += sensitivity;
        else if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F7))
            Time.timeScale = 0f;
        else if (Input.GetKeyDown(KeyCode.F7))
            Time.timeScale = Mathf.Max(0f, Time.timeScale - sensitivity);
	}
}
