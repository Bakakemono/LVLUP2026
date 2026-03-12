using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlantMenuManager : MonoBehaviour {
    public static PlantMenuManager _instance;

    SpotsManager _spotsManager;

    [SerializeField] GameObject[] _plantsPrefabs;
    [SerializeField] GameObject[] _topObstaclesPrefabs;
    [SerializeField] GameObject[] _sideObstaclesPrefabs;

    GameObject _selectedObject;
    bool _selectedObjectInHand = false;

    Vector2 _mouseWorldPos = Vector2.zero;

    InputSystem_Actions _inputSystem;

    [SerializeField] RectTransform _menuTransform;
    Vector3 _menuInitialpose;
    bool _hideMenu;

    Spot.SpotType _spotType;

    [SerializeField] LayerMask _recoltLayerMask;

    [SerializeField] Button _showButton;
    [SerializeField] Button _hideButton;

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

        _spotsManager.HighlightValideSpot(_mouseWorldPos, _spotType);
    }

    public void HideMenu() {
        _hideMenu = true;
        _showButton.transform.gameObject.SetActive(true);
        _hideButton.transform.gameObject.SetActive(false);
    }

    public void ShowMenu() {
        _hideMenu = false;
        _showButton.transform.gameObject.SetActive(false);
        _hideButton.transform.gameObject.SetActive(true);

    }

    public void SelectPlant(int index) {
        _selectedObject = Instantiate(_plantsPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.PLANT;
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

    public void SelectSideObstacle(int index) {
        _selectedObject = Instantiate(_sideObstaclesPrefabs[index], Vector2.one * 100, Quaternion.identity);
        _spotType = Spot.SpotType.SIDE;
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
                Plant newPlant = _selectedObject.GetComponent<Plant>();
                newPlant._spotGroup = spot._spotGroup;
                newPlant._occupiedSpot = spot;
                spot._spotGroup.PlantSet(true);
                
            }
            else {
                Obstacle obstacle = _selectedObject.GetComponent<Obstacle>();
                obstacle._occupiedSpot = spot;
                spot.SetProtectionType(obstacle._protectionType);
                if(_spotType == Spot.SpotType.SIDE) {
                    if(spot._spotSubType == Spot.SpotSubType.RIGHT) {
                        Vector2 scale = _selectedObject.transform.localScale;
                        _selectedObject.transform.localScale = new Vector2(-scale.x, scale.y);
                    }
                }
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
            PlantManager._instance.DeregisterPlant(plant);
            plant.Recolt();
        }
        else if(plant == null && !GameManager._instance._cycleInProgress) {
            Obstacle obstacle = hit.transform.GetComponent<Obstacle>();
            obstacle._occupiedSpot.ReleaseSpot();
            Destroy(obstacle.gameObject);
        }
    }
}