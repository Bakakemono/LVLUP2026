using UnityEngine;

public class CycleManager : MonoBehaviour {
    Transform _sunTransform;
    [SerializeField, Range(1f, 360f)]
    float _degPerSecond = 60f;
    float _lastSunPing = 0f;
    int _pingPerDay = 10;
    bool _sunDown = false;

    bool _clockWise = true;

    bool _startCycle = false;

    

    private void FixedUpdate() {
        if(_sunDown || !_startCycle)
            return;

        //transform.Rotate(0f, 0f, -15f * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (_clockWise ? -1f : 1f) * _degPerSecond * Time.fixedDeltaTime);

        if(Mathf.Floor(_clockWise ? 360f - transform.rotation.eulerAngles.z : transform.rotation.eulerAngles.z) >= _lastSunPing + 180f / (_pingPerDay + 1)) {
            if((_lastSunPing + 180f / (float)(_pingPerDay + 1)) >= 180) {
                Debug.Log("Stop The Sun");
                _sunDown = true;
                return;
            }
            _lastSunPing += 180f / (_pingPerDay + 1);
            GameManager._instance.PingSunray(true);
        }
    }

    public void StartSunCycle() {
        _startCycle = true;
    }

    public void StartMoonCycle() {

    }

    void SunCycle() {

    }

    void MoonCycle() {

    }
}