using UnityEngine;

public class Plant : MonoBehaviour
{
    public Transform _transform;
    Transform _sunTransform;

    int _lightGoal = 1;
    int _lightAborbed = 0;

    int _darknessGoal = 1;
    int _darknessAbsorbed = 0;

    private void Start() {
        _transform = transform;
        _sunTransform = FindFirstObjectByType<Sun>().transform;
    }

    public void AddLightPoint() {
        Debug.Log(gameObject.name + "Lighten Up");
        _lightAborbed++;
    }
}
