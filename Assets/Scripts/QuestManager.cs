using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[System.Serializable]
struct Order {
    public Order(Plant.PlantTypes plantType, Plant.PlantStates plantState, int quantity) {
        _plantType = plantType;
        _plantState = plantState;
        _quantity = quantity;
    }

    public Plant.PlantTypes _plantType;
    public Plant.PlantStates _plantState;
    public int _quantity;
}

[System.Serializable]
struct Quest {
    public Quest(Order[] orders) {
        _orders = orders;
    }
    public Order[] _orders;
}

[System.Serializable]
struct SavedQuests {
    public List<Quest> _quests;
}

public class QuestManager : MonoBehaviour {
    [SerializeField] bool _createQuest = false;

    [SerializeField] bool _saveQuestFile = false;

    [SerializeField] bool _LoadQuests = false;

    [SerializeField] List<Quest> _quests = new List<Quest>();
    int _questInProgress;
    private static string PATH => Application.persistentDataPath + @"\Data.json";

    private void FixedUpdate() {
        if(_createQuest) {
            _createQuest = false;
            CreateQuest();
        }
        if(_saveQuestFile) {
            _saveQuestFile = false;
            SaveQuestToFile();
            Debug.Log($"Save Path : {PATH}");
        }
        if(_LoadQuests) {
            _LoadQuests = false;
            LoadQuestsFromFile();
            Debug.Log($"Load Path : {PATH}");
        }
    }

    void CreateQuest() {
        List<Plant.PlantTypes> typeFirstOrder = new List<Plant.PlantTypes>();
        typeFirstOrder.Add(Plant.PlantTypes.LUX);

        List<Plant.PlantStates> qualityFirstOrder = new List<Plant.PlantStates>();
        qualityFirstOrder.Add(Plant.PlantStates.WELL);

        List<int> quantityFirstOrder = new List<int>();
        quantityFirstOrder.Add(1);

        Order firstOrder = new Order(Plant.PlantTypes.LUX, Plant.PlantStates.WELL, 1);
        Order secondOrder = new Order(Plant.PlantTypes.NOX, Plant.PlantStates.WELL, 1);

        _quests.Add(new Quest(new Order[] { firstOrder, secondOrder }));

    }

    void SaveQuestToFile() {
        SavedQuests savedQuests;
        savedQuests._quests = _quests;

        string json = JsonUtility.ToJson(savedQuests, true);
        File.WriteAllText(PATH, json);
    }

    void LoadQuestsFromFile() {
        string json = File.ReadAllText(PATH);
        _quests = JsonUtility.FromJson<SavedQuests>(json)._quests;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}