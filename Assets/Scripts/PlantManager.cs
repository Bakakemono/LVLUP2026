using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void LightPlants(bool sunLight) {
        Transform star = sunLight ? _sunTransform : _moonTransform;

        foreach(var plant in _plantedPlants) {
            //Debug.DrawRay(star.position, plant.transform.position - star.position, Color.red, 1f);

            RaycastHit2D[] hits =
                Physics2D.RaycastAll(
                    star.position,
                    plant.transform.position - star.position,
                    (plant.transform.position - star.position).magnitude,
                    _hitLayerMask
                    );

            if(hits.Length > 0) {
                bool hitObstacle = false;
                foreach(var hit in hits) {
                    int layerConvertedValue = 1 << hit.transform.gameObject.layer;
                    if(layerConvertedValue == _stopAllLayerMask.value ||
                        (sunLight && layerConvertedValue == _stopLightLayerMask) ||
                        (!sunLight && layerConvertedValue == _stopDarklayerMask)) {
                        hitObstacle = true;
                        Debug.Log("Block");
                        break;
                    }
                }

                if(hitObstacle)
                    return;

                if(sunLight)
                    plant.AddLightPoint();
                else
                    plant.AddDarknessPoint();
            }
        }
    }
    public void RegisterNewPlant(Plant plant) {
        _plantedPlants.Add(plant);
    }

    public void DeregisterPlant(Plant plant) {
        _plantedPlants.Remove(plant);
    }

    public void RevealPerfectPlant() {
        foreach(var plant in _plantedPlants){
            plant.UpgradeToPerfect();
        }
    }
}
