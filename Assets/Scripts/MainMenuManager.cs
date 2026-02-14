using UnityEngine;

public class MainMenuManager : MonoBehaviour {
    public void Play() {
        GameManager._instance.StartGame();
    }

    public void Quit() {
        GameManager._instance.ExitGame();
    }
}