using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlantMenuManager : MonoBehaviour {
    SpotsManager _spotsManager;


    [SerializeField] GameObject[] _plantsPrefabs;

    Plant _selectedPlant;
    bool _selectedPlantInHand = false;

    Vector2 _mouseWorldPos = Vector2.zero;

    InputSystem_Actions _inputSystem;

    private void Awake() {
        _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Click.performed += PlacePlant;
        _inputSystem.Player.Click.Enable();
    }

    void Start() {
        _spotsManager = FindFirstObjectByType<SpotsManager>();

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
    }

    public void SelectPlant(int index) {
        _selectedPlant = Instantiate(_plantsPrefabs[index], Vector2.one * 100, Quaternion.identity).GetComponent<Plant>();
        _selectedPlantInHand = true;

        _spotsManager.EnableAllSpots(true);
    }

    private void FixedUpdate() {
        if(!_selectedPlantInHand)
            return;

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
        _selectedPlant.transform.position = _mouseWorldPos;

        _spotsManager.UpdateSpots(_mouseWorldPos);
    }

    void PlacePlant(InputAction.CallbackContext obj) {
        if(!_selectedPlantInHand)
            return;

        Spot spot = _spotsManager.GetClosestSpot();
        
        _selectedPlantInHand = false;
        _spotsManager.EnableAllSpots(false);

        if(spot == null) {
            Destroy(_selectedPlant);
        }
        else {
            PlantManager._instance.RegisterNewPlant(_selectedPlant);
            _selectedPlant.transform.position = spot.transform.position;
            spot.OccupiedSpot();
            _selectedPlant = null;
        }
    }
}