using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BackgroundController : MonoBehaviour {

    private float length, startPos;
    private Camera cameraMain;
    [SerializeField] private float parallaxEffect;

    private void Start() {
        cameraMain = Camera.main;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(startPos);
    }

    private void FixedUpdate() {
        float distance = cameraMain.transform.position.x * parallaxEffect;
        float distanceDelta = cameraMain.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (distanceDelta > startPos + length) startPos += length;
        else if (distanceDelta < startPos - length) startPos -= length;

    }

}
