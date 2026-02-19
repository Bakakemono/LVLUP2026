using System.Linq;
using UnityEngine;

public class Plant : MonoBehaviour {
    public enum PlantStates {
        BABY,
        WELL,
        PERFECT,
        DEAD
    }

    public enum PlantTypes {
        LUX,
        NOX,
        MOTH_LUX,
        MOTH_NOX,
        MOTH
    }

    PlantTypes _planteType = PlantTypes.LUX;
    PlantStates _plantState = PlantStates.BABY;

    [SerializeField] bool _mothPlant = false;

    [Header("Plant States")]
    [SerializeField] GameObject _babyForm;
    [SerializeField] GameObject _wellForm;
    [SerializeField] GameObject _greatForm;
    [SerializeField] GameObject _deadForm;
    [SerializeField] GameObject _wellFormAlternative;
    [SerializeField] GameObject _greatFormAlternative;

    public Transform _transform;
    Transform _sunTransform;

    [Header("Light Value Params")]
    [SerializeField, Range(0, 10)] int _minLightNecessary = 1;
    [SerializeField, Range(0, 10)] int _maxLightNecessary = 10;
    [SerializeField, Range(0, 10)] int _perfectLightValue = 5;
    [SerializeField] int _lightAborbed = 0;

    [Header("Darkness Value Params")]
    [SerializeField, Range(0, 10)] int _minDarknessNecessary = 1;
    [SerializeField, Range(0, 10)] int _maxDarknessNecessary = 10;
    [SerializeField, Range(0, 10)] int _perfectDarknessValue = 5;
    [SerializeField] int _darknessAbsorbed = 0;


    public Spot _occupiedSpot;

    private void Start() {
        _transform = transform;
        _sunTransform = FindFirstObjectByType<Sun>().transform;
    }

    public void AddLightPoint() {
        Debug.Log("Add Light Point");
        _lightAborbed++;
        UpdateState();
    }

    public void AddDarknessPoint() {
        Debug.Log("Add Darkness Point");
        _darknessAbsorbed++;
        UpdateState();
    }

    public void UpdateState() {
        if(_plantState == PlantStates.DEAD)
            return;

        if(_mothPlant) {
            MothSpecialUpdate();
            return;
        }

        if(_lightAborbed >= _minLightNecessary && _lightAborbed <= _maxLightNecessary && _darknessAbsorbed >= _minDarknessNecessary && _darknessAbsorbed <= _maxDarknessNecessary) {
            if(_plantState != PlantStates.DEAD) {
                _plantState = PlantStates.WELL;
                DisableAllStates();
                _wellForm.SetActive(true);
                if(_lightAborbed == _perfectLightValue && _darknessAbsorbed == _perfectDarknessValue) {
                    _plantState = PlantStates.PERFECT;
                }
            }
        }
        else if (_lightAborbed > _maxLightNecessary || _darknessAbsorbed > _maxDarknessNecessary) {
            _plantState = PlantStates.DEAD;
            DisableAllStates();
            _deadForm.SetActive(true);
        }
    }

    public void MothSpecialUpdate() {
        if(_lightAborbed > _darknessAbsorbed) {
            if(_lightAborbed > _maxLightNecessary) {
                _plantState = PlantStates.DEAD;
                DisableAllStates();
                _deadForm.SetActive(true);
                return;
            }
            else if(_lightAborbed == 6 && _darknessAbsorbed == 3) {
                _plantState = PlantStates.PERFECT;
            }
            else {
                _plantState = PlantStates.WELL;
                DisableAllStates();
                _wellForm.SetActive(true);
            }
        }
        else if(_lightAborbed < _darknessAbsorbed) {
            if(_darknessAbsorbed > _maxDarknessNecessary) {
                _plantState = PlantStates.DEAD;
                DisableAllStates();
                _deadForm.SetActive(true);
                return;
            }
            else if(_lightAborbed == 3 && _darknessAbsorbed == 6) {
                _plantState = PlantStates.PERFECT;
            }
            else {
                _plantState = PlantStates.WELL;
                DisableAllStates();
                _wellFormAlternative.SetActive(true);
            }
        }
    }

    public void UpgradeToPerfect() {
        if(_plantState == PlantStates.PERFECT) {
            DisableAllStates();
            if(_mothPlant) {
                if(_planteType == PlantTypes.MOTH_NOX) {
                    _greatFormAlternative.SetActive(true);
                    return;
                }
            }
            _greatForm.SetActive(true);
        }
    }

    void DisableAllStates() {
        _babyForm.SetActive(false);
        _wellForm.SetActive(false);
        _greatForm.SetActive(false);
        _deadForm.SetActive(false);
        _wellFormAlternative?.SetActive(false);
        _greatFormAlternative?.SetActive(false);
    }

    public void RegisterPoints() {
        if(_plantState == PlantStates.DEAD || _plantState == PlantStates.BABY)
            return;

        switch(_planteType) {
            case PlantTypes.LUX:
                if(_plantState == PlantStates.WELL)
                    GameManager._instance._luxPoints++;
                else if(_plantState == PlantStates.PERFECT)
                    GameManager._instance._luxPerfectPoints++;
                break;

            case PlantTypes.NOX:
                if(_plantState == PlantStates.WELL)
                    GameManager._instance._noxPoints++;
                else if(_plantState == PlantStates.PERFECT)
                    GameManager._instance._noxPerrfectPoints++;
                break;

            case PlantTypes.MOTH_LUX:
                if(_plantState == PlantStates.WELL)
                    GameManager._instance._mothLuxPoints++;
                else if(_plantState == PlantStates.PERFECT)
                    GameManager._instance._mothLuxPerfectPoints++;
                break;

            case PlantTypes.MOTH_NOX:
                if(_plantState == PlantStates.WELL)
                    GameManager._instance._mothNoxPoints++;
                else if(_plantState == PlantStates.PERFECT)
                    GameManager._instance._mothNoxPerfectPoints++;
                break;
        }
    }
}