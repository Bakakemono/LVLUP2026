using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class SpotsManager : MonoBehaviour {
    List<Spot> _plants = new List<Spot>();
    List<Spot> _left = new List<Spot>();
    List<Spot> _right = new List<Spot>();
    List<Spot> _top = new List<Spot>();

    float _minCloseDistance = 1f;
    int _currentClosest = -1;
    float _closestDist = 0f;

    private void Start() {

        Spot[] spots = FindObjectsByType<Spot>(FindObjectsSortMode.None);

        foreach(var spot in spots) {
            switch(spot._spotType) {
                case Spot.SpotType.PLANT:
                    _plants.Add(spot);
                    break;
                case Spot.SpotType.LEFT:
                    _left.Add(spot);
                    break;
                case Spot.SpotType.RIGHT:
                    _right.Add(spot);
                    break;
                case Spot.SpotType.TOP:
                    _top.Add(spot);
                    break;
            }
        }
    }

    public void UpdateSpots(Vector2 plantPos, Spot.SpotType type) {
        List<Spot> spots = new List<Spot>();

        switch(type) {
            case Spot.SpotType.PLANT:
                spots = _plants;
                break;
            case Spot.SpotType.LEFT:
                spots = _left;
                break;
            case Spot.SpotType.RIGHT:
                spots = _right;
                break;
            case Spot.SpotType.TOP:
                spots = _top;
                break;
        }

        foreach(var spot in spots) {
            spot.Fade();
        }
        _currentClosest = -1;
        for(int i = 0; i < spots.Count; i++) {
            if(spots[i].IsItTaken())
                continue;

            float dist = Vector2.SqrMagnitude((Vector2)spots[i].transform.position - plantPos);
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
            spots[_currentClosest].Highlight();
        }
    }

    public void EnableAllSpots(bool enable, Spot.SpotType type) {
        List<Spot> spots = new List<Spot>();

        switch(type) {
            case Spot.SpotType.PLANT:
                spots = _plants;
                break;
            case Spot.SpotType.LEFT:
                spots = _left;
                break;
            case Spot.SpotType.RIGHT:
                spots = _right;
                break;
            case Spot.SpotType.TOP:
                spots = _top;
                break;
        }

        foreach (var spot in spots) {
            if(spot.IsItTaken()) {
                spot.Enable(false);
            }
            else {
                spot.Enable(enable);
            }
        }
    }

    public Spot GetClosestSpot(Spot.SpotType type) {
        List<Spot> spots = new List<Spot>();

        switch(type) {
            case Spot.SpotType.PLANT:
                spots = _plants;
                break;
            case Spot.SpotType.LEFT:
                spots = _left;
                break;
            case Spot.SpotType.RIGHT:
                spots = _right;
                break;
            case Spot.SpotType.TOP:
                spots = _top;
                break;
        }
        return _currentClosest == -1 ? null : spots[_currentClosest];
    }
}
