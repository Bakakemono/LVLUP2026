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


    public int GetInventory(Plant.PlantTypes type, Plant.PlantStates quality) {
        switch(type) {
            case Plant.PlantTypes.LUX:
                if(quality == Plant.PlantStates.WELL)
                    return _luxPoints;
                else if(quality == Plant.PlantStates.PERFECT)
                    return _luxPerfectPoints;
                break;

            case Plant.PlantTypes.NOX:
                if(quality == Plant.PlantStates.WELL)
                    return _noxPoints;
                else if(quality == Plant.PlantStates.PERFECT)
                    return _noxPerrfectPoints;
                break;

            case Plant.PlantTypes.MOTH_LUX:
                if(quality == Plant.PlantStates.WELL)
                    return _mothLuxPoints;
                else if(quality == Plant.PlantStates.PERFECT)
                    return _mothLuxPerfectPoints;
                break;

            case Plant.PlantTypes.MOTH_NOX:
                if(quality == Plant.PlantStates.WELL)
                    return _mothNoxPoints;
                else if(quality == Plant.PlantStates.PERFECT)
                    return _mothNoxPerfectPoints;
                break;
        }

        return -1;
    }

    public void AddToInventory(Plant.PlantTypes type, Plant.PlantStates quality, int value) {
        switch(type) {
            case Plant.PlantTypes.LUX:
                if(quality == Plant.PlantStates.WELL)
                    _luxPoints += value;
                else if(quality == Plant.PlantStates.PERFECT)
                    _luxPerfectPoints += value;
                break;

            case Plant.PlantTypes.NOX:
                if(quality == Plant.PlantStates.WELL)
                    _noxPoints += value;
                else if(quality == Plant.PlantStates.PERFECT)
                    _noxPerrfectPoints += value;
                break;

            case Plant.PlantTypes.MOTH_LUX:
                if(quality == Plant.PlantStates.WELL)
                    _mothLuxPoints += value;
                else if(quality == Plant.PlantStates.PERFECT)
                    _mothLuxPerfectPoints += value;
                break;

            case Plant.PlantTypes.MOTH_NOX:
                if(quality == Plant.PlantStates.WELL)
                    _mothNoxPoints += value;
                else if(quality == Plant.PlantStates.PERFECT)
                    _mothNoxPerfectPoints += value;
                break;
        }
    }

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
