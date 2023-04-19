using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

    [SerializeField] private Transform firePoint;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float lineStep;

    private float initSpeed = 10f;
    private float initAngle = 45f;
    private float projectileLifeTime = 5f;

    private Camera cameraMain;

    private Vector3 startPos;

    private float currentLifeTime;
    private bool isOnFlight;

    private void Awake() {
        startPos = firePoint.position;
        Physics.gravity.Set(1f,1f,1f);
    }

    private void Start() {
        cameraMain = Camera.main;
    }

    private void FixedUpdate() {

        float initAngleRad = initAngle * Mathf.Deg2Rad;
        Vector3 targetPos = cameraMain.ScreenToWorldPoint(Input.mousePosition) - startPos;

        float v0;
        float time;

        CalculatePath(targetPos, initAngleRad, out v0, out time);

        DrawPath(v0, initAngleRad, lineStep, time);
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            currentLifeTime = 0;
            isOnFlight = true;
        }

        if (isOnFlight) {
            MovementHandler(v0, initAngleRad, time);
        }

    }

    public void CalculatePath(Vector3 targetPos, float initAngle, out float v0, out float time) {
        float x = targetPos.x;
        float y = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = x * x * g;
        float v2 = x * Mathf.Tan(initAngle) - y;
        float v3 = 2 * Mathf.Pow(Mathf.Cos(initAngle), 2);

        v0 = Mathf.Sqrt(v1 / (v2 * v3));
        time = x / (v0 * Mathf.Cos(initAngle));
    }

    private void DrawPath(float initSpeed, float angleRad, float lineStep, float time) {
        int count = 0;

        lineStep = Mathf.Max(0.01f, lineStep);
        lineRenderer.positionCount = (int)(time / lineStep) + 2;
        
        for(float predictTime = 0; predictTime < time; predictTime+= lineStep) {

            lineRenderer.SetPosition(count, startPos + CalculatePosition(initSpeed, angleRad, predictTime));

            count++;
        }

        lineRenderer.SetPosition(count, startPos + CalculatePosition(initSpeed, angleRad, time));
    }

    private float QuadraticEquation(float a, float b, float c, float sigh) {
        return (-b + sigh * Mathf.Sqrt(b * b - 4 * a * c) / (2 * a));
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time) {
        float x = targetPos.x;
        float y = targetPos.y;
        float g = -Physics.gravity.y;

        float a = (-0.5f * g);
        float b = Mathf.Sqrt(2 * g * h);
        float c = -y;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(-a, b, c, -1);

        time = tplus > tmin ? tplus : tmin;
        angle = Mathf.Atan(b * time / x);
        v0 = b / Mathf.Sin(angle);
    }

    private void MovementHandler(float initSpeed, float angleRad, float lifeTime) {
        if (currentLifeTime < lifeTime) {
            transform.position = startPos + CalculatePosition(initSpeed, angleRad, currentLifeTime);
            currentLifeTime += Time.fixedDeltaTime;
        }
    }
    
    public Vector3 CalculatePosition(float initSpeed, float angleRad, float time) {
        float x = initSpeed * time * Mathf.Cos(angleRad);
        float y = initSpeed * time * Mathf.Sin(angleRad)
            + Physics.gravity.y * Mathf.Pow(time, 2) / 2;

        Debug.Log(initSpeed);

        return new Vector3(x, y, 0);
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

// ≈сли дойду до реализации отскока, можно активно чекать коллизию по текущему вектору движени€ с помощью
// Physics2D(ƒл€ текущего прототипа Physics2D).RayCast - одиночным лучом, SphereCast тоже позволит такое сделать, но нужно будет отпредел€ть в каком именно месте сфера получила удар и там смотреть вектор