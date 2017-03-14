using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamCtrl : MonoBehaviour {
    public float moveSpeed     =  10f;
    public float rotationSpeed =  10f;
    public float zoomOutSpeed  = -50f;
    public float shiftMult     =   2f;


	void Update () {
        bool shiftDown = Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift);
        float mult = shiftDown ? shiftMult : 1f;
        mult *= Time.deltaTime * 10f;

        if (Input.GetKey(KeyCode.Mouse2))
        {
            Vector3 rot = gameObject.transform.rotation.eulerAngles;

            if (Input.GetAxis("Vertical") > 0)
                rot.x = rot.x - Time.deltaTime * mult * rotationSpeed;
            else if (Input.GetAxis("Vertical") < 0)
                rot.x = rot.x - Time.deltaTime * mult * -rotationSpeed;

            if (Input.GetAxis("Horizontal") > 0)
                rot.y *= 1f * Time.deltaTime * mult * rotationSpeed;
            else if (Input.GetAxis("Horizontal") < 0)
                rot.y *= 1f * Time.deltaTime * mult * -rotationSpeed;

            gameObject.transform.rotation = Quaternion.Euler(rot);
        }

        else
        {
            // Move around.
            Vector3 position = gameObject.transform.position;
            position.x += Input.GetAxis("Horizontal") * mult * moveSpeed;
            position.y += Input.GetAxis("Mouse ScrollWheel") * zoomOutSpeed * mult;
            position.z += Input.GetAxis( "Vertical" ) * mult * moveSpeed;
            gameObject.transform.position = position;
        }
    }
}
