using UnityEngine;

public class SpotsGroup : MonoBehaviour {
    [SerializeField] Spot _plantSpot;
    [SerializeField] Spot _leftSpot;
    [SerializeField] Spot _topSpot;
    [SerializeField] Spot _rightSpot;

    public Spot GetSpot(Spot.SpotType spotType) {
        switch(spotType) {
            case Spot.SpotType.NONE:
                Debug.LogWarning("This Spot has type !");
                break;
            case Spot.SpotType.PLANT:
                return _plantSpot;
                break;
            case Spot.SpotType.LEFT:
                return _leftSpot;
                break;
            case Spot.SpotType.RIGHT:
                return _rightSpot;
                break;
            case Spot.SpotType.TOP:
                return _topSpot;
                break;
        }
        Debug.LogWarning("Invalid Type");
        return null;
    }
}
