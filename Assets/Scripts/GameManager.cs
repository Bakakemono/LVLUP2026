using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    
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

    // Scene Management
    public void StartGame() {
        SceneManager.LoadScene("S_Game", LoadSceneMode.Single);
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void PingSunray(bool sunLight) {
        PlantManager._instance.LightPlants(sunLight);
    }
}
