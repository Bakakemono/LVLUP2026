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

    public void LightPlants(CycleManager.RayType rayType, CycleManager.Periode _periode) {
        foreach(var plant in _plantedPlants) {
            Spot.ProtectionType protectionType = Spot.ProtectionType.NONE;
            switch(_periode) {
                case CycleManager.Periode.MORNING:
                    protectionType = plant._spotGroup.GetProtectionType(Spot.SpotType.SIDE, Spot.SpotSubType.LEFT);
                    break;
                case CycleManager.Periode.AFTERNOON:
                    protectionType = plant._spotGroup.GetProtectionType(Spot.SpotType.TOP);
                    break;
                case CycleManager.Periode.EVENING:
                    protectionType = plant._spotGroup.GetProtectionType(Spot.SpotType.SIDE, Spot.SpotSubType.RIGHT);
                    break;
            }

            switch(protectionType) {
                case Spot.ProtectionType.LIGHT:
                    if(rayType == CycleManager.RayType.LIGHT)
                        continue;
                    break;
                case Spot.ProtectionType.DARKNESS:
                    if(rayType == CycleManager.RayType.DARKNESS)
                        continue;
                    break;
                case Spot.ProtectionType.ALL:
                    continue;
            }

            plant.AddEnergy(rayType);
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
