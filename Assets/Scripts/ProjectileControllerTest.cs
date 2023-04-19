using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProjectileControllerTest : MonoBehaviour {

    [SerializeField] LineRenderer _Line;
    [SerializeField] private Transform _FirePoint;
    [SerializeField] private Camera _cam;

    [SerializeField] float _InitialVelocity;
    [SerializeField] float _Angle;
    [SerializeField] float _Step;

    private void Start() {
        _cam = Camera.main;
    }

    private void Update() {
        float angle = _Angle * Mathf.Deg2Rad;
        Vector3 targetPos = _cam.ScreenToWorldPoint(Input.mousePosition) - _FirePoint.position;

        float v0;
        float time;

        CalculatePath(targetPos, angle, out v0, out time);

        DrawPath(v0, angle, _Step, time);
        if (Input.GetKeyDown(KeyCode.Space)) {
            StopAllCoroutines();
            StartCoroutine(Courutine_Movement(v0, angle));
        }
    }

    private void DrawPath(float v0, float angle, float step, float time) {
        step = Mathf.Max(0.01f, step);

        int count = 0;

        _Line.positionCount = (int)(time / step) + 1;

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
