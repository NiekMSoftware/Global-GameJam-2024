using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private int dialogueToActivate;
    [SerializeField] private int annoyedDialogue;

    //[SerializeField] Animator animator;

    public int GetDialogueToActivate()
    {
        //animator.SetBool("IsTalking", true);

        return dialogueToActivate;
    }

    public void OnEndDialogue(MissionManager missionManager)
    {
        missionManager.OnTalkToNPC(this);

        //animator.SetBool("IsTalking", false);

        dialogueToActivate = annoyedDialogue;
    }
}