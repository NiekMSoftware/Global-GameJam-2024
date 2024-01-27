using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float acceptDistance = 5f;

    [Header("Mission Update")]
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject missionUpdateUI;
    [SerializeField] private int missionUpdateLength = 3;

    [Header("Missions")]
    [SerializeField] private List<Mission> missions = new();

    [SerializeField] private int currentMissionIndex;

    [SerializeField] private List<string> missionUpdateUITexts = new();

    private MissionUpdateUI missionUpdateUIInstance;

    public enum MissionState
    {
        Inactive,
        Active,
        Finished
    }

    public enum UpdateCondition
    {
        TalkToNPC,
        KillEnemies,
        GoToLocation,
        PickUpItem,
    }

    [System.Serializable]
    public class Mission
    {
        public string title;
        public MissionState state = MissionState.Inactive;
        public List<MissionUpdateState> updateStates = new();
        public int updateStateNum;
    }

    [System.Serializable]
    public class MissionUpdateState
    {
        public UpdateCondition updateStates;
        public GameObject npc;
        public Transform location;
        public List<Enemy> enemiesToCure = new();
        [Multiline(3)]
        public string description;
        public Transform objPos;
        public QuestItem.ItemType itemType;
        public bool hasShownUI;
    }

    private void Start()
    {
        //Start mission 1
        UpdateMission(0);
    }

    private void Update()
    {
        CheckForUpdateMission();
    }

    public void UpdateMission(int index)
    {
        //if all submissions are completed, the mission is completed and set the missionstate to finished
        if (currentMissionIndex >= missions.Count) return;

        if (missions[index].updateStateNum >= missions[index].updateStates.Count)
        {
            missions[index].state = MissionState.Finished;
        }
        else
        {
            missions[index].state = MissionState.Active;
        }

        //if it's a new mission show ui that says new mission
        if (missions[index].updateStateNum <= 1 && missions[index].state != MissionState.Finished)
        {
            ShowUpdateMission("New Mission: \n" + missions[index].title + "\n" + missions[index].updateStates[missions[index].updateStateNum].description);
        }
        //if the mission is finished, show that info
        if (missions[index].state == MissionState.Finished)
        {
            ShowUpdateMission("Mission Finished: \n" + missions[index].title);
        }
        //if it's an existing mission show ui that updates existing information
        if (missions[index].state == MissionState.Active && !missions[index].updateStates[missions[index].updateStateNum].hasShownUI)
        {
            missions[index].updateStates[missions[index].updateStateNum].hasShownUI = true;
            ShowUpdateMission("Mission Update: \n" + missions[index].title + "\n" + missions[index].updateStates[missions[index].updateStateNum].description);
        }
    }

    private void IncreaseMissionUpdateState(int index)
    {
        missions[index].updateStateNum++;
        UpdateMission(index);
    }

    public void OnTalkToNPC(NPCDialogue npc)
    {
        Mission currentMission = missions[currentMissionIndex];

        MissionUpdateState currentSubMission = currentMission.updateStates[currentMission.updateStateNum];

        //Talked to NPC
        if (currentSubMission.updateStates == UpdateCondition.TalkToNPC && currentSubMission.npc == npc.gameObject)
        {
            currentMission.updateStateNum++;
            UpdateMission(currentMissionIndex);
        }
    }

    public void CheckForUpdateMission()
    {
        if (currentMissionIndex >= missions.Count) return;

        Mission currentMission = missions[currentMissionIndex];

        //Mission Completed
        if (currentMission.updateStateNum >= currentMission.updateStates.Count)
        {
            currentMissionIndex++;
            UpdateMission(currentMissionIndex);
            return;
        }

        MissionUpdateState currentSubMission = currentMission.updateStates[currentMission.updateStateNum];

        ////Killed enemy
        //else if (currentSubMission.updateStates == UpdateCondition.KillEnemies)
        //{
        //    bool isCorrectEnemy = false;

        //    foreach (var enemy in currentSubMission.enemies)
        //    {
        //        if (go == enemy)
        //        {
        //            isCorrectEnemy = true;
        //        }
        //    }

        //    if (isCorrectEnemy)
        //    {
        //        currentSubMission.currentEnemiesKilled++;

        //        if (currentSubMission.currentEnemiesKilled >= currentSubMission.enemies.Length)
        //        {
        //            IncreaseMissionUpdateState(i);
        //        }
        //    }
        //}

        //Went to location
        if (currentSubMission.updateStates == UpdateCondition.GoToLocation)
            if (Vector3.Distance(currentSubMission.location.position, player.position) <= acceptDistance)
                IncreaseMissionUpdateState(currentMissionIndex);

        if (currentSubMission.updateStates == UpdateCondition.PickUpItem)
            if (inventory.HasItem(currentSubMission.itemType))
                IncreaseMissionUpdateState(currentMissionIndex);
    }

    private void ShowUpdateMission(string text, bool queue = true)
    {
        if (missionUpdateUIInstance) Destroy(missionUpdateUIInstance.gameObject);

        if (queue)
            missionUpdateUITexts.Add(text);

        missionUpdateUIInstance = Instantiate(missionUpdateUI, canvas).GetComponent<MissionUpdateUI>();

        missionUpdateUIInstance.text.SetText(missionUpdateUITexts[0]);

        CancelInvoke();
        Invoke(nameof(RemoveMissionUpdateText), missionUpdateLength);
    }

    public void CuredEnemy(Enemy enemy)
    {
        Mission currentMission = missions[currentMissionIndex];

        MissionUpdateState currentSubMission = currentMission.updateStates[currentMission.updateStateNum];

        if (currentSubMission.updateStates == UpdateCondition.KillEnemies)
            if (currentSubMission.enemiesToCure.Count <= 0)
                IncreaseMissionUpdateState(currentMissionIndex);
    }

    private void RemoveMissionUpdateText()
    {
        Destroy(missionUpdateUIInstance.gameObject);
        missionUpdateUITexts.RemoveAt(0);

        if (missionUpdateUITexts.Count > 0)
        {
            ShowUpdateMission(missionUpdateUITexts[0], false);
        }
    }
}