using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHandController2D : MonoBehaviour {

    [SerializeField] private Transform limbSolver2D_Target;
    private Camera cameraMain;

    private void Awake() {
        cameraMain = Camera.main;
    }

    private void FixedUpdate() {
        Vector3 cursorPosition2D = cameraMain.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition2D.z = 0;
        limbSolver2D_Target.position = cursorPosition2D;
    }


}
