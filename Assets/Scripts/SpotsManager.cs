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

    // NEW
    SpotsGroup[] _spotsGroups;

    float _minCloseDistance = 1f;
    int _closestIndex = -1;
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

        // NEW
        _spotsGroups = FindObjectsByType<SpotsGroup>(FindObjectsSortMode.None);
    }


    public void HighlightValideSpot(Vector2 plantPos, Spot.SpotType type) {
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

        // NEW
        foreach(var spotsGroup in _spotsGroups) {
            spotsGroup.GetSpot(type).Fade();
        }


        _closestIndex = -1;
        for(int i = 0; i < spots.Count; i++) {
            if(spots[i].IsItTaken())
                continue;

            float dist = Vector2.SqrMagnitude((Vector2)spots[i].transform.position - plantPos);
            if(dist <= Mathf.Pow(_minCloseDistance, 2f)) {
                if(_closestIndex == -1)
                    _closestIndex = i;
                else if(dist < _closestDist) {
                    _closestDist = dist;
                    _closestIndex = i;
                }
            }
        }

        // NEW
        _closestIndex = -1;
        for(int i = 0; i < spots.Count; i++) {
            if(_spotsGroups[i].GetSpot(type).IsItTaken())
                continue;

            float dist = Vector2.SqrMagnitude((Vector2)_spotsGroups[i].GetSpot(type).transform.position - plantPos);
            if(dist <= Mathf.Pow(_minCloseDistance, 2f)) {
                if(_closestIndex == -1)
                    _closestIndex = i;
                else if(dist < _closestDist) {
                    _closestDist = dist;
                    _closestIndex = i;
                }
            }
        }

        if(_closestIndex != -1) {
            spots[_closestIndex].Highlight();
        }

        // NEW
        if(_closestIndex != -1) {
            _spotsGroups[_closestIndex].GetSpot(type).Highlight();
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

        // NEW
        foreach(var spotsGroup in _spotsGroups) {
            if(spotsGroup.GetSpot(type).IsItTaken()) {
                spotsGroup.GetSpot(type).Enable(false);
            }
            else {
                spotsGroup.GetSpot(type).Enable(enable);
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
        return _closestIndex == -1 ? null : spots[_closestIndex];

        // NEW
        return _spotsGroups[_closestIndex].GetSpot(type);
    }
}
