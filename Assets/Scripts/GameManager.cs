using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void SwitchToGame() {
        SceneManager.LoadScene("S_Game", LoadSceneMode.Single);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
