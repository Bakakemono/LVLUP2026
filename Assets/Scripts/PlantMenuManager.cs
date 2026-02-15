using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlantMenuManager : MonoBehaviour {
    public static PlantMenuManager _instance;

    SpotsManager _spotsManager;

    [SerializeField] GameObject[] _plantsPrefabs;
    [SerializeField] GameObject[] _leftObstaclesPrefabs;
    [SerializeField] GameObject[] _rightObstaclesPrefabs;
    [SerializeField] GameObject[] _topObstaclesPrefabs;

    GameObject _selectedObject;
    bool _selectedObjectInHand = false;

    Vector2 _mouseWorldPos = Vector2.zero;

    InputSystem_Actions _inputSystem;

    [SerializeField] RectTransform _menuTransform;
    Vector3 _menuInitialpose;
    bool _hideMenu;

    Spot.SpotType _spotType;

    [SerializeField] LayerMask _recoltLayerMask;

    int _luxPoints = 0;
    int _luxPerfectPoints = 0;

    int _noxPoints = 0;
    int _noxPerrfectPoints = 0;

    int _mothNoxPoints = 0;
    int _mothNoxPerfectPoints = 0;

    int _mothLuxPointx = 0;
    int _mothLuxPerfectPoints = 0;

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

        _inputSystem.Player.Recolte.performed += Recolte;
        _inputSystem.Player.Recolte.Enable();
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

        if(!_selectedObjectInHand)
            return;

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);
        _selectedObject.transform.position = _mouseWorldPos;

        _spotsManager.UpdateSpots(_mouseWorldPos, _spotType);
    }


    public void SelectPlant(int index) {
        _selectedObject = Instantiate(_plantsPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.PLANT;
        _selectedObjectInHand = true;

        _spotsManager.EnableAllSpots(true, _spotType);
        _hideMenu = true;

    }

    public void SelectLeftObstacle(int index) {
        _selectedObject = Instantiate(_leftObstaclesPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.LEFT;
        _selectedObjectInHand = true;

        _spotsManager.EnableAllSpots(true, _spotType);
        _hideMenu = true;
    }

    public void SelectRightObstacle(int index) {
        _selectedObject = Instantiate(_rightObstaclesPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.RIGHT;
        _selectedObjectInHand = true;

        _spotsManager.EnableAllSpots(true, _spotType);
        _hideMenu = true;
    }

    public void SelectTopObstacle(int index) {
        _selectedObject = Instantiate(_topObstaclesPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.TOP;
        _selectedObjectInHand = true;

        _spotsManager.EnableAllSpots(true, _spotType);
        _hideMenu = true;
    }

    void PlacePlant(InputAction.CallbackContext obj) {
        if(!_selectedObjectInHand)
            return;

        Spot spot = _spotsManager.GetClosestSpot(_spotType);
        
        _selectedObjectInHand = false;
        _spotsManager.EnableAllSpots(false, _spotType);
        
        _hideMenu = false;

        if(spot == null) {
            Destroy(_selectedObject.gameObject);
        }
        else {
            if(_spotType == Spot.SpotType.PLANT) {
                PlantManager._instance.RegisterNewPlant(_selectedObject.GetComponent<Plant>());
                _selectedObject.GetComponent<Plant>()._occupiedSpot = spot;
            }
            else {
                _selectedObject.GetComponent<Obstacle>()._occupiedSpot = spot;
            }

                _selectedObject.transform.position = spot.transform.position;
            spot.OccupiedSpot();
            _selectedObject = null;
        }
        _spotType = Spot.SpotType.NONE;
    }

    void Recolte(InputAction.CallbackContext obj) {
        if(_selectedObjectInHand || GameManager._instance._starMoving)
            return;

        Vector2 screenMousePos = Mouse.current.position.ReadValue();
        _mouseWorldPos = Camera.main.ScreenToWorldPoint(screenMousePos);

        RaycastHit2D hit = Physics2D.Raycast(_mouseWorldPos, Vector2.down, 0.1f, _recoltLayerMask);
        
        if(hit.transform == null) return;

        Plant plant = hit.transform.GetComponent<Plant>();
        if(plant != null && !GameManager._instance._cycleInProgress) {
            plant._occupiedSpot.ReleaseSpot();
            Destroy(plant.gameObject);
        }
        else if(plant == null) {
            Obstacle obstacle = hit.transform.GetComponent<Obstacle>();
            obstacle._occupiedSpot.ReleaseSpot();
            Destroy(obstacle.gameObject);
        }
    }
}