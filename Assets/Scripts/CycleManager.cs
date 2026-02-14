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

    private void FixedUpdate() {
        if(_dayCycle)
            DayCycle();

        if(_nightCycle)
            NightCycle();
    }

    public void StartSunCycle() {
        _startDayButton.gameObject.SetActive(false);
        _dayCycle = true;
    }

    public void StartMoonCycle() {
        _startNightButton.gameObject.SetActive(false);
        _nightCycle = true;
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
                return;
            }
            _lastPing += -180f / (_pingPerDay + 1);
            GameManager._instance.PingAstreEffect(false);
        }
    }
}