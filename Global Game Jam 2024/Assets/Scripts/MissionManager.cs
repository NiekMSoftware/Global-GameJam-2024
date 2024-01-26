using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MissionManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float acceptDistance = 5f;

    [Header("Mission Update")]
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject missionUpdateUI;
    [SerializeField] private int missionUpdateLength = 3;

    [Header("Missions")]
    [SerializeField] private List<Mission> missions = new();

    private int currentMissionIndex;

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
        GoToLocation
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
        public GameObject[] enemies;
        public Transform location;
        [System.NonSerialized] public int currentEnemiesKilled;
        [Multiline(3)]
        public string description;
        public Transform objPos;
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
        if (missions[index].updateStateNum >= missions[index].updateStates.Count)
        {
            missions[index].state = MissionState.Finished;
        }
        else
        {
            missions[index].state = MissionState.Active;
        }

        //if it's a new mission show ui that says new mission
        if (missions[index].updateStateNum <= 1)
        {
            ShowUpdateMission("New Mission: \n" + missions[index].title + "\n" + missions[index].updateStates[missions[index].updateStateNum].description);
        }
        //if it's an existing mission show ui that updates existing information
        else if (missions[index].state == MissionState.Active)
        {
            ShowUpdateMission("Mission Update: \n" + missions[index].title + "\n" + missions[index].updateStates[missions[index].updateStateNum].description);
        }
        //if the mission is finished, show that info
        else if (missions[index].state == MissionState.Finished)
        {
            ShowUpdateMission("Mission Finished: \n" + missions[index].title);
        }
    }

    private void IncreaseMissionUpdateState(int index)
    {
        missions[index].updateStateNum++;
        UpdateMission(index);
    }

    public void CheckForUpdateMission()
    {
        Mission currentMission = missions[currentMissionIndex];

        //Mission Completed
        if (currentMission.updateStates.Count == currentMission.updateStateNum)
        {
            currentMissionIndex++;
            return;
        }

        MissionUpdateState currentSubMission = currentMission.updateStates[currentMission.updateStateNum];

        //Talked to npc
        //if (currentSubMission.updateStates == UpdateCondition.TalkToNPC && go == currentSubMission.npc)
        //{
        //    IncreaseMissionUpdateState(i);
        //}
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
        if (Vector3.Distance(currentSubMission.location.position, player.position) <= acceptDistance)
        {
            print("player close to target");
            IncreaseMissionUpdateState(currentMissionIndex);
        }
    }

    private void ShowUpdateMission(string text)
    {
        if (missionUpdateUIInstance) Destroy(missionUpdateUIInstance);

        missionUpdateUIInstance = Instantiate(missionUpdateUI, canvas).GetComponent<MissionUpdateUI>();

        missionUpdateUIInstance.text.SetText(text);

        Invoke(nameof(RemoveMissionUpdateText), missionUpdateLength);
    }

    private void RemoveMissionUpdateText()
    {
        Destroy(missionUpdateUIInstance.gameObject);
    }

    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(MissionManager))]
    public class MissionManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MissionManager missionManager = (MissionManager)target;

            //if (npcDialogue.mission == MissionToDo.Update)
            //{
            //    EditorGUILayout.LabelField("Mission Index");
            //    npcDialogue.missionIndex = EditorGUILayout.IntField(npcDialogue.missionIndex);
            //}
            //else if (npcDialogue.mission == MissionToDo.Add)
            //{
            //    EditorGUILayout.LabelField("Mission Title");
            //    npcDialogue.title = EditorGUILayout.TextField(npcDialogue.title);
            //    EditorGUILayout.LabelField("Mission Description");
            //    npcDialogue.description = EditorGUILayout.TextField(npcDialogue.description);
            //    EditorGUILayout.LabelField("Mission Location");
            //    npcDialogue.posTransform = EditorGUILayout.ObjectField("posTransform", npcDialogue.posTransform, typeof(Transform), true) as Transform;
            //}

            //if (missionManager.missions[0].updateStates[0].updateStates == UpdateState.TalkToNPC)
            //{
            //    SerializedProperty npc = serializedObject.FindProperty("npc");
            //    EditorGUILayout.ObjectField(npc, new GUIContent("Title", "Description"));
            //}
        }
    }
#endif
    #endregion
}