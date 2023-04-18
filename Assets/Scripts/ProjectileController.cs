using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    private const float initSpeed = 10f;
    private float initAngle = 45f;

    private float projectileLifeTime = 10;
    private float currentLifeTime;

    private void Start() {
        currentLifeTime = 0;
        initAngle *= Mathf.Deg2Rad;

        ProjectileMovementHandler(initSpeed, initAngle);

        Physics.gravity.Set(1f,1f,1f);
    }


    private void Update() {
        ProjectileMovementHandler(initSpeed, initAngle);

    }

    private void ProjectileMovementHandler(float initSpeed, float initAngle) {
        if (currentLifeTime < projectileLifeTime) {
            float x = initSpeed * currentLifeTime * Mathf.Cos(initAngle);
            float y = initSpeed * currentLifeTime * Mathf.Sin(initAngle) 
                - -Physics.gravity.y * Mathf.Pow(currentLifeTime, 2) / 2;

            transform.position = new Vector3(x, y, 0); 

            currentLifeTime += Time.deltaTime;
        }
    }

}

// A - ���� ������� �������
// x = Vx * t, ��� x - ���������� ���������� �� �����������,
// Vx - �������������� ��������, t - �����
// y = h + (Vy * t) - (g * t^2) / 2, ��� y - ���������� ������������ ����������, h - ��������� ������
// � ������ ������ �������������� ��������� �����������, � ������������ ����� g

// x = v0 * t * cosA
// y = v0 * t * sinA - (g * t) / 2


//���� ������ ������ y ���������� �������� y = ax^2 + bx + c, �� ��� �����