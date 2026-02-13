using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour {
    public static PlantManager _instance;

    [SerializeField] List<Plant> _plantedPlants;
    Transform _sunTransform;
    Transform _moonTransform;

    [SerializeField] ContactFilter2D _contactFilter;

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

    private void Start() {
        
        _sunTransform = FindFirstObjectByType<Sun>().transform;
    }

    public void LightPlants() {
        
        foreach(var plant in _plantedPlants) {
            Ray2D ray = new Ray2D(_sunTransform.position, plant.transform.position - _sunTransform.position);
            Debug.DrawRay(_sunTransform.position, plant.transform.position - _sunTransform.position, Color.red, 1f);
            RaycastHit2D[] result = new RaycastHit2D[1];

            if(Physics2D.Raycast(_sunTransform.position, plant.transform.position - _sunTransform.position, _contactFilter, result, (plant.transform.position - _sunTransform.position).magnitude) > 0) {
                Debug.Log(result[0].transform.gameObject.name);

                plant.AddLightPoint();
            }
        }
    }
}
