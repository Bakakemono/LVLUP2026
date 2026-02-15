using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public bool _cycleInProgress = false;
    public bool _starMoving = false;


    public int _luxPoints = 0;
    public int _luxPerfectPoints = 0;
    
    public int _noxPoints = 0;
    public int _noxPerrfectPoints = 0;
    
    public int _mothNoxPoints = 0;
    public int _mothNoxPerfectPoints = 0;
    
    public int _mothLuxPoints = 0;
    public int _mothLuxPerfectPoints = 0;


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

    public void PingAstreEffect(bool sunLight) {
        PlantManager._instance.LightPlants(sunLight);
    }
}
