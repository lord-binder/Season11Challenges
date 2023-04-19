using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAimController : MonoBehaviour {

    private Camera cameraMain;

    private void Start() {
        cameraMain = Camera.main;
    }

    private void FixedUpdate() {
        Vector3 mouseWorldPosition = cameraMain.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }


}
