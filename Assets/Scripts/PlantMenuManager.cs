using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlantMenuManager : MonoBehaviour {
    public static PlantMenuManager _instance;

    SpotsManager _spotsManager;


    [SerializeField] GameObject[] _plantsPrefabs;
    [SerializeField] GameObject[] _allObstaclesPrefabs;
    [SerializeField] GameObject[] _lightObstaclesPrefabs;
    [SerializeField] GameObject[] _darknessObstaclesPrefabs;

    Plant _selectedPlant;
    bool _selectedPlantInHand = false;

    Vector2 _mouseWorldPos = Vector2.zero;

    InputSystem_Actions _inputSystem;

    [SerializeField] RectTransform _menuTransform;
    Vector3 _menuInitialpose;
    bool _hideMenu;
    
    

    private void Awake() {
        if(_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }

            _inputSystem = new InputSystem_Actions();
        _inputSystem.Player.Click.performed += PlacePlant;
        _inputSystem.Player.Click.Enable();
    }

    void Start() {
        _spotsManager = FindFirstObjectByType<SpotsManager>();

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);

        _menuInitialpose = _menuTransform.anchoredPosition;
    }
    private void FixedUpdate() {
        if(!Mathf.Approximately(_menuTransform.anchoredPosition.x, (_hideMenu ? -1f : 1f) * _menuInitialpose.x)) {  
            _menuTransform.anchoredPosition = Vector3.Lerp(_menuTransform.anchoredPosition, (_hideMenu ? -1f : 1f) * _menuInitialpose, 0.2f);
        }

        if(!_selectedPlantInHand)
            return;

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
        _selectedPlant.transform.position = _mouseWorldPos;

        _spotsManager.UpdateSpots(_mouseWorldPos);
    }


    public void SelectPlant(int index) {
        _selectedPlant = Instantiate(_plantsPrefabs[index], Vector2.one * 100, Quaternion.identity).GetComponent<Plant>();
        _selectedPlantInHand = true;

        _spotsManager.EnableAllSpots(true);
        _hideMenu = true;
    }

    void PlacePlant(InputAction.CallbackContext obj) {
        if(!_selectedPlantInHand)
            return;

        Spot spot = _spotsManager.GetClosestSpot();
        
        _selectedPlantInHand = false;
        _spotsManager.EnableAllSpots(false);
        _hideMenu = false;

        if(spot == null) {
            Destroy(_selectedPlant.gameObject);
        }
        else {
            PlantManager._instance.RegisterNewPlant(_selectedPlant);
            _selectedPlant.transform.position = spot.transform.position;
            spot.OccupiedSpot();
            _selectedPlant = null;
        }
    }
}