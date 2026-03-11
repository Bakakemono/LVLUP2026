using System;
using UnityEngine;
using UnityEngine.UI;

public class CycleManager : MonoBehaviour {
    public static CycleManager _instance;

    public enum RayType {
        NONE,
        LIGHT,
        DARKNESS
    }

    public enum Periode : int {
        MORNING,
        AFTERNOON,
        EVENING
    }

    [SerializeField] Transform _sunMoonPivotTransform;
    bool _nightCycle = false;

    [SerializeField] Button _startDayButton;
    [SerializeField] Button _startNightButton;

    [SerializeField] Transform _sun;
    [SerializeField] Transform _moon;

    [SerializeField] float _arcModificationFactor;

    [SerializeField] Transform _cycleStartTransform;

    bool _cycleBegin = false;

    float _cycleTotalDuration = 9f;

    [SerializeField] int[] _pointsPeriodes;

    int _pastPeriodeCount = 0;
    float _cycleStartTime;
    int _hitNumber = 0;
    float _nextPing = 0f;

    // Values for celestial Object movement.
    Vector2 _celestialPivotPoint;
    Vector2 _celestialEndPosition;
    float _circleRadius;
    float _startingAngle;
    public float _totalArcAngle;

    bool _isCycleFinish = false;
    
    [Header("Debug")]
    [SerializeField] bool DEBUG_displayGizmos = false;
    [SerializeField, Range(0, 100f)]float DEBUG_cycleProgression = 0f;


    private void Awake() {
        if(_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        CalculateCelestialArc();
    }

    private void FixedUpdate() {
        if(!_cycleBegin)
            return;                                                                         

        PlayVisual();
        CycleUpdate();
    }

    public void BeginCycle(bool nightCycle) {
        _nightCycle = nightCycle;
        _isCycleFinish = false;
        _cycleBegin = true;

        _cycleStartTime = Time.time;

        // Calculate first threshold . 
        float onePeriodeLength = _cycleTotalDuration * ((float)_pastPeriodeCount + 1f) / _pointsPeriodes.Length;

        _nextPing =
            _cycleStartTime + onePeriodeLength * _pastPeriodeCount +
                ((onePeriodeLength / (_pointsPeriodes[_pastPeriodeCount] + 1) * (_hitNumber + 1)));

        GameManager._instance._cycleInProgress = true;
        GameManager._instance._starMoving = true;

        if(!_nightCycle)
            _startDayButton.gameObject.SetActive(false);
        else
            _startNightButton.gameObject.SetActive(false);

        FindFirstObjectByType<PlantMenuManager>().HideMenu();
    }

    void CycleUpdate() {
        if(!_cycleBegin)
            return;

        if(Time.time > _cycleStartTime + _cycleTotalDuration) {
            GameManager._instance.CycleFinished();

            if(!_nightCycle)
                _startNightButton.gameObject.SetActive(true);
            else
                _startDayButton.gameObject.SetActive(true);

            _cycleBegin = false;

            return;
        }

        if(_isCycleFinish)
            return;

        if(Time.time > _nextPing) {
            _hitNumber++;

            GameManager._instance.PingAstreEffect(_nightCycle ? RayType.DARKNESS : RayType.LIGHT, (Periode)_pastPeriodeCount);
            if(_hitNumber == _pointsPeriodes[_pastPeriodeCount]) {

                _hitNumber = 0;
                _pastPeriodeCount++;

                if(_pastPeriodeCount == _pointsPeriodes.Length) {
                    _pastPeriodeCount = 0;
                    _isCycleFinish = true;
                }
            }

            // Set Cycle Time
            float onePeriodeLength = _cycleTotalDuration / _pointsPeriodes.Length;

            _nextPing =
                _cycleStartTime + onePeriodeLength * _pastPeriodeCount +
                    (onePeriodeLength / (_pointsPeriodes[_pastPeriodeCount] + 1) * (_hitNumber + 1));
        }
    }


    void PlayVisual() {
        Transform _celestialBody = _nightCycle ? _moon : _sun;
        float progression = (Time.time - _cycleStartTime) / _cycleTotalDuration;

        progression = Mathf.Clamp(progression, 0f, 1f);

        float angle = Mathf.Deg2Rad * (_startingAngle + (_totalArcAngle * progression));

        Vector2 position = _celestialPivotPoint + new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle)) * _circleRadius;

        _celestialBody.position = position;
    }


    void CalculateCelestialArc() {
        // Calculate the end position at the opposite.
        _celestialEndPosition = new Vector2(-_cycleStartTransform.position.x, _cycleStartTransform.position.y);

        // The pivote point around wich the celestial body will turn.
        _celestialPivotPoint = Vector2.zero + Vector2.up * (_cycleStartTransform.position.y - _arcModificationFactor);

        // Radius of said circle.
        _circleRadius = ((Vector2)_cycleStartTransform.position - _celestialPivotPoint).magnitude;

        _startingAngle = Vector2.Angle(Vector2.left, ((Vector2)_cycleStartTransform.position - _celestialPivotPoint).normalized);

        _totalArcAngle = Vector2.Angle(((Vector2)_cycleStartTransform.position - _celestialPivotPoint).normalized, (_celestialEndPosition - _celestialPivotPoint).normalized);
    }

    void StartingNewDay() {
        Debug.Log("New Day starting");
    }

    // Only as a fail safe.
    private void OnValidate() {
        // Snap the value to zero if set below zero.
        _arcModificationFactor = Mathf.Max(0f, _arcModificationFactor);
    }

    private void OnDrawGizmos() {
        if(!DEBUG_displayGizmos)
            return;

        CalculateCelestialArc();

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_cycleStartTransform.position, 0.5f);

        Gizmos.color = Color.blueViolet;
        Gizmos.DrawSphere(new Vector2(-_cycleStartTransform.position.x, _cycleStartTransform.position.y), 0.5f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_celestialPivotPoint, _cycleStartTransform.position);
        Gizmos.DrawLine(_celestialPivotPoint, _celestialEndPosition);

        Gizmos.color = Color.red;
        Vector2 pos = _cycleStartTransform.position;
        for(int i = 1; i <= 10; i++) {
            float angle = Mathf.Deg2Rad * (_startingAngle + (_totalArcAngle * (float)i / 10f));

            Vector2 newPos = _celestialPivotPoint + new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle)) * _circleRadius;

            Gizmos.DrawLine(pos, newPos);
            pos = newPos;
        }

        {
            float angle = Mathf.Deg2Rad * (_startingAngle + (_totalArcAngle * DEBUG_cycleProgression / 100f));
            Vector2 sunPos = _celestialPivotPoint + new Vector2(-Mathf.Cos(angle), Mathf.Sin(angle)) * _circleRadius;

            Gizmos.color = new Color((100f - DEBUG_cycleProgression) / 100f, (100f - DEBUG_cycleProgression) / 100f, 0f, 1f);
            _sun.position = sunPos;
        }
    }   
}