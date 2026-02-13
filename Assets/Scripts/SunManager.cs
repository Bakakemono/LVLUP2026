using UnityEngine;

public class SunManager : MonoBehaviour {
    Transform _sunTransform;
    [SerializeField, Range(1f, 360f)]
    float _degPerSecond = 60f;

    private void Start() {
       
    }

    private void FixedUpdate() {
        //transform.Rotate(0f, 0f, -15f * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + -_degPerSecond * Time.fixedDeltaTime);
    }
}