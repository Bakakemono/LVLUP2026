using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class SpotsManager : MonoBehaviour {
    SpotsGroup[] _spotsGroups;

    float _minCloseDistance = 1f;
    int _closestIndex = -1;
    float _closestDist = 0f;

    private void Start() {
        _spotsGroups = FindObjectsByType<SpotsGroup>(FindObjectsSortMode.None);
    }


    public void HighlightValideSpot(Vector2 objectPos, Spot.SpotType type) {
        foreach(var spotsGroup in _spotsGroups) {
            foreach(Spot spot in spotsGroup.GetSpot(type))
                spot.Fade();
        }

        List<Spot> spots = GetSpots(type);

        _closestIndex = -1;
        for(int i = 0; i < spots.Count; i++) {
            if(spots[i].IsItTaken())
                continue;

            float dist = Vector2.SqrMagnitude((Vector2)spots[i].transform.position - objectPos);
            if(dist <= Mathf.Pow(_minCloseDistance, 2f)) {
                if(_closestIndex == -1) {
                    _closestDist = dist;
                    _closestIndex = i;
                }
                else if(dist < _closestDist) {
                    _closestDist = dist;
                    _closestIndex = i;
                }
            }
        }

        if(_closestIndex != -1) {
            spots[_closestIndex].Highlight();
        }
    }

    public void EnableAllSpots(bool enable, Spot.SpotType type) {
        List<Spot> spots = GetSpots(type);

        foreach(var spot in spots) {
            if(spot.IsItTaken()) {
                spot.Enable(false);
            }
            else {
                spot.Enable(enable);
            }
        }
    }

    public Spot GetClosestSpot(Spot.SpotType type) {
        List<Spot> spots = GetSpots(type);

        return spots[_closestIndex];
    }

    List<Spot> GetSpots(Spot.SpotType type) {
        List<Spot> spots = new List<Spot>();

        foreach(var spotGroup in _spotsGroups)
            foreach(var spot in spotGroup.GetSpot(type))
                spots.Add(spot);

        return spots;
    }
}
