using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2D : MonoBehaviour {

    [SerializeField] private Transform playerTransform;

    private Vector3 cameraOffset;

    private void Awake() {
        cameraOffset = transform.position;
    }

    private void Update() {
        transform.position = playerTransform.position + cameraOffset;
    }



}
