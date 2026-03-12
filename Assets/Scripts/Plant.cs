using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

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

    public SpotsGroup _spotGroup;

    VisualEffect _absorbtionEffect;

    bool _recolted = false;

    private void Start() {
        _transform = transform;
        _sunTransform = FindFirstObjectByType<Sun>().transform;

        _absorbtionEffect = GetComponentInChildren<VisualEffect>();
    }

    public void AddEnergy(CycleManager.RayType rayType) {
        if(_recolted)
            return;

        switch(rayType) {
            case CycleManager.RayType.LIGHT:
                _lightAborbed++;
                break;
            case CycleManager.RayType.DARKNESS:
                _darknessAbsorbed++;
                break;
        }

        if(_plantState == PlantStates.DEAD)
            return;

        PlayEffect(rayType);
        UpdateState();
    }

    public void UpdateState() {
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

        if(!_mothPlant)
            return;

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

    void PlayEffect(CycleManager.RayType rayType) {
        _absorbtionEffect.SetVector4(
            "ParticleColor",
            rayType == CycleManager.RayType.LIGHT ? GameManager._instance._lightColorEffect : GameManager._instance._darkColorEffect);

        _absorbtionEffect.Play();
    }

    public void Recolt() {
        if(_recolted)
            return;

        _recolted = true;

        _spotGroup.PlantSet(false);
        _occupiedSpot.ReleaseSpot();
        RegisterPoints();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut() {
        bool fadeOutFinished = false;
        float fadeOutTime = 0.5f;
        float fadeOutScaleIncrease = 0.3f;
        float startingTime = Time.time;
        
        SpriteRenderer activePlanteRenderer = GetActiveForm();

        float currentHeight = activePlanteRenderer.transform.localPosition.y;

        while(!fadeOutFinished) {
            if(Time.time >= startingTime + fadeOutTime) {
                fadeOutFinished = true;
                break;
            }
            activePlanteRenderer.transform.localScale =
                Vector3.one * (1 + (fadeOutScaleIncrease * (Time.time - startingTime) / fadeOutTime));

            activePlanteRenderer.color =
                new Color(1f, 1f, 1f, 1f - (Time.time - startingTime) / fadeOutTime);

            activePlanteRenderer.transform.localPosition =
                new Vector2(activePlanteRenderer.transform.localPosition.x, currentHeight + 0.5f * (Time.time - startingTime) / fadeOutTime);

            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    SpriteRenderer GetActiveForm() {
        switch(_plantState) {
            case PlantStates.BABY:
                return _babyForm.GetComponent<SpriteRenderer>();
            case PlantStates.WELL:
                if(_planteType == PlantTypes.MOTH_NOX)
                    return _wellFormAlternative.GetComponent<SpriteRenderer>();
                else
                    return _wellForm.GetComponent<SpriteRenderer>();
            case PlantStates.PERFECT:
                if(_planteType == PlantTypes.MOTH_NOX)
                    return _greatFormAlternative.GetComponent<SpriteRenderer>();
                else
                    return _greatForm.GetComponent<SpriteRenderer>();
            case PlantStates.DEAD:
                return _deadForm.GetComponent<SpriteRenderer>();
        }
        return null;
    }
}