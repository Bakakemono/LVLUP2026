using UnityEngine;

public class SpotsGroup : MonoBehaviour {
    [SerializeField] Spot _plantSpot;
    [SerializeField] Spot _leftSpot;
    [SerializeField] Spot _topSpot;
    [SerializeField] Spot _rightSpot;



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
}
