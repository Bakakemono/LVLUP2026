using UnityEngine;

public class SpotsGroup : MonoBehaviour {
    [SerializeField] Spot _plantSpot;
    [SerializeField] Spot _leftSpot;
    [SerializeField] Spot _topSpot;
    [SerializeField] Spot _rightSpot;

    public bool _plantInPlace;

    public Spot[] GetSpot(Spot.SpotType spotType) {
        switch(spotType) {
            case Spot.SpotType.NONE:
                Debug.LogWarning("This Spot has type !");
                break;
            case Spot.SpotType.PLANT:
                return new Spot[1] { _plantSpot };

            case Spot.SpotType.TOP:
                return new Spot[1] { _topSpot };

            case Spot.SpotType.SIDE:
                return new Spot[2] { _leftSpot, _rightSpot };
        }
        Debug.LogWarning("Invalid Type");
        return null;
    }

    public Spot.ProtectionType GetProtectionType(Spot.SpotType spotType, Spot.SpotSubType subType = Spot.SpotSubType.NONE) {
        switch(spotType) {
            case Spot.SpotType.TOP:
                return _topSpot.GetProtectionType();
            case Spot.SpotType.SIDE:
                switch(subType) {
                    case Spot.SpotSubType.LEFT:
                        return _leftSpot.GetProtectionType();
                    case Spot.SpotSubType.RIGHT:
                        return _rightSpot.GetProtectionType();
                }
                break;
        }
        return Spot.ProtectionType.NONE;
    }

    public bool IsPlantInSpot() {
        return _plantInPlace;
    }

    public void PlantSet(bool isPlantSetup) {
        _plantInPlace = isPlantSetup;
    }
}
