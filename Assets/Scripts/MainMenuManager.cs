using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    [SerializeField] Image _tuto1;
    [SerializeField] Image _tuto2;

    bool _tutoStarted = false;
    bool _secondPageLoaded = false;

    InputSystem_Actions _inputSystem;

    void Awake() {
        _inputSystem = new InputSystem_Actions();

        _inputSystem.Player.Click.performed += ManageTuto;
        _inputSystem.Player.Click.Enable();
    }

    private void OnEnable() {
        _inputSystem.Player.Click.Enable();
    }

    private void OnDisable() {
        _inputSystem.Player.Click.Disable();
    }

    public void Play() {
        GameManager._instance.StartGame();
    }

    public void Tuto() {
        _tuto1.gameObject.SetActive(true);
        _tutoStarted = true;
    }

    public void Quit() {
        GameManager._instance.ExitGame();
    }

    void ManageTuto(InputAction.CallbackContext obj) {
        if(!_tutoStarted)
            return;

        if(!_secondPageLoaded) {
            _tuto1.gameObject.SetActive(false);
            _tuto2.gameObject.SetActive(true);
            _secondPageLoaded = true;
        }
        else {
            _tuto2.gameObject.SetActive(false);
            _secondPageLoaded = false;
            _tutoStarted = false;
        }

    }
}