using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Quest {
    public Quest(List<Plant.PlantTypes> typeToOrder, List<Plant.PlantStates> qualityToOreder, List<int> quantityToOreder) {
        _typeToOrder = typeToOrder;
        _qualityToOreder = qualityToOreder;
        _quantityToOreder = quantityToOreder;
    }
    List<Plant.PlantTypes> _typeToOrder;
    List<Plant.PlantStates> _qualityToOreder;
    List<int> _quantityToOreder;
}
public class QuestManager : MonoBehaviour {
    List<Quest> _quests = new List<Quest>();
    int _questInProgress;

    private void Start() {
        List<Plant.PlantTypes> typeFirstOrder = new List<Plant.PlantTypes>();
        typeFirstOrder.Add(Plant.PlantTypes.LUX);
        
        List<Plant.PlantStates> qualityFirstOrder = new List<Plant.PlantStates>();
        qualityFirstOrder.Add(Plant.PlantStates.WELL);

        List<int> quantityFirstOrder = new List<int>();
        quantityFirstOrder.Add(1);

        _quests.Add(new Quest(typeFirstOrder, qualityFirstOrder, quantityFirstOrder));
    }
}