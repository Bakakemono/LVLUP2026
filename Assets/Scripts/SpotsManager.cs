using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpotsManager : MonoBehaviour {
    Spot[] _spots;

    float _minCloseDistance = 1f;
    int _currentClosest = -1;
    float _closestDist = 0f;

    private void Start() {
        _spots = FindObjectsByType<Spot>(FindObjectsSortMode.None);
    }

    public void UpdateSpots(Vector2 plantPos) {
        foreach(var spot in _spots) {
            spot.Fade();
        }
        _currentClosest = -1;
        for(int i = 0; i < _spots.Length; i++) {
            if(_spots[i].IsItTaken())
                continue;

            float dist = Vector2.SqrMagnitude((Vector2)_spots[i].transform.position - plantPos);
            if(dist <= Mathf.Pow(_minCloseDistance, 2f)) {
                if(_currentClosest == -1)
                    _currentClosest = i;
                else if(dist < _closestDist) {
                    _closestDist = dist;
                    _currentClosest = i;
                }
            }
        }

        if(_currentClosest != -1) {
            _spots[_currentClosest].Highlight();
        }
    }

    public void EnableAllSpots(bool enable) {
        foreach (var spot in _spots) {
            if(spot.IsItTaken()) {
                spot.Enable(false);
            }
            else {
                spot.Enable(enable);
            }
        }
    }

    public Spot GetClosestSpot() {
        return _currentClosest == -1 ? null : _spots[_currentClosest];
    }
}
