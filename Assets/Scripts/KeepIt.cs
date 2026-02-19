using UnityEngine;

public class KeepIt : MonoBehaviour {
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
}