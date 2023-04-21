using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileControllerTest : MonoBehaviour {

    [SerializeField] LineRenderer _Line;
    [SerializeField] private Transform _FirePoint;
    [SerializeField] private Camera _cam;

    [SerializeField] private float _InitialVelocity;
    [SerializeField] private float _Angle;
    [SerializeField] private float _Step;
    [SerializeField] private float _Height;

    private void Start() {
        _cam = Camera.main;
    }

    private void Update() {
        
        Vector3 targetPos = _cam.ScreenToWorldPoint(Input.mousePosition) - _FirePoint.position;

        float angle;
        float v0;
        float time;

        CalculatePathWithHeight(targetPos, _Height, out v0, out angle, out time);

        DrawPath(v0, angle, _Step, time);
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            StartCoroutine(Courutine_Movement(v0, angle));
        }
    }

    private float QuadraticEquation(float a, float b, float c, float sign) {
        return (-b + sign * Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
    }

    private void CalculatePathWithHeight(Vector3 targetPos, float h, out float v0, out float angle, out float time) {
        float x = targetPos.x;
        float y = targetPos.y;
        float g = -Physics.gravity.y;

        float a = (-0.5f * g);
        float b = Mathf.Sqrt(2 * g * h);
        float c = -y;

        float tplus = QuadraticEquation(a, b, c, 1);
        float tmin = QuadraticEquation(a, b, c, -1);

        time = tplus > tmin ? tplus : tmin;
        angle = Mathf.Atan(b * time / x);
        v0 = b / Mathf.Sin(angle);
    }

    private void DrawPath(float v0, float angle, float step, float time) {
        step = Mathf.Max(0.01f, step);

        int count = 0;

        _Line.positionCount = (int)(time / step) + 2;

        for (float i =0; i < time; i += step) { 
            float x = v0 * i * Mathf.Cos(angle);
            float y = v0 * i * Mathf.Sin(angle)
                - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            _Line.SetPosition(count, _FirePoint.position + new Vector3(x, y, 0));
            count++;
        }

        float xt = v0 * time * Mathf.Cos(angle);
        float yt = v0 * time * Mathf.Sin(angle)
            - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        _Line.SetPosition(count, new Vector3(xt, yt, 0));
    }

    public void CalculatePath(Vector3 targetPos, float angle, out float v0, out float time) {
        float xt = targetPos.x;
        float yt = targetPos.y;
        float g = -Physics.gravity.y;

        float v1 = Mathf.Pow(xt, 2) * g;
        float v2 = 2 * xt * Mathf.Sin(angle) * Mathf.Cos(angle);
        float v3 = 2 * yt * Mathf.Pow(Mathf.Cos(angle), 2);
        v0 = Mathf.Sqrt(v1 / (v2 - v3));

        time = xt / (v0 * Mathf.Cos(angle));
    }

    IEnumerator Courutine_Movement(float v0, float angle) {
        float t = 0;
        while (t < 100) {
            float x = v0 * t * Mathf.Cos(angle);
            float y = v0 * t * Mathf.Sin(angle)
                - 0.5f * - Physics.gravity.y * Mathf.Pow(t, 2);
            transform.position = _FirePoint.position + new Vector3(x, y, 0);
            t += Time.deltaTime;
            yield return null;
        }
    }



}
