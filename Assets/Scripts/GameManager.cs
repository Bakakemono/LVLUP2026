using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    List<Plant> _plantedPlants;
    
    private void Awake() {
        // Create an instance of the Gamemanager.
        if(_instance == null)
            _instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        _plantedPlants = new List<Plant>();
    }

    // Scene Management
    public void SwitchToGame() {
        SceneManager.LoadScene("S_Game", LoadSceneMode.Single);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void PingSunray() {
        PlantManager._instance.LightPlants();
    }
}
