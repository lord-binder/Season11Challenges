using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour {

    private Rigidbody2D playerRb;

    private float speed = 10f;

    private void Awake() {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");
        
        playerRb.velocity = new Vector2(xMove, playerRb.velocity.y) * speed;
    }

}
