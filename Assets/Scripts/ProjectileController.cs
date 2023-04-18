using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [SerializeField] private Transform firePoint;

    private const float initSpeed = 10f;
    private float initAngle = 45f;

    private float projectileLifeTime = 5;
    private float currentLifeTime;

    private Vector2 startPos;

    private void Awake() {
        currentLifeTime = 0;
        initAngle *= Mathf.Deg2Rad;

        MovementHandler(initSpeed, initAngle);

        Physics.gravity.Set(1f,1f,1f);

        startPos = transform.position;

        Debug.Log(startPos);
    }


    private void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            currentLifeTime = 0;
        }

        MovementHandler(initSpeed, initAngle);
    }

    private void MovementHandler(float initSpeed, float initAngle) {

        if (currentLifeTime < projectileLifeTime) {
            float x = initSpeed * currentLifeTime * Mathf.Cos(initAngle);
            float y = initSpeed * currentLifeTime * Mathf.Sin(initAngle)
                + Physics.gravity.y * Mathf.Pow(currentLifeTime, 2) / 2;

            transform.position = firePoint.position + new Vector3(x, y, 0); 

            currentLifeTime += Time.deltaTime;
        }
    }
}

// A - угол запуска снар€да
// x - рассто€ние пройденное по горизонтали, y - по вертикали,
// Vx - горизонтальна€ скорость, Vy - вертикальна€, t - прошедшее врем€, h - начальна€ высота
// g - ускорение свободного падени€
// ‘ормула снижени€ снар€да g * t^2 / 2
// ”скорение свободного падени€ g = Physics.gravity.y
// x = Vx * t
// y = h + (Vy * t) - (g * t^2) / 2
// 
// ¬ тетрадке полные рассчЄты с графиком
//

// x = v0 * t * cosA
// y = v0 * t * sinA - (g * t) / 2
// 