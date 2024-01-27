using TMPro;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private TMP_Text interactionText;

    private bool interactable;

    private GameObject interactedObject;
    public Interactables interactables;

    public enum Interactables
    {
        npc,
    }

    private void Awake()
    {
        interactionText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("ontriggerenter!");
        if (other.CompareTag("NPC"))
        {
            interactionText.text = "E to talk with NPC";
            interactables = Interactables.npc;

            interactionText.gameObject.SetActive(true);
            interactedObject = other.gameObject;
            interactable = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactable && interactedObject)
        {
            interactionText.text = string.Empty;
            interactionText.gameObject.SetActive(false);

            if (interactables == Interactables.npc)
            {
                dialogueManager.StartDialogue(interactedObject.GetComponent<NPCDialogue>().GetDialogueToActivate());
            }
            interactable = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            if (!interactable)
            {
                dialogueManager.StopDialogue();
            }
            interactedObject = null;
            interactable = false;
            interactionText.text = string.Empty;
            interactionText.gameObject.SetActive(false);
        }
    }
}