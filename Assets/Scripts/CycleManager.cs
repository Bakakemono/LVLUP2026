using System;
using UnityEngine;
using UnityEngine.UI;

public class CycleManager : MonoBehaviour {
    [SerializeField] Transform _sunMoonPivotTransform;
    [SerializeField, Range(1f, 360f)]
    float _degPerSecond = 60f;
    float _lastPing = 0f;
    int _pingPerDay = 10;

    float _currentRotation = 0f;

    bool _dayCycle = false;
    bool _nightCycle = false;

    [SerializeField] Button _startDayButton;
    [SerializeField] Button _startNightButton;

    [SerializeField] Transform _sun;
    [SerializeField] Transform _moon;

    [SerializeField] float _arcModificationFactor;

    [SerializeField] Transform _cycleStartTransform;
    [SerializeField] Transform _cycleEndTransform;

    float _dayDuration = 10f;

    int[] _pointsPeriodes;

    float _cycleStartTime;

    public float _totalArcAngle;

    private void FixedUpdate() {
        if(_dayCycle)
            DayCycle();

        if(_nightCycle)
            NightCycle();
    }

    public void StartSunCycle() {
        GameManager._instance._cycleInProgress = true;
        GameManager._instance._starMoving = true;

        _startDayButton.gameObject.SetActive(false);
        _dayCycle = true;

        FindFirstObjectByType<PlantMenuManager>().HideMenu();
    }

    public void StartMoonCycle() {
        GameManager._instance._starMoving = true;

        _startNightButton.gameObject.SetActive(false);
        _nightCycle = true;
        FindFirstObjectByType<PlantMenuManager>().HideMenu();
    }

    void DayCycle() {
        _currentRotation += -_degPerSecond * Time.fixedDeltaTime;
        _sunMoonPivotTransform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);

        if(_currentRotation <= _lastPing + -180f / (_pingPerDay + 1)) {
            if((_lastPing + -180f / (float)(_pingPerDay + 1)) <= -180f) {
                _dayCycle = false;
                _startNightButton.gameObject.SetActive(true);
                _lastPing = -180f;
                _currentRotation = -180f;

                GameManager._instance._starMoving = false;
                return;
            }
            _lastPing += -180f / (_pingPerDay + 1);
            GameManager._instance.PingAstreEffect(true);
        }
    }

    void NightCycle() {
        _currentRotation += -_degPerSecond * Time.fixedDeltaTime;
        _sunMoonPivotTransform.rotation = Quaternion.Euler(0f, 0f, _currentRotation);

        if(_currentRotation <= _lastPing + -180f / (_pingPerDay + 1)) {
            if((_lastPing + -180f / (float)(_pingPerDay + 1)) <= -360f) {
                _nightCycle = false;
                _startDayButton.gameObject.SetActive(true);
                _lastPing = 0f;
                _currentRotation = 0f;

                GameManager._instance._cycleInProgress = false;
                GameManager._instance._starMoving = false;

                PlantManager._instance.RevealPerfectPlant();
                return;
            }
            _lastPing += -180f / (_pingPerDay + 1);
            GameManager._instance.PingAstreEffect(false);
        }
    }

    private void OnValidate() {
        // Snap the value to zero if set below zero.
        _arcModificationFactor = Mathf.Max(0f, _arcModificationFactor);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cycleStartTransform.position, 1f);
        Gizmos.color = Color.blueViolet;
        Gizmos.DrawSphere(_cycleEndTransform.position, 1f);


        Vector2 pivotPoint = (Vector2)(_cycleStartTransform.position + _cycleEndTransform.position) / 2f - Vector2.up * _arcModificationFactor;
        float distanceFromPivot = ((Vector2)_cycleStartTransform.position - pivotPoint).magnitude;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(pivotPoint, _cycleStartTransform.position);
        Gizmos.DrawLine(pivotPoint, _cycleEndTransform.position);

        float angleOffset = Vector2.Angle(Vector2.left, ((Vector2)_cycleStartTransform.position - pivotPoint).normalized);
        _totalArcAngle = Vector2.Angle(((Vector2)_cycleStartTransform.position - pivotPoint).normalized, ((Vector2)_cycleEndTransform.position - pivotPoint).normalized);

        Gizmos.color = Color.red;
        Vector2 pos = _cycleEndTransform.position;
        for(int i = 1; i <= 10; i++) {
            float angle = Mathf.Deg2Rad * (angleOffset + (_totalArcAngle * (float)i / 10f));

            Vector2 newPos = pivotPoint + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distanceFromPivot;

            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }
    }
}