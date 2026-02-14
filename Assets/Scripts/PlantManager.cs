using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour {
    public static PlantManager _instance;

    [SerializeField] List<Plant> _plantedPlants;
    Transform _sunTransform {
        get {
            if(sunTransform == null)
                sunTransform = FindFirstObjectByType<Sun>().transform;

            return sunTransform;
        }
    }
    Transform sunTransform;

    Transform _moonTransform {
        get {
            if(moonTransform == null)
                moonTransform = FindFirstObjectByType<Moon>().transform;

            return moonTransform;
        }
    }
    Transform moonTransform;

    [SerializeField] LayerMask _hitLayerMask;
    [SerializeField] LayerMask _plantLayerMask;
    [SerializeField] LayerMask _stopAllLayerMask;
    [SerializeField] LayerMask _stopLightLayerMask;
    [SerializeField] LayerMask _stopDarklayerMask;

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LightPlants(bool _sunLight) {
        Transform star = _sunLight ? _sunTransform : _moonTransform;

        foreach(var plant in _plantedPlants) {
            Debug.DrawRay(star.position, plant.transform.position - star.position, Color.red, 1f);

            RaycastHit2D[] hits =
                Physics2D.RaycastAll(
                    _sunTransform.position,
                    plant.transform.position - star.position,
                    (plant.transform.position - star.position).magnitude,
                    _hitLayerMask
                    );

            if(hits.Length > 0) {
                bool hitObstacle = false;
                foreach(var hit in hits) {
                    int layerConvertedValue = 1 << hit.transform.gameObject.layer;
                    if(layerConvertedValue == _stopAllLayerMask.value ||
                        (_sunLight && layerConvertedValue == _stopLightLayerMask) ||
                        (!_sunLight && layerConvertedValue == _stopDarklayerMask)) {
                        hitObstacle = true;
                        break;
                    }
                }

                if(!hitObstacle)
                    plant.AddLightPoint();
            }
        }
    }
    public void RegisterNewPlant(Plant plant) {
        _plantedPlants.Add(plant);
    }
}
